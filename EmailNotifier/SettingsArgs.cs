using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public class SettingsArgs : EventArgs
    {
        public int emailCheckTimespan { get; set; }
        public int notificationBubbleTimespan { get; set; }
        public int emailNumberKept { get; set; }
    }
}
