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

namespace EmailNotifier
{
    /// <summary>
    /// klasa odpowiedzialna za łączenie się z serwerem poczty email i ściąganie/wysyłanie emaili
    /// klasa używa bibliotekę MailKit
    /// </summary>
    public class EmailServicePop : IEmailService
    {
        private IEmailAccountConfiguration emailAccountConfiguration;
        private readonly LinkedList<IEmailMessage> emailsReceived = new LinkedList<IEmailMessage>();

        private Pop3Client emailClient;
        private bool connected = false;

        public EmailServicePop(IEmailAccountConfiguration emailAccountConfiguration)
        {
            this.emailAccountConfiguration = emailAccountConfiguration;
            emailClient = new Pop3Client();
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

        public LinkedList<IEmailMessage> ReceiveEmails(int numberOfMessagesToReceive)
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
        public LinkedList<IEmailMessage> ReceiveEmails(IEmailMessage newestEmail)
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
                if (connectToServer()) {

                    int numberOfMessagesOnServer = emailClient.GetMessageCount();
                    if (numberOfMessagesOnServer == 0)
                    {
                        throw new EmailServiceException("brak wiadomości na serwerze " + "emailClient.IsConnected " + emailClient.IsConnected);
                    }

                    for (int i = numberOfMessagesOnServer - 1; i > 0 && i > (numberOfMessagesOnServer - 1 - numberOfMessagesToReceive); i--)
                    {
                        EmailMessage emailMessage = getOneMessage(i);
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

 
        private void getMessages(string newestEmailId, DateTime newestEmailDateTime)
        {
            try
            {
                if (connectToServer())
                {
                    int numberOfMessagesOnServer = emailClient.GetMessageCount();

                    int messageIndex = numberOfMessagesOnServer - 1;                //index ostatniego, tj najnowszego, maila na serwerze
                    EmailMessage emailMessage;

                    do
                    {
                        emailMessage = getOneMessage(messageIndex);

                        //sprawdzenie po ID wiadomości działa tylko wtedy, gdy w międzyczasie nie usunąłem z serwera wiadomości nowszych, niż ostatnio wczytana
                        //dlatego sprawdzam też po dacie, wczytuję tylko wiadomości młodsze od ostatniej, którą mam w bazie
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
            catch (MailKit.Net.Pop3.Pop3ProtocolException e)
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
            var message = this.emailClient.GetMessage(messageIndex);
            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = message.Subject,
                messageId = message.MessageId,
                FromAddress = message.From.ToString(),
                messageDateTime = message.Date.UtcDateTime,
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
        public LinkedList<IEmailMessage> ReceiveAndDelete(IEmailMessage newestEmail, IList<IEmailMessage> emailsToDelete = null)
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
            if (emailsToDelete!=null && emailsToDelete.Count > 0)
            {
                try
                {
                    //z listy tworzę słownik żeby ułatwić wyszukiwanie wiadomości
                    //teoretycznie wiadomość może być usunięta na serwerze w inny sposób pomiędzy czasem kiedy została zaznaczona do usunięcia w programie
                    //czyli została dodana do listy wiadomości do usunięcia
                    //a zanim została usunięta w pętli poniżej, więc pętlę muszę zatrzymać gdy dojdę do wiadomości na serwerze, która jest starsza niż
                    //najstarsza wiadomość przekazana do usunięcia
                    Dictionary<string, IEmailMessage> emailsToDeleteDict = new Dictionary<string, IEmailMessage>();
                    IEmailMessage oldestMessage = emailsToDelete[0];
                    string emailId = "";

                    for (int i = 1; i < emailsToDelete.Count - 1; i++)
                    {
                        oldestMessage = emailsToDelete[i].messageDateTime >= emailsToDelete[i + 1].messageDateTime ? emailsToDelete[i + 1] : emailsToDelete[i];
                        emailId = emailsToDelete[i].messageId;
                        emailsToDeleteDict.Add(emailId, emailsToDelete[i]);
                    }

                    //muszę do słownika dodać pierwszą, bo pętlę zaczynam od razu od drugiej
                    emailId = emailsToDelete[0].messageId;
                    emailsToDeleteDict.Add(emailId, emailsToDelete[0]);

                    //muszę do słownika dodać ostatnią, bo pętla kończy się na przedostatniej
                    //gdy lista zawiera tylko jeden email to pierwsza jest ostatnią, więc sprawdzam w słowniku, czy już taki wpis jest zanim dodam
                    emailId = emailsToDelete[emailsToDelete.Count-1].messageId;
                    if(!emailsToDeleteDict.ContainsKey(emailId))
                        emailsToDeleteDict.Add(emailId, emailsToDelete[emailsToDelete.Count-1]);

                    int numberOfMessagesOnServer = emailClient.GetMessageCount();

                    int messageIndex = numberOfMessagesOnServer - 1;                //index ostatniego, tj najnowszego, maila na serwerze
                    MimeMessage emailMessage;

                    do
                    {
                        emailMessage = emailClient.GetMessage(messageIndex);

                        if (emailsToDeleteDict.ContainsKey(emailMessage.MessageId))
                        {
                            emailClient.DeleteMessage(messageIndex);
                            emailsToDeleteDict.Remove(emailMessage.MessageId);
                        }
                        messageIndex--;
                    }
                    //teoretycznie wiadomość może być usunięta na serwerze w inny sposób pomiędzy czasem kiedy została zaznaczona do usunięcia w programie 
                    //a zanim została usunięta w tej pętli, więc pętlę muszę zatrzymać gdy dojdę do wiadomości na serwerze, 
                    //która jest starsza od najstarszej przekazanej do skasowania
                    while (emailsToDeleteDict.Count > 0 || emailMessage.Date > oldestMessage.messageDateTime);
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
        }


        public void DeleteEmails(IList<IEmailMessage> emailsToDelete)
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

            public void SendEmails(IList<IEmailMessage> emailMessages)
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

