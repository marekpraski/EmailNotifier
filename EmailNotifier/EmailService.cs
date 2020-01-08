using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public class EmailService : IEmailService
    {
        public virtual void DeleteEmails(IList<IEmailMessage> emailsToDelete)
        {
            throw new NotImplementedException();
        }

        public virtual LinkedList<IEmailMessage> ReceiveAndDelete(IEmailMessage lastEmail, IList<IEmailMessage> emailsToDelete = null)
        {
            throw new NotImplementedException();
        }

        public virtual LinkedList<IEmailMessage> ReceiveEmails(int numberOfMessagesToReceive)
        {
            throw new NotImplementedException();
        }

        public virtual LinkedList<IEmailMessage> ReceiveEmails(IEmailMessage lastEmail)
        {
            throw new NotImplementedException();
        }

        public virtual void SendEmails(IList<IEmailMessage> emailMessages)
        {
            throw new NotImplementedException();
        }

        protected IEmailMessage getOldestEmail(IList<IEmailMessage> emailsToDelete)
        {
            DateTime oldestDate = emailsToDelete[0].messageDateTime;
            int messageIndex = 0;

            for( int i = 1; i< emailsToDelete.Count; i++)
            {
                if(oldestDate > emailsToDelete[i].messageDateTime)      //im wiadomość młodsza, tym data większa
                {
                    oldestDate = emailsToDelete[i].messageDateTime;
                    messageIndex = i;
                }
            }
            return emailsToDelete[messageIndex];
        }


        protected Dictionary<string, IEmailMessage> constructEmailToDeleteDict(IList<IEmailMessage> emailsToDelete)
        {
            Dictionary<string, IEmailMessage> emailsToDeleteDict = new Dictionary<string, IEmailMessage>();

            string emailId;

            for (int i = 0; i < emailsToDelete.Count; i++)
            {
                emailId = emailsToDelete[i].messageId;
                emailsToDeleteDict.Add(emailId, emailsToDelete[i]);
            }

            return emailsToDeleteDict;
        }
    }
}
