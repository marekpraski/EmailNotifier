using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MimeKit;

namespace EmailNotifier
{
    public class EmailService : IEmailService
    {
        protected readonly LinkedList<IEmailMessage> emailsReceived = new LinkedList<IEmailMessage>();


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

        protected virtual IEmailMessage getOneEmail(int messageIndex)
        {
            throw new NotImplementedException();
        }

        protected IEmailMessage getOldestEmail(IList<IEmailMessage> emailsToDelete)
        {
            DateTime oldestDate = emailsToDelete[0].DateTime;
            int messageIndex = 0;

            for( int i = 1; i< emailsToDelete.Count; i++)
            {
                if(oldestDate > emailsToDelete[i].DateTime)      //im wiadomość młodsza, tym data większa
                {
                    oldestDate = emailsToDelete[i].DateTime;
                    messageIndex = i;
                }
            }
            return emailsToDelete[messageIndex];
        }


        protected IEmailMessage createOneEmailMessage(int messageIndex, MimeMessage message)
        {
            return new EmailMessage()
            {
                Subject = message.Subject,
                Id = message.MessageId,
                FromAddress = message.From.ToString(),
                DateTime = message.Date.LocalDateTime,
                Content = message.TextBody,
                nrOnServer = messageIndex
            };
        }

        protected void tryAddToNewEmailsList(IEmailMessage benchmarkEmail, IEmailMessage emailToAdd)
        {
            //sprawdzenie po ID wiadomości działa tylko wtedy, gdy w międzyczasie nie usunąłem z serwera wiadomości nowszych, niż ostatnio wczytana
            //dlatego w warunku pętli czytam maile tylko do określonej daty

            if (emailToAdd.Id != benchmarkEmail.Id)
            {
                this.emailsReceived.AddLast(emailToAdd);
            }
        }

        //wiadomości czytam od najnowszej, aż dojdę do tej, którą już mam w bazie. Ale ...
        //wiadomość może być usunięta na serwerze zanim została zaczytana w programie, więc wiadomości nie będzie wtedy w bazie programu
        //wczytuję więc tylko wiadomości młodsze od ostatniej, którą mam w bazie
        //datę zaokrąglam do dnia, bo są maile które mają wyższy numer, ale wcześniejszą godzinę
        //taki email powoduje zaburzenie i część maili nie zostaje wtedy wczytana gdy warunek jest po dokładnym czasie
        //https://github.com/jstedfast/MailKit/issues/974
        protected bool conditionContinueGettingEmails(IEmailMessage benchmarkEmail, IEmailMessage emailMessage)
        {
            return emailMessage.DateTime.Date > benchmarkEmail.DateTime.Date.AddDays(-1.00);
        }


        protected Dictionary<string, IEmailMessage> constructEmailToDeleteDict(IList<IEmailMessage> emailsToDelete)
        {
            Dictionary<string, IEmailMessage> emailsToDeleteDict = new Dictionary<string, IEmailMessage>();

            string emailId;

            for (int i = 0; i < emailsToDelete.Count; i++)
            {
                emailId = emailsToDelete[i].Id;
                emailsToDeleteDict.Add(emailId, emailsToDelete[i]);
            }

            return emailsToDeleteDict;
        }
    }
}
