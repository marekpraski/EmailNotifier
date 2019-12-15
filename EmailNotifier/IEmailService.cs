using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public interface IEmailService
    {


        void SendEmails(IList<IEmailMessage> emailMessages);
        LinkedList<IEmailMessage> ReceiveEmails(int numberOfMessages);
        LinkedList<IEmailMessage> ReceiveEmails(IEmailMessage lastEmail);


    }
}
