using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;


namespace LAWC.Common
{

    //class PointComparer : IComparer<Point>
    //{
    //    public int Compare(Point first, Point second)
    //    {
    //        if (first.X == second.X)
    //        {
    //            return first.Y - second.Y;
    //        }
    //        else
    //        {
    //            return first.X - second.X;
    //        }

    //    }
    //}

    

    /// <summary>
    /// Extends the <see cref="System.Windows.Forms.Screen"/> class.
    /// </summary>
    public static class ScreenExtensions
    {
        /// <summary>
        /// Represents the different types of scaling.
        /// </summary>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511.aspx"/>
        public enum DpiType
        {
            EFFECTIVE = 0,
            ANGULAR = 1,
            RAW = 2,
        }

        public enum ProcessDPIAwareness
        {
            DPIUnaware = 0,
            SystemDPIAware = 1,
            PerMonitorDPIAware = 2
        }

        public const int MonitorDEFAULTTONEAREST = 2;


        //public static List<Screen> GetList(this Screen screen)
        public static List<Screen> GetList()
        {
            List<Screen> output = new List<Screen>();
            foreach (Screen s in Screen.AllScreens)
            {
                output.Add(s);
            }
            return output;
        }

        public static int GetID(this Screen screen)
        {            
            return int.Parse(screen.DeviceName.ToUpper(CultureInfo.InvariantCulture).Replace("DISPLAY", "").Replace(@"\", "").Replace(".", ""), CultureInfo.InvariantCulture); 
        }

        //Percentage - DPI Values
        //100%       - 96
        //125%       - 120
        //150%       - 144
        //200%       - 192
        public static void DPItoPercent(uint vDPI, out float vPercent)
        {
            vPercent = (float)Math.Round(vDPI / 96f * 100, 0);
        }


        //Percentage - DPI Values
        //100%       - 96
        //125%       - 120
        //150%       - 144
        //200%       - 192
        public static void GetDpi(this System.Windows.Forms.Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY)
        {
            var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var mon = NativeMethods.MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);
            NativeMethods.GetDpiForMonitor(mon, dpiType, out dpiX, out dpiY);

            //if (dpiX > 96&& dpiX < 140) dpiX = (uint)(Math.Floor(dpiX / 5.0) * 5.0); //round down to nearest 5
            //if (dpiY > 96) dpiY = (uint)(Math.Floor(dpiY / 5.0) * 5.0);

        }

        public static float GetDisplayScaleFactor(IntPtr windowHandle)
        {
            try
            {

                float s = NativeMethods.GetDpiForWindow(windowHandle);
                return s / 96f;
            }
            catch (InvalidOperationException)
            {
                // Or fallback to GDI solutions above
                return 1;
            }
        }


        public static void getScale()//(this System.Windows.Forms.Screen screen)
        {
            //uint x;
            //uint y;
            float scale;
            foreach (var scr in System.Windows.Forms.Screen.AllScreens)
            {
                Point pt = new Point(scr.Bounds.X + 1, 1);
                IntPtr windowptr = NativeMethods.MonitorFromPoint(pt, MonitorDEFAULTTONEAREST);
                //GetDpiForMonitor(windowptr, DpiType.ANGULAR, out x, out y);
                //x = GetDpiForWindow(windowptr);
                scale = GetDisplayScaleFactor(windowptr);
                 String result = scale.ToString(CultureInfo.InvariantCulture);
                //return scale;
            }
        }
                  


    } // class

}







    






