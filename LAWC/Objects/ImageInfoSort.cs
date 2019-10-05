
using System.Collections;


namespace LAWC.Objects
{
    internal class ImageInfoSort : IComparer
    {
        public enum ComparisonType
        {
            File, Brightness
        }

        public enum SortDirection
        {
            Asc,
            Desc
        }

        private ComparisonType compMethod;
        public ComparisonType ComparisonMethod
        {
            get { return compMethod; }
            set { compMethod = value; }
        }

        public ImageInfoSort(ComparisonType vType)
        {
            ComparisonMethod = vType;
        }

        public int Compare(object x, object y)
        {
            ImageInfo item1 = (ImageInfo)x;
            ImageInfo item2 = (ImageInfo)y;

            return item1.CompareTo(item2);//, ComparisonMethod, item1);

        }

    }
}
