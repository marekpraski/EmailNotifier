using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.InteropServices;

namespace EmailNotifier
{
    /// <summary>
    /// klasa odpowiedzialna za łączenie się z serwerem poczty email i ściąganie/wysyłanie emaili
    /// klasa używa bibliotekę MailKit
    /// </summary>
    public class EmailService : IEmailService
    {
        private IEmailAccountConfiguration emailAccountConfiguration;
        private readonly LinkedList<IEmailMessage> emailsReceived = new LinkedList<IEmailMessage>();

        public EmailService(IEmailAccountConfiguration emailAccountConfiguration)
        {
            this.emailAccountConfiguration = emailAccountConfiguration;
        }



        /// <summary>
        /// czyta określoną liczbę najnowszych emaili z serwera
        /// </summary>
        /// <param name="numberOfMessagesToReceive"> liczba wiadomości do przeczytania z serwera</param>
        /// <returns></returns>

        public LinkedList<IEmailMessage> ReceiveEmails(int numberOfMessagesToReceive)
        {            
            switch (emailAccountConfiguration.receiveServer.serverType)
            {
                case ServerType.IMAP:
                    GetMessagesImap(numberOfMessagesToReceive);
                    break;
                case ServerType.POP3:
                    GetMessagesPop3(numberOfMessagesToReceive);
                    break;
            }

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
            switch (emailAccountConfiguration.receiveServer.serverType)
            {
                case ServerType.IMAP:
                    GetMessagesImap(newestEmailId, newestEmailDateTime);
                    break;
                case ServerType.POP3:
                    GetMessagesPop3(newestEmailId, newestEmailDateTime);
                    break;
            }

            return this.emailsReceived;
        }



        private void GetMessagesPop3(int numberOfMessagesToReceive)
        {
            try
            {
                using (var emailClient = new Pop3Client())
                {
                    if (pop3Connect(emailClient)) {
                        int numberOfMessagesOnServer = emailClient.GetMessageCount();

                        for (int i = numberOfMessagesOnServer - 1; i > 0 && i > (numberOfMessagesOnServer - 1 - numberOfMessagesToReceive); i--)
                        {
                            var message = emailClient.GetMessage(i);
                            var emailMessage = new EmailMessage
                            {
                                Subject = message.Subject,
                                messageId = message.MessageId,
                                FromAddress = message.From.ToString(),
                                messageDateTime = message.Date.DateTime
                            };

                            emailsReceived.AddLast(emailMessage);
                        }
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


        /// <summary>
        /// zwraca liczbę emaili na serwerze
        /// </summary>
        /// <param name="emailClient"></param>
        /// <returns></returns>
        private bool pop3Connect(Pop3Client emailClient)
        {
            bool connected = false;
            try
            {
                string srvUrl = emailAccountConfiguration.receiveServer.TryGetUrl();
                emailClient.Connect(srvUrl, emailAccountConfiguration.receiveServer.port, emailAccountConfiguration.receiveServer.useAuthorisation);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(emailAccountConfiguration.username, emailAccountConfiguration.password);
                connected = true;
            }
            catch(MailKit.Security.SslHandshakeException exc)
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

            return connected;
        }


 
        private void GetMessagesPop3(string newestEmailId, DateTime newestEmailDateTime)
        {
            try
            {
                using (var emailClient = new Pop3Client())
                {
                    if (pop3Connect(emailClient))
                    {
                        int numberOfMessagesOnServer = emailClient.GetMessageCount();

                        int messageIndex = numberOfMessagesOnServer - 1;                //index ostatniego, tj najnowszego, maila na serwerze
                        EmailMessage emailMessage;

                        do
                        {
                            var message = emailClient.GetMessage(messageIndex);
                            emailMessage = new EmailMessage();
                            emailMessage.Subject = message.Subject;
                            emailMessage.messageId = message.MessageId;
                            emailMessage.FromAddress = message.From.ToString();
                            emailMessage.messageDateTime = message.Date.UtcDateTime;

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
            }
            catch (MailKit.Net.Pop3.Pop3ProtocolException e)
            {
                MyMessageBox.display(e.Message);
                //throw new Exception("Email service error");               
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                MyMessageBox.display(e.Message);
                //throw new Exception("Email service error");
            }
            
            
        }



        private List<EmailMessage> GetMessagesImap(int numberOfMessagesToReceive)
        {
            throw new NotImplementedException();
        }


        private List<EmailMessage> GetMessagesImap(string newestEmailId, DateTime newestEmailDateTime)
        {
            throw new NotImplementedException();
        }


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

                foreach(EmailMessage emailMessage in emailMessages)
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

    }
}

