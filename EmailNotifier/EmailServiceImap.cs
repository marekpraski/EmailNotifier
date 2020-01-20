using MailKit.Net.Imap;
using MailKit;
using System;
using System.Collections.Generic;
using MailKit.Security;
using MimeKit;

namespace EmailNotifier
{
    public class EmailServiceImap : EmailService, IEmailService
    {
        private readonly LinkedList<IEmailMessage> emailsReceived = new LinkedList<IEmailMessage>();
        private readonly IEmailAccountConfiguration emailAccountConfiguration;
        private ImapClient emailClient;
        private bool connected = false;

        public EmailServiceImap(IEmailAccountConfiguration emailAccountConfiguration)
        {
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
            string newestEmailId = newestEmail.messageId;
            DateTime newestEmailDateTime = newestEmail.messageDateTime;
            getMessages(newestEmailId, newestEmailDateTime);
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
                        EmailMessage emailMessage = getOneMessage(i);
                        emailsReceived.AddLast(emailMessage);
                    }
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


        private void getMessages(string newestEmailId, DateTime newestEmailDateTime)
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
                    EmailMessage emailMessage;

                    do
                    {
                        emailMessage = getOneMessage(messageIndex);

                        //sprawdzenie po ID wiadomości działa tylko wtedy, gdy w międzyczasie nie usunąłem z serwera wiadomości nowszych, niż ostatnio wczytana
                        //dlatego sprawdzam też po dacie, wczytuję tylko wiadomości młodsze od ostatniej, którą mam w bazie
                        bool compareId = emailMessage.messageId != newestEmailId;
                        bool compareDateTime = emailMessage.messageDateTime >= newestEmailDateTime;

                        if (emailMessage.messageId != newestEmailId && emailMessage.messageDateTime >= newestEmailDateTime)
                        {
                            this.emailsReceived.AddLast(emailMessage);
                        }
                        messageIndex--;
                    }
                    //wiadomości czytam od najnowszej, aż dojdę do tej, którą już mam w bazie. Ale ...
                    //wiadomość może być usunięta na serwerze zanim została zaczytana w programie, więc wiadomości nie będzie wtedy w bazie programu
                    while (emailMessage.messageDateTime > newestEmailDateTime);
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


        private EmailMessage getOneMessage(int messageIndex)
        {
            var message = this.emailClient.Inbox.GetMessage(messageIndex);
            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = message.Subject,
                messageId = message.MessageId,
                FromAddress = message.From.ToString(),
                messageDateTime = message.Date.LocalDateTime,
                Content = message.TextBody
            };
            if (message.Sender != null)
                emailMessage.SenderAddress = new EmailAddress(message.Sender.Name, message.Sender.Address);

            return emailMessage;
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
            string newestEmailId = newestEmail.messageId;
            DateTime newestEmailDateTime = newestEmail.messageDateTime;
            getMessages(newestEmailId, newestEmailDateTime);
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
                    while (emailsToDeleteDict.Count > 0 && emailMessage.Date >= oldestMessage.messageDateTime && messageIndex > 0);
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
