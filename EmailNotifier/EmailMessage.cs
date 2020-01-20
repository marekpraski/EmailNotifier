using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    [Serializable]
    public class EmailMessage : IEmailMessage
    {
        public EmailMessage()
        {
        }

        public EmailAddress ToAddress { get; set; }
        public EmailAddress SenderAddress { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string messageId { get; set; }
        public DateTime messageDateTime { get; set; }
        public bool markedForDeletion { get; set; }
        public bool deletedFromServer { get ; set; }
        public int nrOnServer { get; set; }
    }
}
