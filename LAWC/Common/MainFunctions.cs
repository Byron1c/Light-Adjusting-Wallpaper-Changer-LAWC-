using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Threading;
using System.Net.NetworkInformation;
using System.Threading.Tasks;



namespace LAWC.Common
{
    internal static class MainFunctions
    {
        internal static DateTime DateNull = new DateTime(1980, 1, 1, 12, 0, 0);

        internal static int min(int a, int b) { return Math.Min(a, b); }
        internal static int max(int a, int b) { return Math.Max(a, b); }

        internal static ThreadPriority threadPriority = ThreadPriority.AboveNormal; //ThreadPriority.Normal;

        internal static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        internal static string AppPathName = "LAWC";
        internal static string SettingsFilenameOnly = "Settings.xml";
        internal static string ErrorLogFilenameOnly = "Error.Log";
        internal static string WallpaperWebsitesFilenameOnly = "WallpaperWebsites.xml";

        


        public static string GetAppFullPath(Boolean vIsPortable)
        {
            //AppDomain.CurrentDomain.BaseDirectory

            string FullPath = AppDomain.CurrentDomain.BaseDirectory; //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                                                                     //+ "\\" + AppPathName;
            if (vIsPortable)
            {
                FullPath = Application.ExecutablePath.ToString(CultureInfo.InvariantCulture).Replace("LAWC.exe", ""); //dd System.IO.Directory.GetCurrentDirectory() + "\\";
            }

            return FullPath;
        }


        public static String GetAppIconPath(Boolean vIsPortable)
        {
            return MainFunctions.GetAppFullPath(vIsPortable) + @"\Images\LAWC.ico";

        }


        public static Boolean isAdministrator()
        {
            WindowsPrincipal myPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (myPrincipal.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                // Not Admin
                return false;
            }
            else
            {
                // Administrator
                return true;
            }
        }
        

        public static bool CheckForInternetConnection()
        {
            Boolean output = false;
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8");
                    if (reply != null && reply.Status != IPStatus.Success)
                    {
                        // Raise an event
                        // you might want to check for consistent failures 
                        // before signalling a the Internet is down
                        output = false;
                    }
                    else if (reply.Status == IPStatus.Success)
                    {
                        output = true;
                    }
                    else
                    {
                        output = false;
                    }
                }
            }
            catch (NetworkInformationException)
            {
                output = false;
            }
            catch (System.Net.NetworkInformation.PingException)
            {
                output = false;
            }
            return output;
        }

        // Threaded Ping from: https://stackoverflow.com/questions/49069381/why-ping-timeout-is-not-working-correctly?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        //private static PingReply ForcePingTimeoutWithThreads(string hostname, int timeout)
        //{
        //    PingReply reply = null;
        //    var a = new Thread(() => reply = normalPing(hostname, timeout));
        //    a.Start();
        //    a.Join(timeout); //or a.Abort() after a timeout, but you have to take care of a ThreadAbortException in that case... brrr I like to think that the ping might go on and be successful in life with .Join :)
        //    return reply;
        //}

        private static PingReply normalPing(string hostname, int timeout)
        {
            try
            {
                return new Ping().Send(hostname, timeout);
            }
            catch (NetworkInformationException)//never do this kids, this is just a demo of a concept! Always log exceptions!
            {
                return null; //or this, in such a low level method 99 cases out of 100, just let the exception bubble up
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vWebsiteAddress"></param>
        /// <returns>Returns the ping time in ms</returns>
        public static int CheckForWebsite(String vWebsiteAddress, int vTimeout)
        {
            int output = -1;

            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = ping.Send(vWebsiteAddress, vTimeout); //timeout
                    //PingReply r = PingOrTimeout(vWebsiteAddress, 500);
                    if (reply != null && reply.Status != IPStatus.Success)
                    {
                        // Raise an event
                        // you might want to check for consistent failures 
                        // before signalling a the Internet is down
                        output = (int)reply.RoundtripTime;
                    }
                    else if (reply.Status == IPStatus.Success)
                    {
                        output = (int)reply.RoundtripTime;
                    }
                    else
                    {
                        output = -1;
                    }
                }
            }
            catch (NetworkInformationException)
            {
                output = -1;
            }

            return output;
        }


        internal static void RecursiveCount(String vPath, ref int vCount, ArrayList vFileTypes, Boolean vCancel)
        {
            if (string.IsNullOrEmpty(vPath)) return;

            string currentItemName = string.Empty;

            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(vPath);


            try
            {

                foreach (FileSystemInfo fileObject in dirInfo.GetFileSystemInfos())
                {
                    if (vCancel == true) break;

                    //frmMain_OnRecursiveProgressChanged();

                    if (fileObject.Attributes == FileAttributes.Directory)
                    {
                        //FileFunctions.WriteToLog("Reading Folder: " + fileObject.FullName, vWriteToLog);
                        System.Windows.Forms.Application.DoEvents();

                        // RECURSE HERE:
                        //if (fileObject.Name.ToUpper() == "SEASON 03")
                        //{
                        //    int g = 1;
                        //}
                        RecursiveCount(fileObject.FullName.ToString(CultureInfo.InvariantCulture), ref vCount, vFileTypes, vCancel);



                    }
                    else
                    {

                        currentItemName = fileObject.Name;


                        // are file types okay / appropriate
                        if (FileFunctions.IsFileTypeOK(fileObject.Name, vFileTypes))
                        {

                            vCount++;

                            Application.DoEvents();

                        } // end if file type OK

                    } // if directory or file

                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error Searching files (" + currentItemName + "): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //throw;
            }


        }
        

        public static Color StringToColor(this string colorString)
        {
            colorString = ExtractHexDigits(colorString);

            Color color = Color.White;

            if (colorString.Length == 6)
            {

                var r = colorString.Substring(0, 2);
                var g = colorString.Substring(2, 2);
                var b = colorString.Substring(4, 2);

                try
                {
                    byte rc = Byte.Parse(r, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                    byte gc = Byte.Parse(g, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                    byte bc = Byte.Parse(b, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                    color = Color.FromArgb(rc, gc, bc);
                }
                catch (Exception)
                {
                    return Color.White;
                    throw;
                }
            }
            if (colorString.Length == 8)
            {
                var a = colorString.Substring(0, 2);
                var r = colorString.Substring(2, 2);
                var g = colorString.Substring(4, 2);
                var b = colorString.Substring(6, 2);

                try
                {
                    byte ac = Byte.Parse(a, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                    byte rc = Byte.Parse(r, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                    byte gc = Byte.Parse(g, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                    byte bc = Byte.Parse(b, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                    color = Color.FromArgb(ac, rc, gc, bc);
                }
                catch (Exception)
                {
                    return Color.White;
                    throw;
                }
            }
            return color;
        }


        /// <summary>
        /// Extracts the hex digits from the string.
        /// </summary>
        /// <param name="colorString">The color string.</param>
        /// <returns></returns>
        private static string ExtractHexDigits(string colorString)
        {
            Regex HexDigits = new Regex(@"[abcdefABCDEF\d]+", RegexOptions.Compiled);

            var hexnum = new StringBuilder();
            foreach (char c in colorString)
                if (HexDigits.IsMatch(c.ToString(CultureInfo.InvariantCulture)))
                    hexnum.Append(c.ToString(CultureInfo.InvariantCulture));

            return hexnum.ToString();
        }

        internal static string ToCamelCase(String vText)
        {
            String output = vText;
            
            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            output = txtInfo.ToTitleCase(output).Replace("_", " ");
            //output = txtInfo.ToTitleCase(output).Replace("_", string.Empty).Replace(" ", string.Empty);
            //output = $"{output.First().ToString(CultureInfo.InvariantCulture).ToLowerInvariant()}{output.Substring(1)}";

            return output;
        }


        ///for System.Windows.Forms.TextBox
        public static System.Drawing.Point GetCaretPoint(System.Windows.Forms.TextBox textBox)
        {
            int start = textBox.SelectionStart;
            if (start == textBox.TextLength)
                start--;

            return textBox.GetPositionFromCharIndex(start);
        }

        ///for System.Windows.Controls.TextBox requires reference to the following dlls: PresentationFramework, PresentationCore & WindowBase.
        //So if not using those dlls you should comment the code below:

        //public static System.Windows.Point GetCaretPoint(System.Windows.Controls.TextBox textBox)
        //{
        //    return textBox.GetRectFromCharacterIndex(textBox.SelectionStart).Location;
        //}

    }
}
