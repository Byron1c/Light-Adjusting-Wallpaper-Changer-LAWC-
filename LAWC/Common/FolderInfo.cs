using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightAdjustingWallpaperChanger.Objects
{
    class FolderInfo : IComparable
    {

        private String path;
        public string Path
        {
            get { return path; }
            set { path = value; }
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



        public FolderInfo()
        {
            Path = "";
            iD = -1;
            enabled = true;
            
        }


        public int CompareTo(object obj)
        {
            FolderInfo otherInfo = (FolderInfo)obj;
            return this.ID.CompareTo(otherInfo.ID);
        }

        public static int CompareByPath(FolderInfo x, FolderInfo y)
        {
            return x.Path.CompareTo(y.Path);
        }

    }
}
