using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    class EmailServiceException : Exception
    {

        public EmailServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public EmailServiceException(string message) : base(message)
        {

        }
    }
}
