using System.Collections.Generic;


namespace LAWC.Common
{
    internal static class StringIDs
    {
        internal enum ImageInfoFields
        {
            FullPath,
            AverageColour,
            AverageBrightness,
            FolderID,
            ViewCount,
            SizeBytes,
            Aspect,
            Width,
            Height
        }

        internal static Dictionary<int, string> ImageInfoKeys = new Dictionary<int, string>()
        {
            { (int)ImageInfoFields.FullPath, "FP" },
            { (int)ImageInfoFields.AverageColour, "AC" },
            { (int)ImageInfoFields.AverageBrightness, "AB" },
            { (int)ImageInfoFields.FolderID, "FID" },
            { (int)ImageInfoFields.ViewCount, "VC" },
            { (int)ImageInfoFields.SizeBytes, "SB" },
            { (int)ImageInfoFields.Aspect, "AS" },
            { (int)ImageInfoFields.Width, "W" },
            { (int)ImageInfoFields.Height, "H" },
                    };



        //    dtItems.Columns.Add("FullPath", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("AverageColour", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("AverageBrightness", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("FolderID", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("ViewCount", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("SizeBytes", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("Aspect", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("Width", System.Type.GetType("System.String"));
        //    dtItems.Columns.Add("Height", System.Type.GetType("System.String"));

    }
}
