using System;
using System.Net.NetworkInformation;

namespace EmailNotifier
{
    [Serializable]
    public class EmailAccountConfiguration : IEmailAccountConfiguration
    {
        public EmailServer receiveServer { get; set; }
        public EmailServer sendServer { get; set; }
        public string username { get; set; }
        public string password { get; set; }


    }
}
