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
        void DeleteEmails(IList<IEmailMessage> emailsToDelete);
        LinkedList<IEmailMessage> ReceiveEmails(int numberOfMessagesToReceive);
        LinkedList<IEmailMessage> ReceiveEmails(IEmailMessage lastEmail);
        LinkedList<IEmailMessage> ReceiveAndDelete(IEmailMessage lastEmail, IList<IEmailMessage> emailsToDelete = null);


    }
}
