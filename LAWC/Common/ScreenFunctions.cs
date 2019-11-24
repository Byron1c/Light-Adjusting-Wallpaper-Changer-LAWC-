using Microsoft.Win32;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using static LAWC.FrmMain;
using static LAWC.Objects.Wallpaper;

namespace LAWC.Common
{
    internal static class ScreenFunctions
    {

        private static NativeMethods.RAMP _ramp;
        private static NativeMethods.RAMP defaultRAMP;

        
        static internal Boolean isWinTransparencyEnabled()
        {
            Boolean output;

            RegistryKey rkApp;

            rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);

            if (rkApp == null)
            {
                output = false;
                return output;
            }

            if (rkApp.GetValue("EnableTransparency") == null)
            {
                // The value doesn't exist, so Light Mode by default

                rkApp.CreateSubKey("EnableTransparency");
                output = false;
                return output;
            }


            // The value exists, check it
            //var result = rkApp.GetValue("AppsUseLightTheme");
            if (int.Parse(rkApp.GetValue("EnableTransparency").ToString(), CultureInfo.CurrentCulture) == 1)
            {
                output = true;
            }
            else
            {
                output = false;
            }

            rkApp.Close();
            return output;

        }


        /// <summary>
        /// Return the number of screens
        /// </summary>
        /// <returns>Count of screens based on the wallpaper mode</returns>
        internal static int GetScreenCount(MultiMonitorModes vMultiMonitorMode, WallpaperModes vWallpaperMode)
        {
            int screenCount;// = Screen.AllScreens.Length;

            //set the screen count default
            if (vMultiMonitorMode == MultiMonitorModes.DifferentOnAll && vWallpaperMode != WallpaperModes.Tile)
            {
                screenCount = Screen.AllScreens.Length;
            }
            else
            {
                screenCount = 1;
                // modify screen count in these specific circumstances
                switch (vWallpaperMode)
                {
                    case WallpaperModes.None:
                        break;
                    case WallpaperModes.Centre:
                        break;
                    case WallpaperModes.FillWidth:
                        break;
                    case WallpaperModes.FillHeight:
                        break;
                    case WallpaperModes.Stretch:
                        break;
                    case WallpaperModes.Tile:
                        //screenCount = 1;
                        //screenCount = Screen.AllScreens.Length;
                        break;
                    case WallpaperModes.Span:
                        //screenCount = 1;
                        screenCount = Screen.AllScreens.Length;  // Screen.AllScreens.Length;
                        break;
                    case WallpaperModes.LAWC:
                        screenCount = Screen.AllScreens.Length;
                        break;
                    default:
                        break;
                }
            }



            return screenCount;
        }



        /// <summary>
        /// Checks if the form is FULLY on the screen
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        internal static bool IsOnScreen(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Rectangle formRectangle = new Rectangle(form.Left, form.Top,
                                                         form.Width, form.Height);

                if (screen.WorkingArea.Contains(formRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if only the TOP LEFT CORNER is on a screen
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static bool IsOnScreenTopLeft(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Point formTopLeft = new Point(form.Left, form.Top);

                if (screen.WorkingArea.Contains(formTopLeft))
                {
                    return true;
                }
            }

            return false;
        }

        internal static void SetWindowsTransparency(Boolean vSetTransparency)
        {
            //CheckAutoRunState();
            RegistryKey rkApp;
            rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);

            if (rkApp == null)
            {
                return;
            }

            //0 = Dark theme
            //1 = Light theme

            if (vSetTransparency)
            {
                // set KEY
                rkApp.SetValue("EnableTransparency", 1);
            }
            else
            {
                // SET KEY
                try
                {
                    rkApp.SetValue("EnableTransparency", 0);
                }
                catch (IndexOutOfRangeException)
                {
                    // entry probably doesnt exist, ignore it
                }
            }

            rkApp.Close();
            //rkApp.Dispose();
        }

        static internal Boolean isWinDarkModeEnabled()
        {
            Boolean output;

            RegistryKey rkApp;

            rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);

            if (rkApp == null)
            {
                output = false;
                return output;
            }

            if (rkApp.GetValue("AppsUseLightTheme") == null)
            {
                // The value doesn't exist, so Light Mode by default

                rkApp.CreateSubKey("AppsUseLightTheme");
                output = false;
                return output;
            }


            // The value exists, check it
            //var result = rkApp.GetValue("AppsUseLightTheme");
            if (int.Parse(rkApp.GetValue("AppsUseLightTheme").ToString(), CultureInfo.CurrentCulture) == 1)
            {
                output = false;
            }
            else
            {
                output = true;
            }

            rkApp.Close();
            return output;

        }


        internal static void SetWindowsDarkMode(Boolean vSetDark)
        {
            //CheckAutoRunState();
            RegistryKey rkApp;
            rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);

            if (rkApp == null)
            {
                return;
            }

            //0 = Dark theme
            //1 = Light theme

            if (vSetDark)
            {
                // set KEY
                rkApp.SetValue("AppsUseLightTheme", 0);
            }
            else
            {
                // SET KEY
                try
                {
                    rkApp.SetValue("AppsUseLightTheme", 1);
                }
                catch (IndexOutOfRangeException)
                {
                    // entry probably doesnt exist, ignore it
                }
            }

            rkApp.Close();
            //rkApp.Dispose();
        }


        /// <summary>
        /// Used to find the lowest common divisor - used by aspect ratio calculations
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal static int LowestCommonDivisor(int a, int b)
        {
            int Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }


        public static void setGamma(int _gammaValue)
        {

            // backup the current ramp.. .ONLY THE FIRST TIME THIS IS USED
            if (defaultRAMP.Red == null && defaultRAMP.Green == null && defaultRAMP.Blue == null)
            {
                NativeMethods.GetDeviceGammaRamp(NativeMethods.GetDC(IntPtr.Zero), ref defaultRAMP);
            }


            // since gamma value is max 44 ,, we need to take the percentage from this because 
            // it needed from 0 - 100%
            double gValue = _gammaValue;
            gValue = Math.Floor(Convert.ToDouble((gValue / 2.27)));

            _gammaValue = Convert.ToInt16(gValue);

            if (_gammaValue != 0)
            {
                _ramp.Red = new ushort[256];
                _ramp.Green = new ushort[256];
                _ramp.Blue = new ushort[256];

                for (int i = 1; i < 256; i++)
                {
                    // gamma is a value between 3 and 44
                    _ramp.Red[i] =
                        _ramp.Green[i] =
                        _ramp.Blue[i] =
                        (ushort)
                        (Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, (_gammaValue + 5) * 0.1) * 65535 + 0.5)));
                }

                NativeMethods.SetDeviceGammaRamp(NativeMethods.GetDC(IntPtr.Zero), ref _ramp);
            }
        }


        public static void resetGamma() {
            //RAMP gamma = new RAMP();
            if (defaultRAMP.Red != null && defaultRAMP.Blue != null && defaultRAMP.Green != null)
            {
                NativeMethods.SetDeviceGammaRamp(NativeMethods.GetDC(IntPtr.Zero), ref defaultRAMP); //defaultRAMP);
            }
        }

    }
}
