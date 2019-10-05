using LAWC.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LAWC
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set the unhandled exception mode to force all Windows Forms errors
            // to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // set higher priority
            Process.GetCurrentProcess().PriorityBoostEnabled = true;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
            // Of course this only affects the main thread rather than child threads.
            Thread.CurrentThread.Priority = MainFunctions.threadPriority;

            // do this before creating Main Form as the loading screen will be displayed unnecessarily
            if (SingleApplication.IsAlreadyRunning())
            {
                LAWC.NativeMethods.SendNotifyMessage(
                    (IntPtr)LAWC.NativeMethods.HWND_BROADCAST,
                    SingleApplication.WMSHOWFIRSTINSTANCE,
                    IntPtr.Zero,
                    IntPtr.Zero);

                //set focus on previously running app
                SingleApplication.SwitchToCurrentInstance();

                return;
            }


            try
            {
                Application.Run(new FrmMain(args));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message + "\n\n\n\n" + ex.Message);
                MessageBox.Show("Well, this is embarassing. Something went wrong... \n\n"
                    + "Try starting up LAWC again. Also check the Error Log (in Advanced Settings)\n\n"
                    + "Error: " + ex.Message.ToString(CultureInfo.InvariantCulture) + "\n\n" + ex.StackTrace.ToString(CultureInfo.InvariantCulture) + "\n\n" + ex.Source.ToString(CultureInfo.InvariantCulture),
                    "LAWC Crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;


            }            
        }

    }
}
