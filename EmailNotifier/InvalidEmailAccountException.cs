using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    class InvalidEmailAccountException : Exception
    {
        public string message { get; set; }

        public InvalidEmailAccountException(string message)
        {
            this.message = message;
        }
    }
}
