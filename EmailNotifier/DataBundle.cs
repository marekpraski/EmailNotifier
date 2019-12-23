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

        public DataBundle()
        {
            mailBoxes = new Dictionary<string, EmailAccount>();
        }

        public DataBundle(Dictionary<string, EmailAccount> mailBoxes) : this()
        {
            this.mailBoxes = mailBoxes;
        }
    }
}
