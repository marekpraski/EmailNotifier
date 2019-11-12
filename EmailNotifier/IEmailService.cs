using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
        Dictionary<string, EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
