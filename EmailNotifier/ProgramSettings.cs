using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotifier
{

    public static class ProgramSettings
    {
        public static string emailDataFileName = "emailNotifierData.bin";

#if DEBUG
        public static string fileSavePath = @"C:\testDesktop\emNotTest\";
#else
         public static string fileSavePath = @"data\";     //tzn katalog data pod katalogiem, z którego skąd aplikacja jest uruchamiana
#endif

        public static int checkEmailTimespan { get; set; } = 15;            //minut 
        public static int showNotificationTimespan { get; set; } = 30;      //sekund
        public static int numberOfEmailsKept { get; set; } = 50;               //przeczytanych maili

    }
}
