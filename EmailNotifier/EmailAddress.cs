using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    [Serializable]
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public EmailAddress(string name, string address)
        {
            this.Address = address;
            this.Name = name;
        }

        public EmailAddress()
        {

        }

    }
}
