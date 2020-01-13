using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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


        protected void printLog(string serverUrl, string log)
        {
            string dir = serverUrl.Replace(".", "_");
            Directory.CreateDirectory(dir);

            string timeStamp = DateTime.Now.ToString().Replace("-","_").Replace(" ","_").Replace(":","_");
            string fileName = dir + "" + @"\detailedLog_" + timeStamp + ".txt";
            using (FileStream stream = new FileStream(fileName, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(log);
                writer.Close();
            }
        }

        protected void appendLog(DateTime newestEmailDateTime, string newestEmailId, EmailMessage emailMessage, bool compareId, bool compareDateTime,bool addedToDB, ref string log)
        {
            DateTime messDate = emailMessage.messageDateTime;
            string messId = emailMessage.messageId;
            log += "newestEmailDateTime  " + newestEmailDateTime.ToString() + "   newestEmailId  " + newestEmailId + "\r\n" +
                "emailMessage.messageDateTime  " + emailMessage.messageDateTime.ToString() + "  emailMessage.messageId  " + emailMessage.messageId + "\r\n" +
                "  compareId  " + compareId + "   compareDateTime   " + compareDateTime + "   addedToDB   "+addedToDB+ "\r\n" +
                emailMessage.Subject + "\r\n\r\n";

        }
    }
}
