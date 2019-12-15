using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    interface IConfigurationReader
    {
        EmailAccountConfiguration getConfiguration();
    }
}
