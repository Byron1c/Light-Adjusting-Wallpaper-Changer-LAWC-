using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace LAWC.Common
{

    public static class FileFunctions
    {
        /// image and video extensions
        internal static ArrayList MediaFileTypes = new ArrayList(
            new object[] { "MPG", "AVI", "MKV", "FLV", "MOV", "MPEG", "RMVB", "MP4", "ASF", "MP3", "MPA", "OGG", "FLAC" }
        );

        // ONLY Image file extensions
        internal static ArrayList ImageFileTypes = new ArrayList(
            new object[] { "BMP", "JPG", "PNG", "JPEG" } //"GIF",
        );


        /// <summary>
        /// Delete a file to the recyclebin with VB.NET
        /// </summary>
        /// <param name="vFilePath"></param>
        internal static void DeleteFileToRecycleBin(String vFilePath)
        {
            FileSystem.DeleteFile(vFilePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }
        
        /// <summary>
        /// duh
        /// </summary>
        /// <param name="filename"></param>
        public static void CreateEmptyFile(string filename)
        {
            if (!File.Exists(filename))
            {
                File.Create(filename).Dispose();
            }
        }
                
        /// <summary>
        /// Open a path/file with the default app (in windows) for that exstension
        /// </summary>
        /// <param name="vPath"></param>
        public static void PlayExternalFile(String vPath)
        {
            try
            {
                if (System.IO.File.Exists(vPath))
                {
                    Process.Start(vPath);
                } else
                {
                    MessageBox.Show("Sorry, this file does not exist (" + vPath + "). \n\nPlease rescan the folder contents.", "File Does Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (FileLoadException ex)
            {
                MessageBox.Show("There was an error opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        /// <summary>
        /// Compares the existing file names' extension is in the supplied array list of file types that ARE allowed
        /// </summary>
        /// <param name="vFile"></param>
        /// <param name="vFileTypesAllowed"></param>
        /// <returns></returns>
        public static Boolean IsFileTypeOK(String vFile, ArrayList vFileTypesAllowed)
        {

            String ext = string.Empty;

            if (vFile.Length > 0)
            {
                ext = System.IO.Path.GetExtension(vFile);

                if (ext.Length > 0)
                {
                    ext = ext.Substring(1);
                }
            }

            if (ext.Length > 0)
            {
                foreach (string fileType in vFileTypesAllowed)
                {
                    //if (string.compare(vFile.ToUpper(), fileType.ToUpper()) > 0)
                    if (ext.ToUpper(CultureInfo.InvariantCulture).Contains(fileType.ToUpper(CultureInfo.InvariantCulture)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Check the file exists (and other things later if necessary)
        /// </summary>
        /// <param name="vPath"></param>
        /// <returns></returns>
        public static Boolean IsFileOK(String vPath)
        {
            Boolean output = true;

            if (!File.Exists(vPath))
            {
                output = false;
            }

            return output;

        }


        public static void GetHDDInfo()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  File type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
        }


    }
}
