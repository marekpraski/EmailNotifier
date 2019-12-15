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
        public static string fileSavePath = @"C:\testDesktop\conf\";
#else
         public static string currentPath = Application.StartupPath;
#endif

    }
}
