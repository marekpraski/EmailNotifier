using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public class EmailService
    {
        private readonly IEmailConfiguration emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration;
        }

        public void checkEmail()
        {
            
        }

        public void ReceiveEmail(EmailAccount mailbox, int maxCount = 6)
        {

            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(emailConfiguration.PopServer, emailConfiguration.PopPort, emailConfiguration.PopUseTSL);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(emailConfiguration.PopUsername, emailConfiguration.PopPassword);
                int numberOfMessages = emailClient.Count;

                for (int i = numberOfMessages - 1; i > 0 && i > (emailClient.Count - maxCount); i--)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject,
                        messageId = message.MessageId
                    };

                    MailboxAddress ma = message.Sender; 
                    emailMessage.FromAddress = new EmailAddress(ma.Name, ma.Address);
                    mailbox.addEmail(emailMessage);
                }
            }
        }
    

        public void Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();

            //message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort, true);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }

        }
    }
}

