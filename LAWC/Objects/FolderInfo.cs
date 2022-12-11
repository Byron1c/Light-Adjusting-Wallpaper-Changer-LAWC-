using LAWC.Common;
using System;

namespace LAWC.Objects
{
    class FolderInfo : IComparable
    {

        private String path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private String pathDisplay;
        public string PathDisplay
        {
            get { return pathDisplay; }
            set { pathDisplay = value; }
        }

        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private Boolean enabled;
        public Boolean Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        private int imageCount;
        public int ImageCount
        {
            get { return imageCount; }
            //set { imageCount = value; }
        }

        public FolderInfo()
        {
            Path = "";
            PathDisplay = "";
            iD = -1;
            enabled = true;
            imageCount = 0;

        }

        public void setImageCount(Setting vSettings)
        {
            imageCount = vSettings.GetImageCount(ID);
        }

        public int CompareTo(object obj)
        {
            FolderInfo otherInfo = (FolderInfo)obj;
            return this.ID.CompareTo(otherInfo.ID);
        }

        public static int CompareByPath(FolderInfo x, FolderInfo y)
        {
            return String.Compare(x.Path, y.Path, StringComparison.InvariantCulture);
            //return x.Path.CompareTo(y.Path);
        }

    }
}
