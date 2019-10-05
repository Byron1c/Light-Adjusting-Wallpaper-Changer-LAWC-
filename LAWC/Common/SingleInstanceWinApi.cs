// FROM: http://www.codeproject.com/Articles/32908/C-Single-Instance-App-With-the-Ability-To-Restore
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;


namespace LAWC.Common
{
    static public class SingleInstanceWinApi
    {
        public static readonly int WMSHOWFIRSTINSTANCE =
            NativeMethods.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        static Mutex mutex;

        static public bool Start()
        {
            string mutexName = String.Format(CultureInfo.InvariantCulture, "Local\\{0}", ProgramInfo.AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out bool onlyInstance);
            return onlyInstance;
        }

        static public void ShowFirstInstance()
        {
            NativeMethods.PostMessage(
                (IntPtr)NativeMethods.HWND_BROADCAST,
                WMSHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);
        }

        static public void Stop()
        {
            mutex.ReleaseMutex();
        }


        // ADDED (NOT IN ORIGINAL ARTICLE):
        /// <summary>
        /// check if given exe alread running or not
        /// </summary>
        /// <returns>returns true if already running</returns>
        internal static bool IsAlreadyRunning()
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;

            //using (Mutex mutex = new Mutex(true, "Global\\" + sExeName, out bCreatedNew))
            //{
            mutex = new Mutex(true, "Global\\" + sExeName, out bool bCreatedNew);
            if (bCreatedNew)
                    mutex.ReleaseMutex();
            //}
            return !bCreatedNew;
        }


    }
}
