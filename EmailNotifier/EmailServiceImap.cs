using MailKit.Net.Imap;
using MailKit;
using System;
using System.Collections.Generic;
using MailKit.Security;
using MimeKit;
using MailKit.Search;

namespace EmailNotifier
{
    public class EmailServiceImap : EmailService, IEmailService
    {
        private readonly IEmailAccountConfiguration emailAccountConfiguration;
        private ImapClient emailClient;
        private bool connected = false;

        public EmailServiceImap(IEmailAccountConfiguration emailAccountConfiguration)
        {
            //emailClient = new ImapClient(new ProtocolLogger("imap.log"));
            emailClient = new ImapClient();
            this.emailAccountConfiguration = emailAccountConfiguration;
        }



        #region Region - łączenie z serwerem



        /// <summary>
        /// zwraca true jeżeli uzyskano połączenie
        /// </summary>
        /// <param name="emailClient"></param>
        /// <returns></returns>
        private bool connectToServer()
        {
            try
            {
                string srvUrl = emailAccountConfiguration.receiveServer.TryGetUrl();
                this.emailClient.Connect(srvUrl, emailAccountConfiguration.receiveServer.port, SecureSocketOptions.SslOnConnect);
                this.emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                this.emailClient.Authenticate(emailAccountConfiguration.username, emailAccountConfiguration.password);
                connected = true;
            }
            catch (NotSupportedException exc)
            {
                MyMessageBox.display(exc.Message + "\r\n" + emailAccountConfiguration.receiveServer.url, MyMessageBoxType.Error);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                MyMessageBox.display(e.Message + "\r\n" + emailAccountConfiguration.receiveServer.url, MyMessageBoxType.Error);
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                MyMessageBox.display(ex.Message + "\r\n" + emailAccountConfiguration.receiveServer.url, MyMessageBoxType.Error);
            }

            return this.connected;
        }




        #endregion



        #region Region - czytanie wiadomości z serwera

        /// <summary>
        /// czyta określoną liczbę najnowszych emaili z serwera
        /// </summary>
        /// <param name="numberOfMessagesToReceive"> liczba wiadomości do przeczytania z serwera</param>
        /// <returns></returns>

        public override LinkedList<IEmailMessage> ReceiveEmails(int numberOfMessagesToReceive)
        {
            getMessages(numberOfMessagesToReceive);
            if (connected) this.emailClient.Disconnect(true);

            return this.emailsReceived;
        }




        /// <summary>
        /// czyta z serwera wszystkie maile nowsze od podanego
        /// </summary>
        /// <param name="newestEmail"></param>
        /// <returns></returns>
        public override LinkedList<IEmailMessage> ReceiveEmails(IEmailMessage newestEmail)
        {
            getMessages(newestEmail);
            if (connected) this.emailClient.Disconnect(true);

            return this.emailsReceived;
        }



        private void getMessages(int numberOfMessagesToReceive)
        {
            try
            {
                if (connectToServer())
                {
                    emailClient.Inbox.Open(FolderAccess.ReadOnly);
                    int numberOfMessagesOnServer = emailClient.Inbox.Count;

                    if(numberOfMessagesOnServer == 0)
                    {
                        throw new EmailServiceException("brak wiadomości na serwerze " + emailAccountConfiguration.receiveServer.url + " emailClient.IsConnected " + emailClient.IsConnected);
                    }
                    for (int i = numberOfMessagesOnServer - 1; i >= 0 && i > (numberOfMessagesOnServer - 1 - numberOfMessagesToReceive); i--)
                    {
                        IEmailMessage emailMessage = getOneMessage(i);
                        emailsReceived.AddLast(emailMessage);
                    }
                    //testMethod();
                }
            }
            catch (ImapProtocolException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
        }


        private void getMessages(IEmailMessage benchmarkEmail)
        {
            try
            {
                if (connectToServer())
                {
                    emailClient.Inbox.Open(FolderAccess.ReadWrite);
                    int numberOfMessagesOnServer = emailClient.Inbox.Count;

                    if (numberOfMessagesOnServer == 0)
                    {
                        throw new EmailServiceException("brak wiadomości na serwerze " + emailAccountConfiguration.receiveServer.url + " emailClient.IsConnected " + emailClient.IsConnected);
                    }

                    int messageIndex = numberOfMessagesOnServer - 1;                //index ostatniego, tj najnowszego, maila na serwerze
                    IEmailMessage emailMessage;

                    do
                    {
                        emailMessage = getOneMessage(messageIndex);
                        tryAddToNewEmailsList(benchmarkEmail, emailMessage);
                        messageIndex--;
                    }

                    while (conditionContinueGettingEmails(benchmarkEmail, emailMessage));
                }

            }
            catch (ImapProtocolException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
        }


        protected override IEmailMessage getOneMessage(int messageIndex)
        {
            var message = this.emailClient.Inbox.GetMessage(messageIndex);

            IEmailMessage emailMessage = createOneEmailMessage(messageIndex, message);
            if (message.Sender != null)
                emailMessage.SenderAddress = new EmailAddress(message.Sender.Name, message.Sender.Address);

            return emailMessage;
        }

        private void testMethod()
        {
            // search for messages where the Subject header contains either "MimeKit" or "MailKit"
            DateTime date = new DateTime(2020, 1, 21, 8, 30, 52);
            var query = SearchQuery.SentOn(date);//.SubjectContains("certyfikat").Or(SearchQuery.SubjectContains("idealna"));
            var uids = emailClient.Inbox.Search(query);
            var items = emailClient.Inbox.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.InternalDate );
            foreach (var item in items)
            {

                // IMessageSummary.TextBody is a convenience property that finds the 'text/plain' body part for us
                var bodyPart = item.TextBody;

                var title = item.InternalDate;
                // download the 'text/plain' body part
                var body = (TextPart)emailClient.Inbox.GetBodyPart(item.UniqueId, bodyPart);

                // TextPart.Text is a convenience property that decodes the content and converts the result to
                // a string for us
                var text = body.Text;
            }
        }


        #endregion



        /// <summary>
        /// najpierw czyta z serwera emaile nowsze od podanego a następnie kasuje z serwera przekazane emaile
        /// wykorzystując to samo połączenie
        /// </summary>
        /// <param name="lastEmail"></param>
        /// <param name="emailsToDelete"></param>
        /// <returns></returns>
        public override LinkedList<IEmailMessage> ReceiveAndDelete(IEmailMessage newestEmail, IList<IEmailMessage> emailsToDelete = null)
        {
            getMessages(newestEmail);
            deleteEmails(emailsToDelete);

            if (connected) this.emailClient.Disconnect(true);

            return this.emailsReceived;
        }



        #region Region - usuwanie emaili z serwera


        private void deleteEmails(IList<IEmailMessage> emailsToDelete)
        {
            if (emailsToDelete != null && emailsToDelete.Count > 0)
            {
                try
                {
                    //z listy tworzę słownik żeby ułatwić wyszukiwanie wiadomości
                    //teoretycznie wiadomość może być usunięta na serwerze w inny sposób pomiędzy czasem kiedy została zaznaczona do usunięcia w programie
                    //czyli została dodana do listy wiadomości do usunięcia
                    //a zanim została usunięta w pętli poniżej, więc pętlę muszę zatrzymać gdy dojdę do wiadomości na serwerze, która jest starsza niż
                    //najstarsza wiadomość przekazana do usunięcia

                    IEmailMessage oldestMessage = getOldestEmail(emailsToDelete);

                    Dictionary<string, IEmailMessage> emailsToDeleteDict = constructEmailToDeleteDict(emailsToDelete);                    

                    int numberOfMessagesOnServer = emailClient.Inbox.Count;
                    IMailFolder trash;
                    try
                    {
                        trash = emailClient.GetFolder(SpecialFolder.Trash);

                    }
                    catch (NotSupportedException e)
                    {

                        throw new EmailServiceException("The email service does not support moving emails to Trash " + emailAccountConfiguration.receiveServer.url, e);
                    }

                    int messageIndex = numberOfMessagesOnServer - 1;                //index ostatniego, tj najnowszego, maila na serwerze
                    MimeMessage emailMessage;

                    do
                    {
                        emailMessage = emailClient.Inbox.GetMessage(messageIndex);

                        if (emailsToDeleteDict.ContainsKey(emailMessage.MessageId))
                        {
                            emailClient.Inbox.MoveTo(messageIndex, trash); // .AddFlags(messageIndex, MessageFlags.Deleted,true);
                            emailsToDeleteDict.Remove(emailMessage.MessageId);
                        }
                        messageIndex--;
                    }
                    //teoretycznie wiadomość może być usunięta na serwerze w inny sposób pomiędzy czasem kiedy została zaznaczona do usunięcia w programie 
                    //a zanim została usunięta w tej pętli, więc pętlę muszę zatrzymać gdy dojdę do wiadomości na serwerze, 
                    //która jest starsza od najstarszej przekazanej do skasowania
                    while (emailsToDeleteDict.Count > 0 && emailMessage.Date >= oldestMessage.DateTime && messageIndex > 0);
                }
                catch (ImapProtocolException e)
                {
                    throw new EmailServiceException("Email service error " + emailAccountConfiguration.receiveServer.url, e);
                }
                catch (MailKit.ServiceNotConnectedException e)
                {
                    throw new EmailServiceException("Email service error " + emailAccountConfiguration.receiveServer.url, e);
                }
            }
        }


        public override void DeleteEmails(IList<IEmailMessage> emailsToDelete)
        {
            try
            {
                if (connectToServer())
                {
                    deleteEmails(emailsToDelete);
                    if (connected) this.emailClient.Disconnect(true);
                }
            }
            catch (ImapProtocolException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
        }

        #endregion






        public override void SendEmails(IList<IEmailMessage> emailMessages)
        {
            throw new NotImplementedException();
        }
    }
}
