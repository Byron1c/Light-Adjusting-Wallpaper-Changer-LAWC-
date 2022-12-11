using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using LAWC.Objects;
using System.IO;
using System.Runtime.InteropServices;
using static LAWC.Objects.Wallpaper;
using System.Windows.Media.Imaging;
using MetadataExtractor;
using System.Windows.Forms;
using static LAWC.Common.ErrorHandling;
using System.Globalization;
using ImageProcessor;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LAWC.Common
{
    internal static class ImageFunctions
    {
        internal static Size ImageProcessSizeSmall = new Size(8, 4);
        internal static Size ImageProcessSizeMedium = new Size(16, 9); //(32, 18); // 16:9 default
        internal static Size ImageProcessSizeLarge = new Size(32, 18);


        internal static Bitmap CropImage(Image source, int x, int y, int width, int height)
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }


        //http://csharp-me.blogspot.com/2014/01/simple-image-cropping-in-c.html
        internal static Bitmap CropImage2(Bitmap source, int x, int y, int width, int height)
        {
            int cropX = x;
            int cropY = y;
            int cropWidth = width;
            int cropHeight = height;

            try
            {
                Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
                //Bitmap OriginalImage = new Bitmap(source, source.Width, source.Height);
                Bitmap _img = new Bitmap(cropWidth, cropHeight);
                Graphics g = Graphics.FromImage(_img);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(source, 0, 0, rect, GraphicsUnit.Pixel);

                return new Bitmap(_img);
                //return _img;

            }
            catch (ImageProcessingException ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }


        internal static void CropImage3(ref Bitmap source, int x, int y, int width, int height)
        {
            //return;
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.CompositingMode = CompositingMode.SourceOver;

                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            source = (Bitmap)bmp.Clone();
            bmp.Dispose();
        }


        /// <summary>
        /// Given a filename, retrieves a cached Bitmap or reads from disk.
        /// If filename is not valid, a 1x1 dummy is returned (no error)
        /// </summary>
        /// <param name="filename">The name of the file to locate</param>
        /// <param name="index">Which screen this is for (used for caching)</param>
        /// <returns></returns>
        internal static void BitmapFromFile(ref Bitmap vImageResult, string filename, String vSettingsFullPath)
        {
            try
            {
                // If it's not in cache but it's on disk...
                if (File.Exists(filename))
                {
                    using (Stream sr = File.OpenRead(filename))
                    {

                        if (vImageResult != null) vImageResult.Dispose();
                        // Use FromStream, not FromFile to avoid an unnecessary lock
                        vImageResult = (Bitmap)Bitmap.FromStream(sr);

                    }
                }
                else
                {
                    //WriteError("Error 005: BitmapFromFile - file doesnt exist " + filename);
                    ProcessError(null, ErrorMessageType.FileDoesntExist, false, false, string.Format(CultureInfo.InvariantCulture, "{0}", filename), vSettingsFullPath);
                    //return image not found?
                    vImageResult = null;
                }
            }
            catch (FileLoadException ex)
            {
                //WriteError("Error 005b: BitmapFromFile - file: " + filename + "\\nException: " + ex.Message);
                ProcessError(ex, ErrorMessageType.FileProblem, false, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
            }

        }


        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            Image img = (Image)converter.ConvertFrom(byteArrayIn);

            return img;
        }

        //https://stackoverflow.com/questions/8846654/read-image-and-determine-if-its-corrupt-c-sharp
        public static bool IsValidGDIPlusImage(string filename)
        {
            try
            {
                using (var bmp = new Bitmap(filename))
                {
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsValidGDIPlusImage(byte[] imageData)
        {
            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    using (var bmp = new Bitmap(ms))
                    {
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

            /// <summary>
            /// load and return an image
            /// </summary>
            /// <param name="vImagePath"></param>
            /// <returns></returns>
        internal static Image LoadImage(String vImagePath, String vSettingsFullPath)
        {

            if (IsValidGDIPlusImage(vImagePath) && System.IO.File.Exists(vImagePath))
            {

                //using (FileStream fileStream = new FileStream(vImagePath, FileMode.Open, FileAccess.Read))
                using (FileStream fileStream = new FileStream(vImagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    try
                    {
                        return System.Drawing.Bitmap.FromStream(fileStream);
                        //return Image.FromStream(fileStream);
                    }
                    catch (FileLoadException ex)
                    {
                        //WriteError("Error 008: Unable to load image (" + vImagePath + "): " + ex.Message);
                        //ProcessError(ex, ErrorMessageType.FileProblem, true, false, string.Format(CultureInfo.InvariantCulture, "{0}", vImagePath), vSettingsFullPath);
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }


        internal static String GetImageMetaData(String vPath)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(vPath); //ImageMetadataReader.ReadMetadata(vPath);

            String output = "File: " + vPath + "\n\n";

            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    //Console.WriteLine(tag);
                    if (directory.Name.ToUpper(CultureInfo.InvariantCulture).Contains("EXIF THUMBNAIL") == false
                        && directory.Name.ToUpper(CultureInfo.InvariantCulture).Contains("INTEROPERABILITY") == false
                        && directory.Name.ToUpper(CultureInfo.InvariantCulture).Contains("XMP") == false
                        )
                    {
                        if (directory.Name.ToUpper(CultureInfo.InvariantCulture).Contains("ICC PROFILE"))
                        {
                            if (tag.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("RED TRC") == false
                                && tag.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("GREEN TRC") == false
                                && tag.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("BLUE TRC") == false
                                )
                            {
                                //output += "" + tag + "\n";
                                output += "" + tag.Name + " = " + tag.Description + "\n";
                            }

                        }
                        else
                        {
                            //output += "" + tag + "\n";
                            output += "" + tag.Name + " = " + tag.Description + "\n";
                        }
                    }

                }
                output += "" + "\n";
            }

            return output;

        }

        internal static String GetImageMetaData(String vPath, String vDirectory, String vTagName)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(vPath); //ImageMetadataReader.ReadMetadata(vPath);

            String output = string.Empty; //= "File: " + vPath + "\n\n";

            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    //if (directory.Name.ToLower().Contains(vDirectory)) //"exif"
                    if (directory.Name.ToUpperInvariant().Contains(vDirectory.ToUpperInvariant())) //"exif"
                    {
                        if (tag.ToString().ToUpperInvariant().Contains(vTagName.ToUpperInvariant())) // "title"))
                        {
                            //MessageBox.Show(tag.ToString(CultureInfo.InvariantCulture));
                            output = tag.Description;
                        }
                    }
                }
            }

            return output;

        }

        internal static BitmapMetadata GetImageMetaDataMS(String vPath)
        {
            // get extension to vPath
            FileInfo file = new FileInfo(vPath);
            String ext = file.Extension.ToUpperInvariant().Replace(".", "");

            // if the 
            if (ext != "jpg") return null;

            BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;
            BitmapMetadata importedMetaData = new BitmapMetadata(ext); // ("jpg");
            using (Stream sourceStream = File.Open(vPath, FileMode.Open, FileAccess.Read))
            {
                BitmapDecoder sourceDecoder = BitmapDecoder.Create(sourceStream, createOptions, BitmapCacheOption.Default);
                // Check source is has valid frames 
                if (sourceDecoder.Frames[0] != null && sourceDecoder.Frames[0].Metadata != null)
                {
                    sourceDecoder.Frames[0].Metadata.Freeze();
                    // Get a clone copy of the metadata
                    BitmapMetadata sourceMetadata = sourceDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;
                    importedMetaData = sourceMetadata;
                }
            }
            return importedMetaData;
        }


        //https://stackoverflow.com/questions/39483712/to-clear-loaded-images-in-a-picturebox-c
        /// <summary>
        /// Will correctly clear and load an image to a PictureBox
        /// </summary>
        /// <param name="pbox">the control that the image is loaded to</param>
        /// <param name="file">the full path to the image</param>
        internal static Boolean loadImageToPictureBox(System.Windows.Forms.PictureBox pbox, string file)
        {
            if (pbox.Image != null)
            {
                var dummy = pbox.Image;
                pbox.Image = null;
                dummy.Dispose();
            }
            if (System.IO.File.Exists(file))
            {
                pbox.Image = Image.FromFile(file);
                return true;
            }

            return false;
        }

        /// <summary>
        /// https://www.codicode.com/art/resize_images_and_keep_aspect_ra.aspx
        /// </summary>
        /// <param name="Org"></param>
        /// <param name="Des"></param>
        /// <param name="FinalWidth"></param>
        /// <param name="FinalHeight"></param>
        /// <param name="ImageQuality"></param>
        internal static void ResizeImage(Bitmap bmp, string Des, int FinalWidth, int FinalHeight, int ImageQuality)
        {
            System.Drawing.Bitmap NewBMP;
            System.Drawing.Graphics graphicTemp;
            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Org);

            int iWidth;
            int iHeight;
            if ((FinalHeight == 0) && (FinalWidth != 0))
            {
                iWidth = FinalWidth;
                iHeight = (bmp.Size.Height * iWidth / bmp.Size.Width);
            }
            else if ((FinalHeight != 0) && (FinalWidth == 0))
            {
                iHeight = FinalHeight;
                iWidth = (bmp.Size.Width * iHeight / bmp.Size.Height);
            }
            else
            {
                iWidth = FinalWidth;
                iHeight = FinalHeight;
            }

            NewBMP = new System.Drawing.Bitmap(iWidth, iHeight);
            graphicTemp = System.Drawing.Graphics.FromImage(NewBMP);
            graphicTemp.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graphicTemp.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphicTemp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphicTemp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphicTemp.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphicTemp.DrawImage(bmp, 0, 0, iWidth, iHeight);
            graphicTemp.Dispose();
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
            encoderParams.Param[0] = encoderParam;
            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (int fwd = 0; fwd <= arrayICI.Length - 1; fwd++)
            {
                if (arrayICI[fwd].FormatDescription.Equals("JPEG", StringComparison.InvariantCulture))
                {
                    NewBMP.Save(Des, arrayICI[fwd], encoderParams);
                }
            }

            encoderParams.Dispose();
            NewBMP.Dispose();
            bmp.Dispose();
        }


        /// <summary>
        /// Resize the image to the specified width and height.
        /// https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="newWidth">The width to resize to.</param>
        /// <param name="newHeight">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        internal static Bitmap ResizeImage(Image image, int newWidth, int newHeight)
        {
            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var destImage = new Bitmap(newWidth, newHeight);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        //https://stackoverflow.com/questions/7230687/fast-good-quality-image-resizing-algorithm-in-c-sharp-without-using-gdi-wpf
        internal static void ResizeImage(string filename)//, float scale)
        {
            using (var bitmap = Image.FromFile(filename))
            using (var resized = ResizeBitmap(bitmap, 0.1f, InterpolationMode.HighQualityBicubic))
            {
                var newFile = Path.ChangeExtension(filename, ".thumbnail" + Path.GetExtension(filename));
                if (File.Exists(newFile))
                    File.Delete(newFile);
                resized.Save(newFile);
            }
        }
        //https://stackoverflow.com/questions/7230687/fast-good-quality-image-resizing-algorithm-in-c-sharp-without-using-gdi-wpf
        internal static Bitmap ResizeBitmap(Image source, float scale, InterpolationMode quality)
        {
            if (source == null)
                throw new ArgumentNullException(source.ToString());

            // Figure out the new size.
            var width = (int)(source.Width * scale);
            var height = (int)(source.Height * scale);

            Bitmap bmp;

            // Create the new bitmap.
            // Note that Bitmap has a resize constructor, but you can't control the quality.
            try
            {
                bmp = new Bitmap(width, height);
            
                using (var g = Graphics.FromImage(bmp))
                {
                    g.InterpolationMode = quality;
                    g.DrawImage(source, new Rectangle(0, 0, width, height));
                    g.Save();
                }

            }
            catch (ArgumentException)
            {
                bmp = new Bitmap(1920, 1080);
            }

            return bmp;
        }


        /// <summary>
        /// The first argument is the source image, the second defines the area to crop, and 
        /// the third optionally specifies the size of the target image. The third argument is 
        /// only necessary if you would like to scale your image in any way. If you just want a 
        /// straight-up crop, leave it empty
        /// http://www.levibotelho.com/development/how-to-crop-resize-images-in-c/
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="destinationRectangle"></param>
        /// <returns></returns>
        internal static Bitmap CropImage(Image originalImage, Rectangle sourceRectangle, Rectangle? destinationRectangle = null)
        {
            if (destinationRectangle == null)
            {
                destinationRectangle = new Rectangle(Point.Empty, sourceRectangle.Size);
            }

            var croppedImage = new Bitmap(destinationRectangle.Value.Width,
                destinationRectangle.Value.Height);
            using (var graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(originalImage, destinationRectangle.Value,
                    sourceRectangle, GraphicsUnit.Pixel);
            }
            return croppedImage;
        }

        internal static Bitmap CropBitmap(Bitmap bitmap, int x, int y, int w, int h)
        {
            Rectangle rect = new Rectangle(x, y, w, h);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }

        internal static Bitmap cloneCrop(Bitmap bmp, int startX, int startY, int width, int height)
        {
            Rectangle srcRect = Rectangle.FromLTRB(startX, startY, width, height);
            Bitmap dest = new Bitmap(srcRect.Width, srcRect.Height);
            Rectangle destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (Graphics graphics = Graphics.FromImage(dest))
            {
                graphics.DrawImage(bmp, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return dest;
        }


        /// <summary>
        /// https://stackoverflow.com/questions/1940581/c-sharp-image-resizing-to-different-size-while-preserving-aspect-ratio
        /// </summary>
        /// <param name="path"></param>
        /// <param name="originalFilename"></param>
        /// <param name="canvasWidth"></param>
        /// <param name="canvasHeight"></param>
        /// <param name="originalWidth"></param>
        /// <param name="originalHeight"></param>
        internal static void resizeImageUNTESTED(string path,
                     int canvasWidth, int canvasHeight,
                     int originalWidth, int originalHeight)
        {
            Image image = Image.FromFile(path);

            System.Drawing.Image thumbnail =
                new Bitmap(canvasWidth, canvasHeight); // changed parm names
            System.Drawing.Graphics graphic =
                         System.Drawing.Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            /* ------------------ new code --------------- */

            // Figure out the ratio
            double ratioX = (double)canvasWidth / (double)originalWidth;
            double ratioY = (double)canvasHeight / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posX = Convert.ToInt32((canvasWidth - (originalWidth * ratio)) / 2);
            int posY = Convert.ToInt32((canvasHeight - (originalHeight * ratio)) / 2);

            graphic.Clear(Color.White); // white padding
            graphic.DrawImage(image, posX, posY, newWidth, newHeight);

            /* ------------- end new code ---------------- */

            System.Drawing.Imaging.ImageCodecInfo[] info =
                             ImageCodecInfo.GetImageEncoders();
            EncoderParameters encoderParameters;
            encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality,
                             100L);
            thumbnail.Save(path + newWidth + "." + path, info[1],
                             encoderParameters);
        }


        /// <summary>
        /// Resize the image while maintaining the exiting aspect ratio
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="vColour"></param>
        /// <param name="vInterpolation"></param>
        /// <param name="vPercentSize"></param>
        internal static void ResizeImageFixedAspectRatio(ref Bitmap imgPhoto, int Width, int Height, System.Drawing.Color vColour,
            InterpolationMode vInterpolation, float vPercentSize)
        {

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent; // = 0;
            float nPercentW; // = 0;
            float nPercentH; // = 0;

            nPercentW = ((float)Width / (float)sourceWidth) * vPercentSize;
            nPercentH = ((float)Height / (float)sourceHeight) * vPercentSize;
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            using (Bitmap bmPhoto = new Bitmap(Width, Height, Wallpaper.CurrentPixelFormat)) // System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                 imgPhoto.VerticalResolution);

                using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
                {
                    grPhoto.Clear(vColour); //System.Drawing.Color.Red
                    grPhoto.InterpolationMode = vInterpolation; //InterpolationMode.HighQualityBicubic;
                    grPhoto.CompositingMode = CompositingMode.SourceOver;
                    grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                    grPhoto.SmoothingMode = SmoothingMode.HighQuality;

                    if (vInterpolation == InterpolationMode.Low)
                    {
                        grPhoto.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    }
                    else
                    {
                        grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    }

                    if (nPercentH < nPercentW)
                    {
                        destY += (Height - destHeight) / 2;
                    }
                    else
                    {
                        destX += (Width - destWidth) / 2;
                    }


                    grPhoto.DrawImage(imgPhoto,
                        new Rectangle(destX - 1, destY - 1, destWidth + 1, destHeight + 1),
                        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                        GraphicsUnit.Pixel);

                }

                if (imgPhoto != null) imgPhoto.Dispose();
                imgPhoto = (Bitmap)bmPhoto.Clone();
                //bmPhoto.Dispose();
            }


        }


        internal static void ResizeFixedAspectRatio(ref Bitmap imgPhoto, int Width, int Height, System.Drawing.Color vColour, InterpolationMode vInterpolation)
        {

            ResizeImageFixedAspectRatio(ref imgPhoto, Width, Height, vColour, vInterpolation, 1.0f);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="vAccuracy">1 = Default (Full); 2 = faster, 3 = fast, 4 = Very Fast</param>
        /// <returns></returns>
        internal static ImageStats GetImageStats(Bitmap bmp)
        {
            //int accuracy = vAccuracy;
            //if (accuracy > 16) accuracy = 16;
            //if (accuracy < 1) accuracy = 1;

            //Bitmap bitmap = // the bitmap
            var colors = new List<System.Drawing.Color>();

            int r = 0;
            int g = 0;
            int b = 0;
            int total = 0;

            for (int x = 0; x < bmp.Size.Width; x += 1)
            {
                for (int y = 0; y < bmp.Size.Height; y += 1)
                {
                    //if (x < bmp.Size.Width && y < bmp.Size.Height)
                    {
                        System.Drawing.Color c = bmp.GetPixel(x, y);
                        colors.Add(c); //System.Windows.Media.Color.FromArgb(c.A, c.R, c.B, c.G));

                        r += c.R;
                        g += c.G;
                        b += c.B;

                        total++;
                    }
                }
            }

            float imageBrightness = colors.Average(color => color.GetBrightness());

            //Calculate average colour
            r /= total;
            g /= total;
            b /= total;

            ImageStats output = new ImageStats
            {
                AverageColour = System.Drawing.Color.FromArgb(r, g, b),
                Brightness = imageBrightness,
                //output.Brightness = getAverageBrightness(bmp);

                Aspect = (float)((float)(bmp.Width) / (float)(bmp.Height))
            };

            return output;

        }


        internal static float GetAverageBrightness(Bitmap bmp)
        {
            //Bitmap bitmap = // the bitmap
            var colors = new List<System.Drawing.Color>();
            for (int x = 0; x < bmp.Size.Width; x++)
            {
                for (int y = 0; y < bmp.Size.Height; y++)
                {
                    System.Drawing.Color c = bmp.GetPixel(x, y);
                    colors.Add(c); //System.Windows.Media.Color.FromArgb(c.A, c.R, c.B, c.G));

                }
            }

            float imageBrightness = colors.Average(color => color.GetBrightness());

            return imageBrightness;
        }


        internal static void SaveBitmap(String vPath, Bitmap vBitmap, long vQuality)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            FileInfo file = new FileInfo(vPath);



            switch (file.Extension.ToUpper(CultureInfo.InvariantCulture))
            {
                //case ImageFormat.Jpeg:
                //    // Get an ImageCodecInfo object that represents the JPEG codec.
                //    myImageCodecInfo = GetEncoderInfo("image/jpeg");

                case "PNG":
                    // Get an ImageCodecInfo object that represents the PNG codec.
                    myImageCodecInfo = GetEncoderInfo("image/png");
                    break;

                case "TIFF":
                    // Get an ImageCodecInfo object that represents the TIFF codec.
                    myImageCodecInfo = GetEncoderInfo("image/tiff");
                    break;

                case "GIF":
                    // Get an ImageCodecInfo object that represents the TIFF codec.
                    myImageCodecInfo = GetEncoderInfo("image/gif");
                    break;

                default:
                    // Get an ImageCodecInfo object that represents the JPEG codec.
                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    break;
            }


            // Create an Encoder object based on the GUID 

            // for the Quality parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Quality;


            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with quality level 
            myEncoderParameter = new EncoderParameter(myEncoder, vQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;


            try
            {
                using (Bitmap tempImage = new Bitmap(vBitmap))
                {
                    tempImage.Save(vPath, myImageCodecInfo, myEncoderParameters);
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine("DEBUG::LoadImages()::Error attempting to create image::" + e.Message);
            }

        }


        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }


        internal static System.Drawing.Color GetAverageColor(Bitmap bmp)
        {

            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    System.Drawing.Color clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        internal static void ImageHSVAdjust(ref Bitmap vImage, float vSat, float vHue, float vVal, float vRed, float vGreen, float vBlue, Boolean vUseGDI)
        {
            if (vImage == null) return;
            // quick test code, do not copy paste in real apps
            //this.Text = "H: " + trHue.Value.ToString()
            //    + " S: " + (trSat.Value / 100.0f).ToString("0.0")
            //    + " B: " + (trBri.Value / 100.0f).ToString("0.0");

            Bitmap b; // = null;
            if (vUseGDI)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Img.QColorMatrix qm = new Img.QColorMatrix();
                qm.RotateHue(vHue);
                qm.SetSaturation2(vSat);
                qm.SetBrightness(vVal);
                qm.ScaleColors(vRed, vGreen, vBlue);
                imageAttr.SetColorMatrix(qm.ToColorMatrix());
                b = new Bitmap(vImage.Width, vImage.Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    Rectangle r = new Rectangle(0, 0, vImage.Width, vImage.Height);
                    g.DrawImage(vImage, r, 0, 0, vImage.Width, vImage.Height, GraphicsUnit.Pixel, imageAttr);
                }
            }
            else
            {
                b = (Bitmap)vImage.Clone();
                Img.QColorMatrix qm = new Img.QColorMatrix();
                qm.RotateHue(vHue);
                qm.SetSaturation2(vSat);
                qm.SetBrightness(vVal);
                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        c = Img.QColorMatrix.Vector2Color(qm.TransformVector(Img.QColorMatrix.Color2Vector(c), true));
                        b.SetPixel(i, j, c);
                    }
                }
            }
            if (b != null)
            {
                vImage = b;
            }




            //// Create the color matrix and set the appropriate values
            //ColorMatrixExt clrMtx = new ColorMatrixExt();
            //clrMtx.SetSaturation(vSat);
            //clrMtx.RotateHue(vHue);
            //clrMtx.ScaleValue(vVal);
            //clrMtx.ScaleColors(vRed, vGreen, vBlue);

            //// Set the color matrix to the image attributes
            //ImageAttributes imageAttributes = new ImageAttributes();
            //imageAttributes.SetColorMatrix(clrMtx);

            //using (Graphics NewGraphics = Graphics.FromImage(vImage))
            //{

            //    // FIX THIS ERRORL Palette = 'vImage.Palette' threw an exception of type 'System.ArgumentException'

            //    // Draw the image on the canvas with the provided image attributes
            //    NewGraphics.DrawImage(vImage, new Rectangle(0, 0, vImage.Width, vImage.Height), 0, 0, vImage.Width, vImage.Height, GraphicsUnit.Pixel, imageAttributes);
            //}

            //if (imageAttributes != null) imageAttributes.Dispose();

        }



        /// <summary>
        /// Tints an image with a given colour
        /// </summary>
        /// <param name="vImage"></param>
        /// <param name="vTintColour"></param>
        /// <param name="vPercent">0 - 1f</param>
        /// <param name="vTintStrengthPercent">0 - 100</param>
        internal static void ImageTint(ref Bitmap vImage, System.Drawing.Color vTintColour, float vPercent, int vTintStrengthPercent)
        {

            float percent = vPercent;
            int r = vTintColour.R;
            int g = vTintColour.G;
            int b = vTintColour.B;


            float redPercent = r / 255f;
            float greenPercent = g / 255f;
            float bluePercent = b / 255f;


            float red;
            float green;
            float blue;

            // stop the percent from actually making it zero cause that will negate all the tint
            if (percent <= 0.0f)
            {
                percent = 0.0000001f;
            }

            red = redPercent * (vTintStrengthPercent / 100f) * percent;
            green = greenPercent * (vTintStrengthPercent / 100f) * percent;
            blue = bluePercent * (vTintStrengthPercent / 100f) * percent;

            ImageFunctions.ColorTint(ref vImage, red, green, blue);
        }


        internal static Bitmap ImageTint(System.Drawing.Color vTintColour, Bitmap vImage, float vPercent, int vTintStrength)
        {

            float percent = vPercent;
            int r = vTintColour.R;
            int g = vTintColour.G;
            int b = vTintColour.B;


            float redPercent = r / 255f;
            float greenPercent = g / 255f;
            float bluePercent = b / 255f;


            float red;
            float green;
            float blue;

            // stop the percent from actually making it zero cause that will negate all the tint
            if (percent <= 0.0f)
            {
                percent = 0.0000001f;
            }

            red = redPercent * (vTintStrength / 100f) * percent;
            green = greenPercent * (vTintStrength / 100f) * percent;
            blue = bluePercent * (vTintStrength / 100f) * percent;

            return ImageFunctions.ColorTint(vImage, red, green, blue);
        }


        /// <summary>  
        /// method for changing the opacity of an image  1f=Solid
        /// </summary>  
        /// <param name="vSourceImage">image to set opacity on</param>  
        /// <param name="opacity">percentage of opacity 0-1f  1f=Solid</param>  
        /// <returns></returns>  
        internal static void SetImageOpacity(ref Bitmap vSourceImage, float opacity)
        {

            if (vSourceImage != null)
            {

                //try
                //{
                //create a Bitmap the size of the image provided  
                using (Bitmap bmp = new Bitmap(vSourceImage.Width, vSourceImage.Height))
                {

                    //create a graphics object from the image  
                    using (Graphics gfx = Graphics.FromImage(bmp))
                    {

                        //create a color matrix object  
                        ColorMatrix matrix = new ColorMatrix
                        {
                            //set the opacity  
                            Matrix33 = opacity
                        };

                        //create image attributes  
                        ImageAttributes attributes = new ImageAttributes();

                        //set the color(opacity) of the image  
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        //now draw the image  
                        gfx.DrawImage(vSourceImage, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, vSourceImage.Width, vSourceImage.Height, GraphicsUnit.Pixel, attributes);

                    }

                    vSourceImage.Dispose();
                    vSourceImage = (Bitmap)bmp.Clone();
                    //bmp.Dispose();
                    //if (bmp != null) bmp.Dispose();


                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //    //return null;
                //}

            }
            else
            {
                //return null;
            }
        }


        /// <summary>  
        /// method for changing the opacity of an image  
        /// </summary>  
        /// <param name="image">image to set opacity on</param>  
        /// <param name="opacity">percentage of opacity</param>  
        /// <returns></returns>  
        internal static Bitmap SetImageOpacity(Bitmap image, float opacity, String vSettingsFullPath)
        {

            if (image != null)
            {

                try
                {
                    //create a Bitmap the size of the image provided  
                    using (Bitmap bmp = new Bitmap(image.Width, image.Height))
                    {

                        //create a graphics object from the image  
                        using (Graphics gfx = Graphics.FromImage(bmp))
                        {

                            //create a color matrix object  
                            ColorMatrix matrix = new ColorMatrix
                            {

                                //set the opacity  
                                Matrix33 = opacity
                            };

                            //create image attributes  
                            ImageAttributes attributes = new ImageAttributes();

                            //set the color(opacity) of the image  
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                            //now draw the image  
                            gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                        }

                        return (Bitmap)bmp.Clone();

                    }
                }
                catch (ImageProcessingException ex)
                {
                    //WriteError("Error 008c: Unable to set the image opacity: " + ex.Message);
                    ProcessError(ex, ErrorMessageType.SetImageOpacity, false, false, string.Empty, vSettingsFullPath);
                    return null;
                }

            }
            else
            {
                return null;
            }
        }


        // FROM: http://softwarebydefault.com/2013/04/12/bitmap-color-tint/
        internal static void ColorTint(ref Bitmap sourceBitmap, float redTint, float greenTint, float blueTint)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                    ImageLockMode.ReadOnly, Wallpaper.CurrentPixelFormat); // System.Drawing.Imaging.PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            float blue; // = 0;
            float green; // = 0;
            float red; // = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * blueTint;
                green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * greenTint;
                red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * redTint;


                if (blue > 255)
                { blue = 255; }


                if (green > 255)
                { green = 255; }


                if (red > 255)
                { red = 255; }


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;


            }


            using (Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height))
            {
                BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, Wallpaper.CurrentPixelFormat);//System.Drawing.Imaging.PixelFormat.Format32bppArgb);


                Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
                resultBitmap.UnlockBits(resultData);

                if (sourceBitmap != null) sourceBitmap.Dispose();
                sourceBitmap = (Bitmap)resultBitmap.Clone();
                //resultBitmap.Dispose();
                //if (resultBitmap != null) resultBitmap.Dispose();
            }



        }



        private static void adjustHSVOrangeIMAGEFACTORYTEST(ref Bitmap vBitmap)
        {
            //if (settings.UseHSV)
            {
                //float sat = 1.1f;
                //float hue = 0f;
                //float val = 0.9f;
                //float red = 1.1f;
                //float green = 1.1f;
                //float blue = 0.8f;
                try
                {
                    ImageFactory imageFactory = new ImageFactory();
                    imageFactory.Load((Image)vBitmap); //60ms

                    imageFactory.Brightness(10); //146ms
                    imageFactory.Saturation(10); //256ms
                    imageFactory.Tint(Color.Red); //49ms
                    imageFactory.AutoRotate(); //8ms
                    imageFactory.Contrast(10); //238ms
                    imageFactory.Gamma(1.1f); //220ms
                    imageFactory.GaussianSharpen(2); //838ms
                    imageFactory.GaussianBlur(2); //866ms
                    imageFactory.Quality(90); //14ms
                    imageFactory.RoundedCorners(3); //296ms
                    imageFactory.Vignette(); //145ms


                    vBitmap = (Bitmap)imageFactory.Image.Clone();
                    // ImageFunctions.ImageHSVAdjust(ref vBitmap, sat, hue, val, red, green, blue);
                    //vBitmap = ImageFunctions.ImageHSVAdjust(vBitmap, sat, hue, val, red, green, blue);
                    imageFactory.Dispose();
                }
                catch (OutOfMemoryException)
                {
                    //throw;
                }

            }
        }


        // FROM: http://softwarebydefault.com/2013/04/12/bitmap-color-tint/
        internal static Bitmap ColorTint(this Bitmap sourceBitmap, float redTint, float greenTint, float blueTint)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                    ImageLockMode.ReadOnly, Wallpaper.CurrentPixelFormat); // System.Drawing.Imaging.PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            float blue; // = 0;
            float green; // = 0;
            float red; // = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * blueTint;
                green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * greenTint;
                red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * redTint;


                if (blue > 255)
                { blue = 255; }


                if (green > 255)
                { green = 255; }


                if (red > 255)
                { red = 255; }


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;

            }

            using (Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height))
            {

                BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, Wallpaper.CurrentPixelFormat); // System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
                resultBitmap.UnlockBits(resultData);

                return resultBitmap;
            }

        }

        /// <summary>
        /// Adjust the supplied image with the given parameters
        /// Brightness, Contrast, and Gamma
        /// Brightness and Contrast 0f = dark, 1f = 100%
        /// Contrast 0f = Bright, 1f = 100%, > 1f = darker
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="vBrightness"></param>
        /// <param name="vContrast"></param>
        /// <param name="vGamma"></param>
        internal static void AdjustImage(ref Bitmap originalImage, double vBrightness, double vContrast, float vGamma)
        {
            try
            {
                double brightness = (1f - vBrightness) * -1; //1.0f; // no change in brightness
                double contrast = vContrast; // 1.0f; //2.0f; // twice the contrast
                float gamma = vGamma; // 1.0f; // no change in gamma ... values from 0.1f to 2.2 (or 5.0f for extremes)
                if (gamma <= 0) gamma = 0.001f;

                float adjustedBrightness = (float)(brightness);// - 1.0f);
                // create matrix that will brighten and contrast the image
                float[][] ptsArray ={
                new float[] {(float)(contrast), 0, 0, 0, 0}, // scale red
                new float[] {0, (float)(contrast), 0, 0, 0}, // scale green
                new float[] {0, 0, (float)(contrast), 0, 0}, // scale blue
                new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.ClearColorMatrix();
                imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);

                //create a Bitmap the size of the image provided  
                using (Bitmap bmp = new Bitmap(originalImage.Width, originalImage.Height))
                {

                    //create a graphics object from the image  
                    using (Graphics gfx = Graphics.FromImage(bmp))
                    {
                        gfx.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, imageAttributes);

                    }

                    originalImage.Dispose();
                    originalImage = (Bitmap)bmp.Clone();

                    //bmp.Dispose();

                }

                imageAttributes.Dispose();
                //imageAttributes = null;
            }
            catch (Exception)
            {

                throw;
            }
            

        }


        internal static Bitmap AdjustImage(Bitmap originalImage, double vBrightness, double vContrast, float vGamma)
        {

            using (Bitmap adjustedImage = new Bitmap(originalImage.Width, originalImage.Height))
            {
                double brightness = vBrightness; //1.0f; // no change in brightness
                double contrast = vContrast; // 1.0f; //2.0f; // twice the contrast
                float gamma = vGamma; // 1.0f; // no change in gamma ... values from 0.1f to 2.2 (or 5.0f for extremes)
                if (gamma == 0) gamma = 0.01f;

                float adjustedBrightness = (float)(brightness - 1.0f);
                // create matrix that will brighten and contrast the image
                float[][] ptsArray ={
                    new float[] {(float)(contrast), 0, 0, 0, 0}, // scale red
                    new float[] {0, (float)(contrast), 0, 0, 0}, // scale green
                    new float[] {0, 0, (float)(contrast), 0, 0}, // scale blue
                    new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                    new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.ClearColorMatrix();
                imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);

                using (Graphics NewGraphics = Graphics.FromImage(adjustedImage))
                {
                    NewGraphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, imageAttributes);
                    //if (NewGraphics != null) NewGraphics.Dispose();
                }

                if (imageAttributes != null) imageAttributes.Dispose();

                return adjustedImage;

            }

        }
        

        /// <summary>
        /// Give a Bitmap and bounds, calculates new bounds to maintain the
        /// Bitmap's aspect ratio.
        /// </summary>
        /// <param name="img">The Bitmap to fit</param>
        /// <param name="bounds">The bounding box in which it should fit.  This is modifed with the calculated bounds</param>
        /// <returns>The multiplier (aspect ratio) used to fit the image into the given bounds</returns>
        internal static double MaintainAspectRatio(Bitmap img, ref Rectangle bounds, float vScalePercent, WallpaperModes vMode, int vAdjustX, int vAdjustY)
        {
            // Figure out the ratio
            double ratioX = (double)bounds.Width / (double)img.Width;
            double ratioY = (double)bounds.Height / (double)img.Height;
            // use whichever multiplier is larger
            double ratio = ratioX > ratioY ? ratioX : ratioY; 

            if (
                (ratioY < ratioX && vMode == WallpaperModes.FillHeight)
                || (ratioY > ratioX && vMode == WallpaperModes.FillWidth)
                )
            {
                // get other ratio
                ratio = ratioX < ratioY ? ratioX : ratioY;
            }            

            if (
                vMode == WallpaperModes.Tile
                )
            {
                // dont scale the image to the rectangle
                ratio = 1;
            }

            ratio *= vScalePercent;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(img.Height * ratio);
            int newWidth = Convert.ToInt32(img.Width * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posX = Convert.ToInt32((bounds.Width - (img.Width * ratio)) / 2) + bounds.X;
            int posY = Convert.ToInt32((bounds.Height - (img.Height * ratio)) / 2) + bounds.Y;

            // user adjust / offset
            posX += vAdjustX;
            posY += vAdjustY;

            bounds = new Rectangle(posX, posY, newWidth, newHeight);

            return ratio;
        }

        internal static double MaintainAspectRatioRectangle(Bitmap img, ref Rectangle bounds, float vShrinkPercent, float vScale)
        {
            int x = 0, y = 0;
            float newRatio, widthRatio, heightRatio;

            widthRatio = ((float)bounds.Width * vScale) / ((float)img.Width) * vShrinkPercent;
            heightRatio = ((float)bounds.Height * vScale) / ((float)img.Height) * vShrinkPercent;

            //widthRatio = (float)Math.Round(widthRatio, 2);
            //heightRatio = (float)Math.Round(heightRatio, 2);

            if ((heightRatio < widthRatio))
            {
                // portrait
                newRatio = heightRatio;
                x = (short)((((bounds.Width * vScale) - (img.Width * newRatio * vShrinkPercent)) / 2f)); // * vScale);
            }
            else
            {
                //Landscape
                newRatio = widthRatio;
                y = (int)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)); // * vScale);

                //y = (short)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)); // * vScale);
            }

            int newWidth = (int)(img.Width * newRatio * vShrinkPercent);
            int newHeight = (int)(img.Height * newRatio * vShrinkPercent);

            bounds.X += x;// -(int)((bounds.Width - newWidth) / 2);
            bounds.Y += y;// -(int)((bounds.Height - newHeight) / 2);

            if ((heightRatio < widthRatio))
            {
                //bounds.X += (bounds.Width - newWidth) / 2;
                bounds.Y += ((int)(bounds.Height * vScale) - newHeight) / 2;
            }
            else
            {
                bounds.X += ((int)(bounds.Width * vScale) - newWidth) / 2;
            }

            bounds.Width = newWidth;
            bounds.Height = newHeight;

            return newRatio;
        }

        internal static double MaintainAspectRatio(Bitmap img, ref Rectangle bounds, float vShrinkPercent, float vScale, WallpaperModes vMode)
        {
            int x = 0, y = 0;
            float newRatio, widthRatio, heightRatio;

            widthRatio = ((float)bounds.Width * vScale) / ((float)img.Width) * vShrinkPercent;
            heightRatio = ((float)bounds.Height * vScale) / ((float)img.Height) * vShrinkPercent;

            if ((heightRatio >= widthRatio))
            {
                // portrait
                newRatio = heightRatio;
                x = (short)((((bounds.Width * vScale) - (img.Width * newRatio * vShrinkPercent)) / 2f)); // * vScale);

                //y =;
            }
            else
            {
                //Landscape
                newRatio = widthRatio;
                y = (int)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)); // * vScale);
                //x =;

                //y = (short)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)); // * vScale);
            }

            //// alter the values for some modes
            switch (vMode)
            {
                case WallpaperModes.None:
                    break;
                case WallpaperModes.Centre:
                    break;
                case WallpaperModes.FillWidth:
                    newRatio = widthRatio;
                    x = (int)((((bounds.Width * vScale) - (img.Width * newRatio * vShrinkPercent)) / 2f)); // * vScale);
                    //x = (int)((((bounds.Width * vScale) - (img.Width * newRatio * vShrinkPercent)) / 2f)); // * vScale);
                    //y = (int)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)) - (bounds.Height / 2); // * vScale);
                    break;
                case WallpaperModes.FillHeight:
                    newRatio = heightRatio;
                    //x = (short)((((bounds.Width * vScale) - (img.Width * newRatio * vShrinkPercent)) / 2f)); // * vScale);
                    y = (int)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)); // * vScale);
                    break;
                case WallpaperModes.Stretch:
                    break;
                case WallpaperModes.Tile:
                    break;
                case WallpaperModes.Span:
                    break;
                case WallpaperModes.LAWC:
                    break;
                default:
                    break;
            }

            int newWidth = (int)(img.Width * newRatio * vShrinkPercent);
            int newHeight = (int)(img.Height * newRatio * vShrinkPercent);

            bounds.X += x;// -(int)((bounds.Width - newWidth) / 2);
            bounds.Y += y;// -(int)((bounds.Height - newHeight) / 2);

            if ((heightRatio > widthRatio))
            {
                //bounds.X += (bounds.Width - newWidth) / 2;
                bounds.Y += ((int)(bounds.Height * vScale) - newHeight) / 2;
            }
            else
            {
                bounds.X += ((int)(bounds.Width * vScale) - newWidth) / 2;
            }

            bounds.Width = newWidth;
            bounds.Height = newHeight;

            return newRatio;
        }

        internal static double MaintainAspectRatioOLD(Bitmap img, ref Rectangle bounds, float vShrinkPercentBorder, float vScreenScale, WallpaperModes vMode)
        {
            int x = 0, y = 0;
            float newRatio, widthRatio, heightRatio;

            widthRatio = ((float)bounds.Width * vScreenScale) / ((float)img.Width) * vShrinkPercentBorder;
            heightRatio = ((float)bounds.Height * vScreenScale) / ((float)img.Height) * vShrinkPercentBorder;

            //widthRatio = (float)Math.Round(widthRatio, 1);
            //heightRatio = (float)Math.Round(heightRatio, 1);

            if ((heightRatio >= widthRatio))
            {
                // portrait
                newRatio = heightRatio;
                x = (short)((((bounds.Width * vScreenScale) - (img.Width * newRatio * vShrinkPercentBorder)) / 2f)); // * vScale);
                //y =;
            }
            else
            {
                //Landscape
                newRatio = widthRatio;
                y = (int)((((bounds.Height * vScreenScale) - (img.Height * newRatio * vShrinkPercentBorder)) / 2f)); // * vScale);
                //x =;

                //y = (short)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)); // * vScale);
            }

            //// alter the values for some modes
            switch (vMode)
            {
                case WallpaperModes.None:
                    break;
                case WallpaperModes.Centre:
                    break;
                case WallpaperModes.FillWidth:
                    newRatio = widthRatio;
                    x = (int)((((bounds.Width * vScreenScale) - (img.Width * newRatio * vShrinkPercentBorder)) / 2f)); // * vScale);
                    //y = (int)((((bounds.Height * vScale) - (img.Height * newRatio * vShrinkPercent)) / 2f)) - (bounds.Height / 2); // * vScale);
                    break;
                case WallpaperModes.FillHeight:
                    newRatio = heightRatio;
                    //x = (short)((((bounds.Width * vScale) - (img.Width * newRatio * vShrinkPercent)) / 2f)); // * vScale);
                    y = (int)((((bounds.Height * vScreenScale) - (img.Height * newRatio * vShrinkPercentBorder)) / 2f)); // * vScale);
                    break;
                case WallpaperModes.Stretch:
                    break;
                case WallpaperModes.Tile:
                    break;
                case WallpaperModes.Span:
                    break;
                case WallpaperModes.LAWC:
                    break;
                default:
                    break;
            }

            int newWidth = (int)(img.Width * newRatio * vShrinkPercentBorder);
            int newHeight = (int)(img.Height * newRatio * vShrinkPercentBorder);

            bounds.X += x;// -(int)((bounds.Width - newWidth) / 2);
            bounds.Y += y;// -(int)((bounds.Height - newHeight) / 2);

            if ((heightRatio > widthRatio))
            {
                //bounds.X += (bounds.Width - newWidth) / 2;
                bounds.Y += ((int)(bounds.Height * vScreenScale) - newHeight) / 2;
            }
            else
            {
                bounds.X += ((int)(bounds.Width * vScreenScale) - newWidth) / 2;
            }

            bounds.Width = newWidth;
            bounds.Height = newHeight;

            return newRatio;
        }




        static internal void BlurFast(ref Bitmap selectedSource, int radius)
        {

            //selectedSource = Blur(selectedSource, new Rectangle(0, 0, selectedSource.Width, selectedSource.Height), 8);
            FastBlur(ref selectedSource, radius);

        }


        // FROM http://snippetsfor.net/csharp/StackBlur
        internal static void FastBlur(ref Bitmap SourceImage, int radius)
        {
            var rct = new Rectangle(0, 0, SourceImage.Width, SourceImage.Height);
            var dest = new int[rct.Width * rct.Height];
            var source = new int[rct.Width * rct.Height];
            var bits = SourceImage.LockBits(rct, ImageLockMode.ReadWrite, Wallpaper.CurrentPixelFormat); // System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(bits.Scan0, source, 0, source.Length);
            SourceImage.UnlockBits(bits);

            if (radius < 1) return;

            int w = rct.Width;
            int h = rct.Height;
            int wm = w - 1;
            int hm = h - 1;
            int wh = w * h;
            int div = radius + radius + 1;
            var r = new int[wh];
            var g = new int[wh];
            var b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p1, p2, yi;
            var vmin = new int[MainFunctions.max(w, h)];
            var vmax = new int[MainFunctions.max(w, h)];

            var dv = new int[256 * div];
            for (i = 0; i < 256 * div; i++)
            {
                dv[i] = (i / div);
            }

            int yw = yi = 0;

            for (y = 0; y < h; y++)
            { // blur horizontal
                rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    int p = source[yi + MainFunctions.min(wm, MainFunctions.max(i, 0))];
                    rsum += (p & 0xff0000) >> 16;
                    gsum += (p & 0x00ff00) >> 8;
                    bsum += p & 0x0000ff;
                }
                for (x = 0; x < w; x++)
                {

                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];

                    if (y == 0)
                    {
                        vmin[x] = MainFunctions.min(x + radius + 1, wm);
                        vmax[x] = MainFunctions.max(x - radius, 0);
                    }
                    p1 = source[yw + vmin[x]];
                    p2 = source[yw + vmax[x]];

                    rsum += ((p1 & 0xff0000) - (p2 & 0xff0000)) >> 16;
                    gsum += ((p1 & 0x00ff00) - (p2 & 0x00ff00)) >> 8;
                    bsum += (p1 & 0x0000ff) - (p2 & 0x0000ff);
                    yi++;
                }
                yw += w;
            }

            for (x = 0; x < w; x++)
            { // blur vertical
                rsum = gsum = bsum = 0;
                int yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = MainFunctions.max(0, yp) + x;
                    rsum += r[yi];
                    gsum += g[yi];
                    bsum += b[yi];
                    yp += w;
                }
                yi = x;
                for (y = 0; y < h; y++)
                {
                    dest[yi] = (int)(0xff000000u | (uint)(dv[rsum] << 16) | (uint)(dv[gsum] << 8) | (uint)dv[bsum]);
                    if (x == 0)
                    {
                        vmin[y] = MainFunctions.min(y + radius + 1, hm) * w;
                        vmax[y] = MainFunctions.max(y - radius, 0) * w;
                    }
                    p1 = x + vmin[y];
                    p2 = x + vmax[y];

                    rsum += r[p1] - r[p2];
                    gsum += g[p1] - g[p2];
                    bsum += b[p1] - b[p2];

                    yi += w;
                }
            }

            // copy back to image
            var bits2 = SourceImage.LockBits(rct, ImageLockMode.ReadWrite, Wallpaper.CurrentPixelFormat);  //System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(dest, 0, bits2.Scan0, dest.Length);
            SourceImage.UnlockBits(bits);
        }


        internal static void FastBlurTest(ref Bitmap SourceImage, int radius)
        {
            var rct = new Rectangle(0, 0, SourceImage.Width, SourceImage.Height);
            var dest = new int[rct.Width * rct.Height];
            var source = new int[rct.Width * rct.Height];
            var bits = SourceImage.LockBits(rct, ImageLockMode.ReadWrite, Wallpaper.CurrentPixelFormat);  //System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(bits.Scan0, source, 0, source.Length);
            SourceImage.UnlockBits(bits);

            if (radius < 1) return;

            int w = rct.Width;
            int h = rct.Height;
            int wm = w - 1;
            int hm = h - 1;
            int wh = w * h;
            int div = radius + radius + 1;
            var r = new int[wh];
            var g = new int[wh];
            var b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p1, p2, yi;
            var vmin = new int[MainFunctions.max(w, h)];
            var vmax = new int[MainFunctions.max(w, h)];

            var dv = new int[256 * div];
            for (i = 0; i < 256 * div; i++)
            {
                dv[i] = (i / div);
            }

            int yw = yi = 0;

            for (y = 0; y < h; y++)
            { // blur horizontal
                rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    int p = source[yi + MainFunctions.min(wm, MainFunctions.max(i, 0))];
                    rsum += (p & 0xff0000) >> 16;
                    gsum += (p & 0x00ff00) >> 8;
                    bsum += p & 0x0000ff;
                }
                for (x = 0; x < w; x++)
                {

                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];

                    if (y == 0)
                    {
                        vmin[x] = MainFunctions.min(x + radius + 1, wm);
                        vmax[x] = MainFunctions.max(x - radius, 0);
                    }
                    p1 = source[yw + vmin[x]];
                    p2 = source[yw + vmax[x]];

                    rsum += ((p1 & 0xff0000) - (p2 & 0xff0000)) >> 16;
                    gsum += ((p1 & 0x00ff00) - (p2 & 0x00ff00)) >> 8;
                    bsum += (p1 & 0x0000ff) - (p2 & 0x0000ff);

                    yi++;
                }
                yw += w;
            }

            for (x = 0; x < w; x++)
            { // blur vertical
                rsum = gsum = bsum = 0;
                int yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = MainFunctions.max(0, yp) + x;
                    rsum += r[yi];
                    gsum += g[yi];
                    bsum += b[yi];
                    yp += w;
                }
                yi = x;
                for (y = 0; y < h; y++)
                {
                    dest[yi] = (int)(0xff000000u | (uint)(dv[rsum] << 16) | (uint)(dv[gsum] << 8) | (uint)dv[bsum]);
                    if (x == 0)
                    {
                        vmin[y] = MainFunctions.min(y + radius + 1, hm) * w;
                        vmax[y] = MainFunctions.max(y - radius, 0) * w;
                    }
                    p1 = x + vmin[y];
                    p2 = x + vmax[y];

                    rsum += r[p1] - r[p2];
                    gsum += g[p1] - g[p2];
                    bsum += b[p1] - b[p2];

                    yi += w;
                }
            }

            // copy back to image
            var bits2 = SourceImage.LockBits(rct, ImageLockMode.ReadWrite, Wallpaper.CurrentPixelFormat);  //System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(dest, 0, bits2.Scan0, dest.Length);
            SourceImage.UnlockBits(bits);
        }


        // SLOW:
        internal static Bitmap BlurTest(Bitmap vBmp)
        {

            LockBitmap lockBitmap = new LockBitmap(vBmp);
            lockBitmap.LockBits();

            System.Drawing.Color pixel;

            //Gauss template
            int[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };

            //int[] Gauss = { 1, 4, 7, 4, 1, 4, 16, 26, 16, 4, 7, 26, 41, 26, 7, 4, 16, 26, 16, 4, 4, 16, 26, 16, 4, 1, 4, 7, 4, 1 };
            for (int x = 1; x < lockBitmap.Width - 1; x++)
                for (int y = 1; y < lockBitmap.Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = lockBitmap.GetPixel(x + row, y + col);
                            r += pixel.R * Gauss[Index];
                            g += pixel.G * Gauss[Index];
                            b += pixel.B * Gauss[Index];
                            Index++;
                        }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //Color value overflow
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    lockBitmap.SetPixel(x - 1, y - 1, System.Drawing.Color.FromArgb(r, g, b));
                }

            lockBitmap.UnlockBits();

            return lockBitmap.source;

        }


        static internal Bitmap Superimpose(Bitmap largeBmp, Bitmap smallBmp)
        {
            using (Graphics g = Graphics.FromImage(largeBmp))
            {
                g.CompositingMode = CompositingMode.SourceOver;
                smallBmp.MakeTransparent();
                int margin = 5;
                int x = largeBmp.Width - smallBmp.Width - margin;
                int y = largeBmp.Height - smallBmp.Height - margin;
                g.DrawImage(smallBmp, new Point(x, y));
                return largeBmp;
            }

        }


        internal static Bitmap ChangeColor(Bitmap bmp)
        {
            LockBitmap lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();

            System.Drawing.Color compareClr = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    if (lockBitmap.GetPixel(x, y) == compareClr)
                    {
                        lockBitmap.SetPixel(x, y, System.Drawing.Color.Red);
                    }
                }
            }
            lockBitmap.UnlockBits();

            return bmp;

        }



    }



}
