using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace EmailNotifier
{
    class InternetConnection
    {
        public static bool available
        {
            get { return NetworkInterface.GetIsNetworkAvailable(); }
        }

    }
}
