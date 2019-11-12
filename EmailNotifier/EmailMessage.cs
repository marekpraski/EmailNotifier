using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    [Serializable]
    public class EmailMessage
    {
        public EmailMessage()
        {
        }

        public EmailAddress ToAddress { get; set; }
        public EmailAddress FromAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string messageId { get; set; }
    }
}
