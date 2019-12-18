using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;


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
            emailsReceived.Clear();
            
            switch (emailAccountConfiguration.receiveServerType)
            {
                case ServerTypes.IMAP:
                    GetMessagesImap(numberOfMessagesToReceive);
                    break;
                case ServerTypes.POP3:
                    GetMessagesPop3(numberOfMessagesToReceive);
                    break;
            }

            return emailsReceived;
            
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
            switch (emailAccountConfiguration.receiveServerType)
            {
                case ServerTypes.IMAP:
                    GetMessagesImap(newestEmailId, newestEmailDateTime);
                    break;
                case ServerTypes.POP3:
                    GetMessagesPop3(newestEmailId, newestEmailDateTime);
                    break;
            }

            return emailsReceived;

        }



        private void GetMessagesPop3(int numberOfMessagesToReceive)
        {

            using (var emailClient = new Pop3Client())
            {               
                int numberOfMessagesOnServer = initialPOP3Setup(emailClient); 

                for (int i = numberOfMessagesOnServer - 1; i > 0 && i > (numberOfMessagesOnServer -1 - numberOfMessagesToReceive); i--)
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


        /// <summary>
        /// zwraca liczbę emaili na serwerze
        /// </summary>
        /// <param name="emailClient"></param>
        /// <returns></returns>
        private int initialPOP3Setup(Pop3Client emailClient)
        {
            try
            {
            emailClient.Connect(emailAccountConfiguration.ReceiveServer, emailAccountConfiguration.ReceivePort, emailAccountConfiguration.ReceiveUseAuthorisation);
            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
            emailClient.Authenticate(emailAccountConfiguration.ReceiveUsername, emailAccountConfiguration.ReceivePassword);

            return emailClient.Count;
            }
            catch (System.Net.Sockets.SocketException exc)
            {
                MyMessageBox.display(exc.Message + "  " + emailAccountConfiguration.ReceiveServer);
            }
            catch(MailKit.Security.AuthenticationException ex)
            {
                MyMessageBox.display(ex.Message + "  " + emailAccountConfiguration.ReceiveServer);
            }

            return 0;
        }


 
        private void GetMessagesPop3(string newestEmailId, DateTime newestEmailDateTime)
        {
            try
            {
                using (var emailClient = new Pop3Client())
                {
                    int numberOfMessagesOnServer = initialPOP3Setup(emailClient);
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
                        if (emailMessage.messageId != newestEmailId)
                        {
                            this.emailsReceived.AddLast(emailMessage);
                        }
                        messageIndex--;
                    }
                    //wiadomości czytam od najnowszej, aż dojdę do tej, którą już mam w bazie. Ale ...
                    //wiadomość może być usunięta na serwerze zanim została zaczytana w programie, więc wiadomości nie będzie wtedy w bazie programu
                    //dlatego sprawdzam też po dacie, wczytuję tylko wiadomości młodsze od ostatniej, którą mam w bazie
                    while (!emailMessage.messageId.Equals(newestEmailId) || emailMessage.messageDateTime > newestEmailDateTime);
                }
            }
            catch (MailKit.Net.Pop3.Pop3ProtocolException exc)
            {

                MyMessageBox.display(exc.Message + "\r\n" + exc.Source, MessageBoxType.Error);
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
                emailClient.Connect(emailAccountConfiguration.SendServer, emailAccountConfiguration.SendPort, true);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(emailAccountConfiguration.SendUsername, emailAccountConfiguration.SendPassword);

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

