using MailKit.Net.Imap;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public class EmailServiceImap : IEmailService
    {
        private ImapClient emailClient;

        public EmailServiceImap(IEmailAccountConfiguration emailAccountConfiguration)
        {
            emailClient = new ImapClient(); MimeKit.MimeMessage message = emailClient.Inbox.GetMessage(0);
            //message.
        }

        public void DeleteEmails(IList<IEmailMessage> emailsToDelete)
        {
            throw new NotImplementedException();
        }


        public LinkedList<IEmailMessage> ReceiveAndDelete(IEmailMessage lastEmail, IList<IEmailMessage> emailsToDelete = null)
        {
            throw new NotImplementedException();
        }

        public LinkedList<IEmailMessage> ReceiveEmails(int numberOfMessagesToReceive)
        {
            UniqueId id;
            var message = emailClient.Inbox.GetMessage(1);
            //message.
            throw new NotImplementedException();
        }

        public LinkedList<IEmailMessage> ReceiveEmails(IEmailMessage lastEmail)
        {
            throw new NotImplementedException();
        }

        public void SendEmails(IList<IEmailMessage> emailMessages)
        {
            throw new NotImplementedException();
        }
    }
}
