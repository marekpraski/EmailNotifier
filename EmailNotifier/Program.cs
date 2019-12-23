using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Net;
using System.Runtime.InteropServices;

namespace EmailNotifier
{
    static class Program
    {
        //Retrieves the connected state of the local system
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);


        public static bool checkInternetOn() //First and fastest way to determine Internet Connection
        {
            try
            {
                int ConnDesc; //Return value
                return InternetGetConnectedState(out ConnDesc, 0); //Return result
            }
            catch
            {
                return false; //Not connected
            }

        }


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
