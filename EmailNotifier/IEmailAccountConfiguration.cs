using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
   
    public interface IEmailAccountConfiguration
    {
        string SendServer { get; set; }
        int SendPort { get; set; }
        string SendUsername { get; set; }
        string SendPassword { get; set; }


        ServerTypes receiveServerType { get; set; }
        string ReceiveServer { get; set; }
        int ReceivePort { get; set; }
        string ReceiveUsername { get; set; }
        string ReceivePassword { get; set; }
        bool ReceiveUseAuthorisation { get; set; }
    }
}
