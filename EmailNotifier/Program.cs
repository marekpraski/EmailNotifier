﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EmailNotifier
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new MainForm());

            }
            catch (System.Reflection.TargetInvocationException ex)
            {

                MyMessageBox.display(ex.Message +"\r\n"+ ex.InnerException +"\r\n"+ ex.StackTrace);
            }
        }

    }
}
