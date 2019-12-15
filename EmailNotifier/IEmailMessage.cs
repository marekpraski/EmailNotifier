using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public interface IEmailMessage
    {

        string FromAddress { get; set; }
        string Subject { get; set; }
        string messageId { get; set; }
        DateTime messageDateTime { get; set; }
    }
}
