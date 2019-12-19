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
        public LinkedList<IEmailMessage> allEmailsList { get; set; }
        public LinkedList<IEmailMessage> newEmailsList { get; set; }
        public IEmailAccountConfiguration configuration { get; set; }
        public bool hasNewEmails
        {
            get { return newEmailsList.Count > 0; }
            set { }
        }

        public EmailAccount()
        {
            allEmailsList = new LinkedList<IEmailMessage>();
            newEmailsList = new LinkedList<IEmailMessage>();
        }

        public void addEmail(IEmailMessage email)
        {
                allEmailsList.AddFirst(email);
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



        public bool removeNewEmail(string emailId)
        {
            bool emailFound = false;
            int i = 0;
            do
            {
                IEmailMessage email = newEmailsList.ElementAt(i);
                if(email.messageId == emailId)
                {
                    newEmailsList.Remove(email);
                    emailFound = true;
                }
                i++;
            }
            while (!emailFound);
            return emailFound;
        }


    }
}
