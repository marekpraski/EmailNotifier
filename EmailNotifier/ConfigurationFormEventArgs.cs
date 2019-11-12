using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public class ConfigurationFormEventArgs : EventArgs
    {
        public EmailAccount emailAccount { get; set; }
    }
}
