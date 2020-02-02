using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace EmailNotifier
{
    /// <summary>
    /// klasa odpowiedzialna za łączenie się z serwerem poczty email i ściąganie/wysyłanie emaili
    /// klasa używa bibliotekę MailKit
    /// </summary>
    public class EmailServicePop : EmailService, IEmailService
    {
        private IEmailAccountConfiguration emailAccountConfiguration;

        private Pop3Client emailClient;
        private bool connected = false;

        public EmailServicePop(IEmailAccountConfiguration emailAccountConfiguration)
        {
            this.emailAccountConfiguration = emailAccountConfiguration;
            emailClient = new Pop3Client();
            emailsReceived.Clear();
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
                this.emailClient.Connect(srvUrl, emailAccountConfiguration.receiveServer.port, emailAccountConfiguration.receiveServer.useAuthorisation);
                this.emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                this.emailClient.Authenticate(emailAccountConfiguration.username, emailAccountConfiguration.password);
                connected = true;
            }
            catch (MailKit.Security.SslHandshakeException exc)
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
            getEmails(newestEmail);
            if (connected) this.emailClient.Disconnect(true);

            return this.emailsReceived;
        }



        private void getMessages(int numberOfMessagesToReceive)
        {
            try
            {
                if (connectToServer()) {

                    int numberOfMessagesOnServer = emailClient.GetMessageCount();
                    if (numberOfMessagesOnServer == 0)
                    {
                        throw new EmailServiceException("brak wiadomości na serwerze " + "emailClient.IsConnected " + emailClient.IsConnected);
                    }

                    for (int i = numberOfMessagesOnServer - 1; i >= 0 && i > (numberOfMessagesOnServer - 1 - numberOfMessagesToReceive); i--)
                    {
                        IEmailMessage emailMessage = getOneEmail(i);
                        emailsReceived.AddLast(emailMessage);
                    }
                }
            }
            catch (MailKit.Net.Pop3.Pop3ProtocolException e)
            {                
                throw new EmailServiceException("Email service error", e);
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
        }

 
        private void getEmails(IEmailMessage benchmarkEmail)
        {
            try
            {
                if (connectToServer())
                {
                    int numberOfEmailsOnServer = emailClient.GetMessageCount();
                    if (numberOfEmailsOnServer == 0)
                    {
                        throw new EmailServiceException("brak wiadomości na serwerze " + emailAccountConfiguration.receiveServer.url + " emailClient.IsConnected " + emailClient.IsConnected);
                    }

                    int emailIndex = numberOfEmailsOnServer - 1;                //index ostatniego, tj najnowszego, maila na serwerze
                    IEmailMessage emailMessage;

                    do
                    {
                        emailMessage = getOneEmail(emailIndex);
                        tryAddToNewEmailsList(benchmarkEmail, emailMessage);
                        emailIndex--;
                    }
                    while (conditionContinueGettingEmails(benchmarkEmail, emailMessage));
                }

            }
            catch (MailKit.Net.Pop3.Pop3ProtocolException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                throw new EmailServiceException("Email service error", e);
            }          
        }


        protected override IEmailMessage getOneEmail(int emailIndex)
        {
            var mimeMessage = this.emailClient.GetMessage(emailIndex);
            IEmailMessage emailMessage = createOneEmailMessage(emailIndex, mimeMessage);
            if (mimeMessage.Sender != null)
                emailMessage.SenderAddress = new EmailAddress(mimeMessage.Sender.Name, mimeMessage.Sender.Address);

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
            getEmails(newestEmail);
            deleteEmails(emailsToDelete);

            if (connected) this.emailClient.Disconnect(true);

            return this.emailsReceived;
        }



        #region Region - usuwanie emaili z serwera


        private void deleteEmails(IList<IEmailMessage> emailsToDelete)
        {
            if (emailsToDelete!=null && emailsToDelete.Count > 0)
            {
                try
                {
                    //z listy tworzę słownik żeby ułatwić wyszukiwanie wiadomości
                    //teoretycznie wiadomość może być usunięta na serwerze w inny sposób pomiędzy czasem kiedy została zaznaczona do usunięcia w programie
                    //czyli została dodana do listy wiadomości do usunięcia
                    //a zanim została usunięta w pętli poniżej, więc pętlę muszę zatrzymać gdy dojdę do wiadomości na serwerze, która jest starsza niż
                    //najstarsza wiadomość przekazana do usunięcia

                    IEmailMessage oldestEmail = getOldestEmail(emailsToDelete);

                    Dictionary<string, IEmailMessage> emailsToDeleteDict = constructEmailToDeleteDict(emailsToDelete);

                    int numberOfEmailsOnServer = emailClient.GetMessageCount();
                    if (numberOfEmailsOnServer == 0)
                    {
                        throw new EmailServiceException("brak wiadomości na serwerze " + emailAccountConfiguration.receiveServer.url + " emailClient.IsConnected " + emailClient.IsConnected);
                    }

                    int emailIndex = numberOfEmailsOnServer - 1;                //index ostatniego, tj najnowszego, maila na serwerze
                    MimeMessage mimeMessage = null; ;

                    do
                    {
                        mimeMessage = emailClient.GetMessage(emailIndex);

                        if (emailsToDeleteDict.ContainsKey(tryGetId(mimeMessage)))
                        {
                            emailClient.DeleteMessage(emailIndex);
                            emailsToDeleteDict.Remove(mimeMessage.MessageId);
                        }
                        emailIndex--;

                    }
                    //teoretycznie wiadomość może być usunięta na serwerze w inny sposób pomiędzy czasem kiedy została zaznaczona do usunięcia w programie 
                    //a zanim została usunięta w tej pętli, więc pętlę muszę zatrzymać gdy dojdę do wiadomości na serwerze, 
                    //która jest starsza od najstarszej przekazanej do skasowania
                    while (emailsToDeleteDict.Count > 0 && mimeMessage.Date >= oldestEmail.DateTime && emailIndex > 0);
                }
                catch (MailKit.Net.Pop3.Pop3ProtocolException e)
                {
                    throw new EmailServiceException("Email service error", e);
                }
                catch(Pop3CommandException e)
                {
                    throw new EmailServiceException("Email service error", e);
                }
                catch (MailKit.ServiceNotConnectedException e)
                {
                    throw new EmailServiceException("Email service error", e);
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
            catch (MailKit.Net.Pop3.Pop3ProtocolException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                throw new EmailServiceException("Email service error", e);
            }
        }

            #endregion




            #region Region - wysyłanie wiadomości

            public override void SendEmails(IList<IEmailMessage> emailMessages)
        {

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(emailAccountConfiguration.sendServer.url, emailAccountConfiguration.sendServer.port, true);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(emailAccountConfiguration.username, emailAccountConfiguration.password);

                foreach (EmailMessage emailMessage in emailMessages)
                {
                    var message = new MimeMessage();
                    message.Subject = emailMessage.Subject;
                    //We will say we are sending HTML. But there are options for plaintext etc. 
                    message.Body = new TextPart(TextFormat.Html)
                    {
                        Text = emailMessage.Content
                    };

                    emailClient.Send(message);
                }

                emailClient.Disconnect(true);
            }

        }


        #endregion



    }
}

