using System;


namespace LAWC.Objects
{
    class WebsiteInfo : IComparable
    {

        private String url;
        public string URL
        {
            get { return url; }
            set { url = value; }
        }


        private String name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        private DateTime lastVisited;
        public DateTime LastVisited
        {
            get { return lastVisited; }
            set { lastVisited = value; }
        }


        public String LastVisit
        {
            get
            {
                if (LastVisited > new DateTime(1980, 1, 1, 12,0,0))
                {
                    return LastVisited.ToShortDateString();
                } else
                {
                    return "Never";
                }
            }
            
        }

        

        private Boolean done;
        public Boolean Done
        {
            get { return done; }
            set { done = value; }
        }
        

        public WebsiteInfo()
        {

            URL = "";
            LastVisited = new DateTime(1980, 1, 1);
            Done = true;
            Name = "";
        }


        public int CompareTo(object obj)
        {
            WebsiteInfo otherInfo = (WebsiteInfo)obj;
            return String.Compare(this.URL, otherInfo.URL, StringComparison.InvariantCulture);
            //return this.URL.CompareTo(otherInfo.URL);
        }


        public static int CompareByURL(WebsiteInfo x, WebsiteInfo y)
        {
            return String.Compare(x.URL, y.URL, StringComparison.InvariantCulture);
            //return x.URL.CompareTo(y.URL);
        }


        public static int CompareByName(WebsiteInfo x, WebsiteInfo y)
        {
            return String.Compare(x.Name, y.Name, StringComparison.InvariantCulture);
            //return x.Name.CompareTo(y.Name);
        }

    }
}
