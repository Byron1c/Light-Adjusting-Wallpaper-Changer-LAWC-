using System;
using Microsoft.Win32;
using System.IO;
using LAWC.Common;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using static LAWC.Common.ErrorHandling;
using System.Globalization;

namespace LAWC.Objects
{
    public sealed class Wallpaper
    {

        //Multi screen wallpaper examples
        //https://channel9.msdn.com/coding4fun/articles/MultiWall-Wallpaper-Tool-for-Multiple-Monitors
        // dunno if this is useful:
        // https://stackoverflow.com/questions/1540337/how-to-set-multiple-desktop-backgrounds-dual-monitor

        //https://stackoverflow.com/questions/43921924/incorrect-pixel-layout-occurs-sometimes-in-wpf-image-element

        //private const int SPI_SETDESKWALLPAPER = 20;
        //private const int SPIF_UPDATEINIFILE = 0x01;
        //private const int SPIF_SENDWININICHANGE = 0x02;
        private const int SpiGetdeskwallpaper = 0x73;
        private const int MaxPath = 260;

        //multi mon
        //https://stackoverflow.com/questions/1540337/how-to-set-multiple-desktop-backgrounds-dual-monitor
        internal Point primaryMonitorPoint = new Point(0, 0);


        internal enum WallpaperModes : int
        {
            None,
            Centre,
            FillWidth,
            FillHeight,
            Stretch,
            Tile,
            Span,
            LAWC,
        }

        internal enum ImageOrder
        {
            Ordered,
            Random,
            LowestViewCountOrdered,
            LowestViewCountRandom,
        }

        // original: PixelFormat.Format32bppArgb
        internal const System.Drawing.Imaging.PixelFormat CurrentPixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb; // Format64bppArgb;// .Format24bppRgb;//System.Drawing.Imaging.PixelFormat.Format32bppArgb; //System.Drawing.Imaging.PixelFormat.Format16bppArgb1555; //PixelFormat.Format32bppArgb


        Wallpaper() { }


        // Converts Bitmap to byte[]
        public static byte[] ImageToByte(Bitmap img)
        {
            var converter = new System.Drawing.ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }



        // NEED THESE REFERENCES FOR THE BITMAPSOURCE() FUNCTION
        // C:\Program Files(x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Core.dll
        //C:\Program Files(x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\PresentationCore.dll

        // Converts byte to bitmap source
        //public static BitmapSource BytesToBitmapSource(byte[] data)
        //{
        //    try
        //    {
        //        return (BitmapSource)new ImageSourceConverter().ConvertFrom(data);
        //    }
        //    catch (Exception)
        //    {
        //        //Logger.ErrorLog.WriteException(ex);
        //        //return BitmapToSource(EmptyBitmap());
        //        return null;
        //    }
        //}

        //public static Rectangle AddBounds(Rectangle sourceBounds, Rectangle newBounds)
        //{
        //    if (newBounds.Right > sourceBounds.Right)
        //        sourceBounds.Width += (newBounds.Right - sourceBounds.Width);

        //    if (newBounds.Bottom > sourceBounds.Bottom)
        //        sourceBounds.Height += (newBounds.Bottom - sourceBounds.Height);

        //    if (newBounds.Left < sourceBounds.Left)
        //    {
        //        sourceBounds.X = newBounds.X;
        //    }

        //    if (newBounds.Top < sourceBounds.Top)
        //    {
        //        sourceBounds.Y = newBounds.Y;
        //    }

        //    return sourceBounds;
        //}



        internal static String GetCurrentWallpaper()
        {
            RegistryKey theCurrentMachine = Registry.CurrentUser;
            RegistryKey theControlPanel = theCurrentMachine.OpenSubKey("Control Panel");
            RegistryKey theDesktop = theControlPanel.OpenSubKey("Desktop");
            return Convert.ToString(theDesktop.GetValue("Wallpaper"), CultureInfo.InvariantCulture);
        }


        /// <summary>
        ///  NOTES:  https://code.msdn.microsoft.com/windowsapps/CSSetDesktopWallpaper-2107409c
        ///  https://stackoverflow.com/questions/1061678/change-desktop-wallpaper-using-code-in-net
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        internal static Boolean SetWallpaper(string filename, WallpaperModes style)
        {
            Boolean output;// = false;

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

            switch (style)
            {
                case WallpaperModes.Tile:
                    //key.SetValue(@"WallpaperStyle", "0");
                    //key.SetValue(@"TileWallpaper", "1");
                    key.SetValue(@"WallpaperStyle", "0"); 
                    key.SetValue(@"TileWallpaper", "1");
                    //output = true;
                    break;

                case WallpaperModes.Centre:
                    key.SetValue(@"WallpaperStyle", "0");
                    key.SetValue(@"TileWallpaper", "0");
                    //output = true;
                    break;

                case WallpaperModes.Stretch:
                    key.SetValue(@"WallpaperStyle", "2");
                    key.SetValue(@"TileWallpaper", "0");
                    //output = true;
                    break;

                case WallpaperModes.FillWidth:  
                    key.SetValue(@"WallpaperStyle", "10"); //1
                    key.SetValue(@"TileWallpaper", "0");
                    //output = true;
                    break;

                case WallpaperModes.FillHeight: 
                    key.SetValue(@"WallpaperStyle", "6");
                    key.SetValue(@"TileWallpaper", "0");
                    //output = true;
                    break;

                case WallpaperModes.Span:
                    key.SetValue(@"WallpaperStyle", "22");
                    key.SetValue(@"TileWallpaper", "1");
                    //output = true;
                    break;

                //Same as SPAN, but just for custom building of the wallpaper (in other parts of the code)
                case WallpaperModes.LAWC:
                    key.SetValue(@"WallpaperStyle", "22");
                    key.SetValue(@"TileWallpaper", "1");
                    //output = true;
                    break;

                default:
                    //output = false;
                    break;

            }

            //WORKS, but black screens occasionally:
            //output = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            //output = NativeMethods.SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            output = NativeMethods.SystemParametersInfo(0x0014, 0, filename, 0x0001);

            //int result = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            //output = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, 0);  // 0 = dont save changes to theme   https://stackoverflow.com/questions/35716843/how-to-restore-undo-desktop-wallpaper-after-changing

            //output = Convert.ToBoolean(result);
            //if (Convert.ToBoolean(result) == false)
            //{
            //    MessageBox.Show("ERROR WARNING WARNING WARNING!!!!");
            //}

            if (output == false)
            {
                MessageBox.Show("There was an error setting the wallpaper.", "Error Setting Wallpaper", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return output;
        }


        internal string GetCurrentDesktopWallpaper()
        {
            string currentWallpaper = new string('\0', MaxPath);
            NativeMethods.SystemParametersInfo(SpiGetdeskwallpaper, currentWallpaper.Length, currentWallpaper, 0);
            return currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0'));
        }

        internal static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static string SaveWallpaper(Bitmap bmp, int vFileNum, ImageFormat vFormat, int vQuality, String vSettingsFullPath)
        {
            //System.Drawing.Imaging.EncoderParameter qualityParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, vQuality);
            //System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
            //encoderParams.Param[0] = qualityParam;

            ImageCodecInfo jpgEncoder = GetEncoder(vFormat); // ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID  
            // for the Quality parameter category.  
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.  
            // An EncoderParameters object has an array of EncoderParameter  
            // objects. In this case, there is only one  
            // EncoderParameter object in the array.  
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, (long)vQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            //bmp.Save(@"c:\TestPhotoQualityFifty.jpg", jpgEncoder, myEncoderParameters);

            //string root = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); //.CommonPictures); //.MyPictures);
            string path = GetWallpaperPath(vFileNum, FrmMain.getWallpaperExtension(vFormat));//Path.Combine(root, "LAWC_Image.bmp");

            try
            {
                bmp.Save(path, jpgEncoder, myEncoderParameters);

                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        EncoderParameters e = new EncoderParameters();
                        
                //        //bmp.Save(memory, ImageFormat.Png);
                //        //bmp.Save(memory, ImageFormat.Jpeg);
                        
                //        //bmp.Save(memory, vFormat);
                //        bmp.Save(memory, vFormat, encoderParams);

                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}

                return path;

            }
            catch (IOException ex)
            {
                Boolean vLogged = false;
                //WriteError("Wallpaper Error 001: Unable to save wallpaper " + path + "\n" + ex.Message, true);
                new ApplicationException(string.Format(CultureInfo.InvariantCulture, "Problem Saving Wallpaper: " + path + ""), ex).Log(ref vLogged, vSettingsFullPath);

                return string.Empty;

            }

            //return path;

        }


        //private static void WriteError(String vMessage)
        //{
        //    //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(), DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day
        //    //    + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + " " 
        //    //    + ":  " + vMessage, settings.WriteToLog);

        //    FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable),
        //         String.Format("{0}-{1}-{2} {3}:{4}:{5}: {6}",
        //            DateTime.Now.Year.ToString("D2"),
        //            DateTime.Now.Month.ToString("D2"),
        //            DateTime.Now.Day.ToString("D2"),

        //            DateTime.Now.Hour.ToString("D2"),
        //            DateTime.Now.Minute.ToString("D2"),
        //            DateTime.Now.Second.ToString("D2"),

        //            vMessage

        //         ), true);

        //}

        
        public static string GetPicuresPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }


        public static string GetTEMPPath()
        {
            // this returns app path ie. C:\Users\MyUserName\AppData\Local\Temp
            return Environment.GetEnvironmentVariable("TEMP");

            // this returns windows path ie C:\WINDOWS\TEMP
            //return Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine);
        }


        //public static string SaveWallpaper(Bitmap bmp, String vFilename, ImageFormat vImageFormat)
        //{
        //    //string root = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); //.CommonPictures); //.MyPictures);
        //    string path = Path.Combine(GetPicuresPath(), vFilename);

        //    bmp.Save(path, vImageFormat); //ImageFormat.Bmp);

        //    return path;
        //}


        public static string GetWallpaperPath(int vNum, String vExtension)
        {
            //string root = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);// .CommonPictures);//MyPictures);
            string path = Path.Combine(GetTEMPPath(), "LAWC_Image" + vNum + vExtension);
            //string path = Path.Combine(GetTEMPPath(), "LAWC_Image" + vNum + ".png");
            //string path = Path.Combine(GetTEMPPath(), "LAWC_Image" + vNum + ".jpg");
            //string path = Path.Combine(GetTEMPPath(), "LAWC_Image" + vNum + ".bmp");

            return path;
        }


    }
}
