using System;
using System.Collections.Generic;
using System.Linq;

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

        public void addEmails(IEmailMessage email)
        {
                allEmailsList.AddFirst(email);
                newEmailsList.AddFirst(email);
                hasNewEmails = true;
        }

        public void addEmails(LinkedList<IEmailMessage> emails)
        {
            while(emails.Count>0)
            {
                addEmails(emails.Last.Value);
                emails.RemoveLast();
            }            
        }


        public void markEmailDelete(IEmailMessage email)
        {
            newEmailsList.Find(email).Value.markedForDeletion = true;
            allEmailsList.Find(email).Value.markedForDeletion = true;
        }

        public void markEmailDoNotDelete(IEmailMessage email)
        {
            newEmailsList.Find(email).Value.markedForDeletion = false;
            allEmailsList.Find(email).Value.markedForDeletion = false;

        }

        public void markEmailsDeletedFromServer(List<IEmailMessage> emails)
        {
            foreach (IEmailMessage email in emails)
            {
                LinkedListNode<IEmailMessage> node = allEmailsList.Find(email);
                node.Value.deletedFromServer = true;
                node.Value.Content = "";
            }
        }


    }
}
