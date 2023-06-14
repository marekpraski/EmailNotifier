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
        public static string emailTextSummaryFileName = "emailSummary.txt";

#if DEBUG
        public static string fileSavePath = @"C:\testApps\emNotTest\";
#else
         public static string fileSavePath = @"data\";     //tzn katalog data pod katalogiem, z którego aplikacja jest uruchamiana
#endif

        public static int checkEmailTimespan { get; set; } = 15;            //minut 
        public static int showNotificationTimespan { get; set; } = 30;      //sekund
        public static int numberOfEmailsKept { get; set; } = 50;               //przeczytanych maili
        public static int numberOfEmailsAtSetup { get; set; } = 10;          //emaili wczytywanych na starcie po utworzeniu konta
        public static bool deleteCheckedEmails { get; set; } = false;
        public static bool enableLogFile { get; set; } = false;
        private static int deleteCheckedEmailsAsInt => deleteCheckedEmails ? 1 : 0;
        private static int enableLogFileAsInt => enableLogFile ? 1 : 0;

        public static string settingsToText()
		{
            StringBuilder sb = new StringBuilder();
            sb.Append(checkEmailTimespan);
            sb.Append(";");
            sb.Append(showNotificationTimespan);
            sb.Append(";");
            sb.Append(numberOfEmailsKept);
            sb.Append(";");
            sb.Append(numberOfEmailsAtSetup);
            sb.Append(";");
            sb.Append(deleteCheckedEmailsAsInt);
            sb.Append(";");
            sb.Append(enableLogFileAsInt);
            return sb.ToString();
		}
    }
}
