using System;


namespace EmailNotifier
{
    [Serializable]
    public class EmailAccountConfiguration : IEmailAccountConfiguration
    {
        public ServerTypes receiveServerType { get; set; }
        public string SendServer { get; set; }
        public int SendPort { get; set; }
        public string SendUsername { get; set; }
        public string SendPassword { get; set; }

        public string ReceiveServer { get; set; }
        public int ReceivePort { get; set; }
        public string ReceiveUsername { get; set; }
        public string ReceivePassword { get; set; }
        public bool ReceiveUseAuthorisation { get; set; }
    }
}
