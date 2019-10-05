using System;
using System.Collections;
using LAWC.Common;
using System.Drawing;
using System.ComponentModel;

namespace LAWC.Objects
{
    internal class ImageInfo : IComparable, INotifyPropertyChanged
    {

        public string Filename
        {
            get { return System.IO.Path.GetFileName(fullPath); }
        }

        private String fullPath;
        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }

        private System.Drawing.Color averageColour;
        public System.Drawing.Color AverageColour
        {
            get { return averageColour; }
            set { averageColour = value; }
        }

        private float averageBrightness;
        public float AverageBrightness
        {
            get { return averageBrightness; }
            set { averageBrightness = value; }
        }

        private float asapectRatio;
        public float Aspect
        {
            get { return asapectRatio; }
            set { asapectRatio = value; }
        }

        private long sizeBytes;
        public long SizeBytes
        {
            get { return sizeBytes; }
            set { sizeBytes = value; }
        }
        
        private int folderID;
        public int FolderID
        {
            get { return folderID; }
            set { folderID = value; }
        }

        private Int32 viewCount;
        public int ViewCount
        {
            get { return viewCount; }
            set { viewCount = value; }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        
        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }


        public ImageInfo()
        {
            fullPath = "";
            averageColour = System.Drawing.Color.White;
            averageBrightness = 1.0f;
            folderID = -1;
            viewCount = 0;
            sizeBytes = 0;
            width = 0;
            height = 0;
        }

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void OnPropertyChanged(string propertyName)
        {
            //if (this.PropertyChanged != null)
            //    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(object obj)
        {
            ImageInfo otherInfo = (ImageInfo)obj;
            return this.AverageBrightness.CompareTo(otherInfo.AverageBrightness);
        }


        public static int CompareByBrightness(ImageInfo x, ImageInfo y)
        {
            return x.AverageBrightness.CompareTo(y.AverageBrightness);
        }


        public static int CompareByBrightnessDESC(ImageInfo x, ImageInfo y)
        {
            return y.AverageBrightness.CompareTo(x.AverageBrightness);
        }


        public static int CompareByAspectRatio(ImageInfo x, ImageInfo y)
        {
            return x.Aspect.CompareTo(y.Aspect);
        }


        public static int CompareByAspectRatioDESC(ImageInfo x, ImageInfo y)
        {
            return y.Aspect.CompareTo(x.Aspect);
        }


        public static int CompareByViewCount(ImageInfo x, ImageInfo y)
        {
            return x.ViewCount.CompareTo(y.ViewCount);
        }


        public static int CompareByViewCountDESC(ImageInfo x, ImageInfo y)
        {
            return y.ViewCount.CompareTo(x.ViewCount);
        }


        public static int CompareByTitle(ImageInfo x, ImageInfo y)
        {
            string filenameX = System.IO.Path.GetFileName(x.FullPath);
            string filenameY = System.IO.Path.GetFileName(y.FullPath);

            //return filenameX.CompareTo(filenameY);
            return String.Compare(filenameX, filenameY, StringComparison.InvariantCulture);
        }


        public static int CompareByTitleDESC(ImageInfo x, ImageInfo y)
        {
            string filenameX = System.IO.Path.GetFileName(x.FullPath);
            string filenameY = System.IO.Path.GetFileName(y.FullPath);

            //return filenameY.CompareTo(filenameX);
            return String.Compare(filenameY, filenameX, StringComparison.InvariantCulture);
        }


        public int CompareTo(ImageInfo item2, ImageInfoSort.ComparisonType comparisonType)
        {
            int returnValue;

            if (comparisonType == ImageInfoSort.ComparisonType.File)
            {
                if (this.FullPath == item2.FullPath)
                {
                    //returnValue = FullPath.CompareTo(item2.FullPath);
                    returnValue = String.Compare(FullPath, item2.fullPath, StringComparison.InvariantCulture);
                }
                else
                {
                    returnValue = AverageBrightness.CompareTo(item2.AverageBrightness);
                }
            }
            else if (comparisonType == ImageInfoSort.ComparisonType.Brightness)
            {
                returnValue = AverageBrightness.CompareTo(item2.AverageBrightness);
            }
            else
            {
                //returnValue = FullPath.CompareTo(item2.FullPath);
                returnValue = String.Compare(FullPath, item2.fullPath, StringComparison.InvariantCulture);
            }

            return returnValue;

        }
                       

    } // END CLASS


    

    internal class ImageStats
    {
        internal System.Drawing.Color AverageColour;
        internal float Brightness;
        internal float Aspect;
        internal int Width;
        internal int Height;

        internal ImageStats()
        {
        }

        internal ImageStats(Bitmap vBMP)
        {
            Brightness = ImageFunctions.GetAverageBrightness(vBMP);
            AverageColour = ImageFunctions.GetAverageColor(vBMP);
            Aspect = vBMP.Width / vBMP.Height;
            Width = vBMP.Width;
            Height = vBMP.Height;
        }

    }



    




}
