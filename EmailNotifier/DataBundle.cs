using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    [Serializable]
    class DataBundle
    {
        public int checkEmailTimespan { get; set; }
        public int showNotificationTimespan { get; set; }
        public int numberOfEmailsKept { get; set; }
        public Dictionary<string, EmailAccount> mailBoxes { get; set; }
        public Dictionary<string, List<IEmailMessage>> emailsToBeDeletedDict { get; set;}

        public DataBundle()
        {
            mailBoxes = new Dictionary<string, EmailAccount>();
            emailsToBeDeletedDict = new Dictionary<string, List<IEmailMessage>>();
        }

        public DataBundle(Dictionary<string, EmailAccount> mailBoxes, Dictionary<string, List<IEmailMessage>> emailsToBeDeletedDict) : this()
        {
            this.mailBoxes = mailBoxes;
            this.emailsToBeDeletedDict = emailsToBeDeletedDict;
        }
    }
}
