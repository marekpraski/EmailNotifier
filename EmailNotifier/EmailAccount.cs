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
        public LinkedList<IEmailMessage> emailsList { get; set; }
        public LinkedList<IEmailMessage> newEmailsList { get; set; }
        public IEmailAccountConfiguration configuration { get; set; }
        public bool hasNewEmails { get; set; }

        public EmailAccount()
        {
            emailsList = new LinkedList<IEmailMessage>();
            newEmailsList = new LinkedList<IEmailMessage>();
        }

        public void addEmail(IEmailMessage email)
        {
                emailsList.AddFirst(email);
                newEmailsList.AddFirst(email);
                hasNewEmails = true;
        }

        public void addEmail(LinkedList<IEmailMessage> emails)
        {
            while(emails.Count>0)
            {
                addEmail(emails.Last.Value);
                emails.RemoveLast();
            }
            
        }


        public void markEmailRead(IEmailMessage email)
        {
            if(hasNewEmails && newEmailsList.Contains(email))
            {
                newEmailsList.Remove(email);
                if (newEmailsList.Count == 0)
                {
                    hasNewEmails = false;
                }
            }
        }
    }
}
