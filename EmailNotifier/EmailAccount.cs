using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace EmailNotifier
{
    [Serializable]
    public class EmailAccount
    {
        private ConcurrentDictionary<string, IEmailMessage> allEmailsDict;
        public string name { get; set; }
        public LinkedList<IEmailMessage> allEmailsList { get; set; }

        public LinkedList<IEmailMessage> newEmailsList { get; set; }
        public IEmailAccountConfiguration configuration { get; set; }
        public bool hasNewEmails 
        {
            get { return newEmailsList.Count > 0; }
        }

        public EmailAccount()
        {
            allEmailsList = new LinkedList<IEmailMessage>();
            newEmailsList = new LinkedList<IEmailMessage>();
            allEmailsDict = new ConcurrentDictionary<string, IEmailMessage>();
        }

        public void addEmails(IEmailMessage email)
        {
            if (allEmailsDict.TryAdd(email.Id, email))
            {
                allEmailsList.AddFirst(email);
                newEmailsList.AddFirst(email);
            }
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
            allEmailsDict[email.Id].markedForDeletion = true;
        }


        public void markEmailDoNotDelete(IEmailMessage email)
        {
            allEmailsDict[email.Id].markedForDeletion = false;
        }

        public void markEmailsDeletedFromServer(List<IEmailMessage> deletedEmails)
        {
            foreach (IEmailMessage email in deletedEmails)
            {
                allEmailsDict[email.Id].deletedFromServer = true;
                allEmailsDict[email.Id].Content = "";
            }
        }


        public void updateNewEmailsList(List<IEmailMessage> emails)
        {
            //TODO: jeżeli użytkownik zaznaczy email do skasowania w oknie wszystkich emaili, a email ten jest również nowym mailem to email ten nie jest usuwany z listy nowych maili; można by to obsłużyć podczas operacji zaznaczania maili do skasowania
            //ale chyba najprościej czyścić takie przypadki w tym miejscu; zakładam że lista nowych maili będzie zawsze stosunkowo krótka, więc nie zajmie to dużo czasu

            for (int i = 0; i < newEmailsList.Count; i++)
            {
                if (newEmailsList.ElementAt(i).deletedFromServer)
                    newEmailsList.Remove(newEmailsList.ElementAt(i));
            }


            foreach (IEmailMessage email in emails)
            {
                if(newEmailsList.Contains(email))
                    newEmailsList.Remove(email);
            }
        }


        /// <summary>
        /// docina listę wszystkich emaili do liczby widocznych emaili zdefiniowanej przez użytkownika, oznaczając odpowiednią liczbę najstarszych maili jako niewidoczne
        /// zadziała tylko jeżeli liczba nowych emaili jest zero;
        /// emailTrimIndex - pozycja na liście allEmailsList powyżej której usuwam emaile z tej listy
        /// </summary>
        /// <param name="emailTrimIndex"></param>
        public void trimEmailDisplayList(int emailTrimIndex)
        {
            if (newEmailsList.Count == 0)
            {
                int i = emailTrimIndex +1;
                while (i < allEmailsList.Count && allEmailsList.ElementAt(i).visible)
                {
                    allEmailsList.ElementAt(i).visible = false;
                    i++;
                }
            }
        }

        public override string ToString()
        {
            string emailSummaryTxt = this.name + "\r\n====================================================================\r\n";
            foreach(EmailMessage email in allEmailsList)
            {
                if(!email.deletedFromServer)
                    emailSummaryTxt += email.DateTime + "  id  " + email.Id + "                                  " + email.FromAddress + "  " + email.Subject + "\r\n";
            }
            return emailSummaryTxt + "\r\n\r\n";
        }

        internal void clearData()
        {
            allEmailsList.Clear();
            newEmailsList.Clear();
            allEmailsDict.Clear();
        }

        public void deleteLeadingEmails(int numberOfEmails)
        {
            for (int i = 0; i < numberOfEmails; i++)
            {
                allEmailsList.RemoveFirst();
            }
        }
    }
}
