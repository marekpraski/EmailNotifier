using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
   
    public interface IEmailAccountConfiguration
    {
        EmailServer receiveServer { get; set; }
        EmailServer sendServer { get; set; }
        string username { get; set; }
        string password { get; set; }
    }
}
