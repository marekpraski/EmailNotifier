using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    [Serializable]
    public class EmailAccount
    {
        public string name { get; set; }
        public Dictionary<string, EmailMessage> emailsDict {get; set;}
        private Dictionary<string, EmailMessage> newEmailsDict;
        public EmailConfiguration configuration { get; set; }
        private bool newEmails = false;

        public EmailAccount()
        {
            emailsDict = new Dictionary<string, EmailMessage>();
            newEmailsDict = new Dictionary<string, EmailMessage>();
        }

        public void addEmail(EmailMessage email)
        {
            if (!emailsDict.ContainsKey(email.messageId))
            {
                emailsDict.Add(email.messageId, email);
                newEmailsDict.Add(email.messageId, email);
                newEmails = true;
            }
        }

        public bool hasNewEmails()
        {
            return newEmails;
        }

        public void markEmailRead(EmailMessage email)
        {
            if(newEmails)
            {
                newEmailsDict.Remove(email.messageId);
                if (newEmailsDict.Count > 0)
                {
                    newEmails = false;
                }
            }
        }
    }
}
