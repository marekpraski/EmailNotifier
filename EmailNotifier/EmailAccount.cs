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
            try
            {
            newEmailsList.Find(email).Value.markedForDeletion = true;
            }
            catch (Exception)
            {
            }
            allEmailsList.Find(email).Value.markedForDeletion = true;
        }

        public void markEmailDoNotDelete(IEmailMessage email)
        {
            try
            {
            newEmailsList.Find(email).Value.markedForDeletion = false;
            }
            catch (Exception)
            {                
            }
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


        /// <summary>
        /// docina listę wszystkich emaili do liczby widocznych emaili zdefiniowanej przez użytkownika, usuwając odpowiednią liczbę najstarszych maili.
        /// zadziała tylko jeżeli liczba nowych emaili jest zero;
        /// emailTrimIndex - pozycja na liście allEmailsList powyżej której usuwam emaile z tej listy
        /// </summary>
        /// <param name="emailTrimIndex"></param>
        public void trimEmailList(int numberOfEmailsDisplayed, int emailTrimIndex)
        {
            //nie mogę po prostu zastosować parametru z ustawień, bo lista maili zawiera też
            //maile oznaczone jako skasowane z serwera, które nie są wyświetlane
            //a parametr liczby trzymanych maili odnosi się do maili widocznych dla użytkownika

            //jeżeli użytkownik zaznaczy email do skasowania w oknie wszystkich emaili, a email ten jest nowym mailem
            //to email ten nie jest usuwany z listy nowych maili; można by to obsłużyć podczas operacji zaznaczania maili do skasowania
            //ale chyba najprościej czyścić takie przypadki w tym miejscu
            //zakładam że lista nowych maili będzie zawsze stosunkowo krótka, więc nie zajmie to dużo czasu
            if(newEmailsList.Count > 0)
            {
                for(int i=0; i<newEmailsList.Count; i++)
                {
                    if (newEmailsList.ElementAt(i).deletedFromServer)
                        newEmailsList.Remove(newEmailsList.ElementAt(i));
                }
            }

            if(newEmailsList.Count == 0)
            {
                while (allEmailsList.Count > emailTrimIndex)
                {
                    allEmailsList.RemoveLast();
                }
            }
        }


    }
}
