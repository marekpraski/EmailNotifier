
using System;
using System.Net.NetworkInformation;


namespace EmailNotifier
{
    [Serializable]
    public class EmailServer
    {
        private string serverUrl;
        public string url
        {
            
            get { return serverUrl; }
            
            set { if(verifyServer(value)) serverUrl = value; }
        }
        public int port { get; set; }
        public bool useAuthorisation { get; set; }
        public bool verified { get; set; }
        public ServerType serverType { get; set; }


        private bool verifyServer(string url)
        {
            if (url == "")
            {
                throw new ArgumentException("Pole nazwy serwera nie może być puste");
            }
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(url);
            verified = setVerified(reply.Status == IPStatus.Success);
            if (!verified)
            {
                throw new ArgumentException("host not found  " + url);
            }
            return verified;
        }

        private bool setVerified(bool status)
        {
            return status;
        }

    }
}
