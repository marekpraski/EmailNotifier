
using System;
using System.Net.NetworkInformation;


namespace EmailNotifier
{
    [Serializable]
    public class EmailServer
    {
        private string _url;

        /// <summary>
        /// przyjmując url sprawdza jego poprawność
        /// </summary>
        public string url
        {
            get { return _url; }

            set { _url = setUrl(value); }
        }
        public int port { get; set; }
  
        public bool useAuthorisation { get; set; }
        public bool verified { get; private set; }
        public ServerType serverType { get; set; }



        private string setUrl(string value)
        {
            verifyUrl(value);
            return value;
        }

        private void verifyUrl(string value)
        {
            if (value == "")
            {
                throw new ArgumentException("Pole nazwy serwera nie może być puste");
            }
            if (NetworkInterface.GetIsNetworkAvailable())       //czy serwer istnieje sprawdzam tylko wtedy gdy jest internet
            {
                try
                {
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(value);
                    verified = reply.Status == IPStatus.Success;
                }
                catch (PingException ex)
                {
                    throw new ArgumentException("nieznany host  " + value, ex);
                }

                if (!verified)
                {
                    throw new ArgumentException("host not found  " + value);
                }
            }
        }

        /// <summary>
        /// zwraca url serwera, jeżeli został on już wcześniej zweryfikowany, w przeciwnym wypadku usiłuje zweryfikować serwer 
        /// i go zwraca gdy weryfikacja przebiegnie bez błędu
        /// </summary>
        /// <returns></returns>
        public string TryGetUrl()
        {
            if (!this.verified)
            {
                verifyUrl(_url);
            }
            return _url;
        }

    }
}
