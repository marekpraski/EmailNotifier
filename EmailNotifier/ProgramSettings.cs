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

    }
}
