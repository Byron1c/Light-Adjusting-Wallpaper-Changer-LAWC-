using LAWC.Common;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using LAWC.Objects;

namespace LAWC
{
    public partial class frmRenameImage : Form
    {
        internal Boolean OK = false;
        private readonly FrmMain parentForm = null;
        internal String fullPathCurrent = string.Empty;

        // ==================================================
        private static readonly Size MaxResolution = GetMaxResolution();
        private static double AspectRatio = (double)MaxResolution.Width / MaxResolution.Height;
        private readonly PointF Dpi;

        Boolean cancelled = false;

        // Aspect Ratio Stuff==================================
        //double so division keeps decimal points
        //const double widthRatio = 6;
        //const double heightRatio = 4;

        //const int WM_SIZING = 0x214;
        //const int WMSZ_LEFT = 1;
        //const int WMSZ_RIGHT = 2;
        //const int WMSZ_TOP = 3;
        //const int WMSZ_BOTTOM = 6;

        internal struct RECT : IEquatable<RECT>
        {
            //internal int Left;
            //internal int Top;
            //internal int Right;
            //internal int Bottom;

            public override bool Equals(object obj)
            {
                throw new NotImplementedException();
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }

            public static bool operator ==(RECT left, RECT right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(RECT left, RECT right)
            {
                return !(left == right);
            }

            public bool Equals(RECT other)
            {
                throw new NotImplementedException();
            }
        }


        public frmRenameImage(FrmMain vParentForm, String vFullPathCurrent)
        {
            InitializeComponent();

            parentForm = vParentForm;
            lblCurrentFile.Text = vFullPathCurrent;
            fullPathCurrent = vFullPathCurrent;

            cancelled = false;

            Dpi = GetDpi();
            AspectRatio *= Dpi.X / Dpi.Y;
            //Apply current aspect ratio, using width as the anchor
            //this.Height = (int)(heightRatio * this.Width / widthRatio);

            this.Resize += FrmRenameImage_Resize;
            this.FormClosing += FrmRenameImage_FormClosing;

            txtNewFilename.KeyPress += TxtNewFilename_KeyPress;
        }

        private void frmRenameImage_Load(object sender, EventArgs e)
        {
            load();
        }

        internal void load()
        {
            setWallpaperPreview();
            
            string ext = Path.GetExtension(fullPathCurrent);
            lblExtension.Text = ext;

            if (!string.IsNullOrEmpty(ext) )
            {
                txtNewFilename.Text = System.IO.Path.GetFileName(fullPathCurrent).Replace(ext, string.Empty);
            } else
            {
                txtNewFilename.Text = System.IO.Path.GetFileName(fullPathCurrent);
            }
            txtNewFilename.Focus();
            txtNewFilename.SelectionStart = 0;
            txtNewFilename.SelectionLength = txtNewFilename.Text.Length;// - ext.Length;

            setFormChangedButtons(false);

        }

        private void FrmRenameImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancelled == false) finished();
        }

        private void TxtNewFilename_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if there is text present
            setFormChangedButtons((txtNewFilename.Text.Trim().Length > 0));
        }

        private void FrmRenameImage_Resize(object sender, EventArgs e)
        {
            saveSizeLocation();
        }

        internal void saveSizeLocation()
        {
            Properties.Settings.Default.SizeRename = Size;
            Properties.Settings.Default.LocationRename = Location;
            Properties.Settings.Default.Save();
        }

        internal void SetInterfaceColour()
        {
            this.BackColor = parentForm.colourDarker;
            this.ForeColor = parentForm.colourLightest;

            foreach (Control c in this.Controls)
            {
                c.BackColor = parentForm.colourDarkest;
                c.ForeColor = parentForm.colourLightest;
            }
        }

        internal void loadSizeLocation()
        {
            Size = Properties.Settings.Default.SizeRename;
            //Location = Properties.Settings.Default.LocationRename;
            Properties.Settings.Default.Save();
        }

        private PointF GetDpi()
        {
            PointF dpi = PointF.Empty;
            using (Graphics g = CreateGraphics())
            {
                dpi.X = g.DpiX;
                dpi.Y = g.DpiY;
            }
            return dpi;
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelled = true;
            this.Hide();
        } 

        private void btnSave_Click(object sender, EventArgs e)
        {   
            doCamelCase();

            String newFileName = txtNewFilename.Text.Trim() + lblExtension.Text;

            // if the filename has changed
            // if (fullPathCurrent.ToUpperInvariant().Contains(newFileName.ToUpperInvariant()) == false)
            if (txtNewFilename.Text.Trim().Length != System.IO.Path.GetFileNameWithoutExtension(fullPathCurrent).Length)
            {

                // Validate the filename                
                if (newFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    MessageBox.Show("Sorry, you have entered invalid characters for the filename.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(fullPathCurrent) + "\\" + newFileName))
                {
                    MessageBox.Show("Sorry, this filename already exists.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (fullPathCurrent.Contains(newFileName) == false)
                {
                    doRename(false, false);
                }
            }
            getNextImage();
            setWallpaperPreview();
        }

        private void setFormChangedButtons(Boolean vChanged)
        {
            if (vChanged)
            {
                btnLast.Text = "<< Save";
                btnOkay.Text = "Save >>";
            }
            else
            {
                btnLast.Text = "<< Last";
                btnOkay.Text = "Next >>";
            }
        }

        private void doRename(Boolean vScrollToEntry, Boolean vSelectEntry)
        {
            OK = true;
            string newFile = txtNewFilename.Text.Trim() + lblExtension.Text;

            //string pathNew;// = string.Empty;

            // invalid chars are removed as the user types
            if (string.IsNullOrEmpty(newFile))
            {
                MessageBox.Show("You cannot rename a file to be empty.");
                return;
            }

            //pathNew = System.IO.Path.GetDirectoryName(fullPathCurrent);
            //System.IO.Path.GetDirectoryName(fullPathCurrent);

            String destination = parentForm.RenameWallpaperFile(fullPathCurrent, newFile);
            if (string.IsNullOrEmpty(destination))
            {
                MessageBox.Show("Sorry, there was a problem renaming the filename. Please rescan the folder contents and try again.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            parentForm.settings.ImageLastSelected = destination;
            if (vScrollToEntry)
                parentForm.ScrollToSelectedImageItem(destination, vScrollToEntry, vSelectEntry);
            fullPathCurrent = destination; // update the forms record of the filename too, so we can use it to select the next item on the list
            
        }

        //https://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
        private String RemoveInvalidCharacters(String vIllegal)
        {
            String output = vIllegal;
            //string vIllegal = "\"M\"\\a/ry/ h**ad:>> a\\/:*?\"| li*tt|le|| la\"mb.?";
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            
            foreach (char c in invalid)
            {
                output = output.Replace(c.ToString(CultureInfo.InvariantCulture), "");
            }

            return output;

        }

        private void getNextImageBAK()
        {
            int index = parentForm.settings.Images.FindIndex(r => r.FullPath == fullPathCurrent);
            index++; // go to the next entry
            if (index >= parentForm.settings.Images.Count) index = 0; // go to the start if needed 

            // set this form with the new information
            lblCurrentFile.Text = parentForm.settings.Images[index].FullPath;
            fullPathCurrent = parentForm.settings.Images[index].FullPath;

            load();

            parentForm.SaveSettings(string.Empty, this.parentForm.settings);
            //parentForm.ScrollToSelectedImageItem(index, true);
        }
        private void getLastImageBAK()
        {
            int index = parentForm.settings.Images.FindIndex(r => r.FullPath == fullPathCurrent);
            index--; // go to the LAST entry
            if (index < 0) index = parentForm.settings.Images.Count; // go to the start if needed 

            // set this form with the new information
            lblCurrentFile.Text = parentForm.settings.Images[index].FullPath;
            fullPathCurrent = parentForm.settings.Images[index].FullPath;

            load();

            //parentForm.ScrollToSelectedImageItem(index, true);
            parentForm.SaveSettings(string.Empty, this.parentForm.settings);
        }


        private void getNextImage()
        {            
            int index = parentForm.GetImageIndex(fullPathCurrent); //parentForm.settings.Images.FindIndex(r => r.FullPath == fullPathCurrent);
            index++; // go to the next entry
            if (index >= parentForm.olvImages.Items.Count) index = 0; // go to the start if needed 
            parentForm.olvImages.SelectedIndex = index;

            // set this form with the new information
            lblCurrentFile.Text = ((ImageInfo)parentForm.olvImages.SelectedItem.RowObject).Filename;//parentForm.settings.Images[index].FullPath;
            fullPathCurrent = ((ImageInfo)parentForm.olvImages.SelectedItem.RowObject).FullPath; // parentForm.settings.Images[index].FullPath;

            parentForm.ScrollToSelectedImageItem(fullPathCurrent, true, true);

            load();

            parentForm.SaveSettings(string.Empty, this.parentForm.settings);
            //parentForm.ScrollToSelectedImageItem(index, true);
        }
        private void getLastImage()
        {
            int index = parentForm.GetImageIndex(fullPathCurrent); //parentForm.settings.Images.FindIndex(r => r.FullPath == fullPathCurrent);
            index--; // go to the LAST entry
            if (index < 0) index = parentForm.olvImages.Items.Count - 1; // go to the start if needed 
            parentForm.olvImages.SelectedIndex = index;

            // set this form with the new information
            lblCurrentFile.Text = ((ImageInfo)parentForm.olvImages.SelectedItem.RowObject).Filename;//parentForm.settings.Images[index].FullPath;
            fullPathCurrent = ((ImageInfo)parentForm.olvImages.SelectedItem.RowObject).FullPath; // parentForm.settings.Images[index].FullPath;

            parentForm.ScrollToSelectedImageItem(fullPathCurrent, true, true);

            load();

            parentForm.SaveSettings(string.Empty, this.parentForm.settings);
            //parentForm.ScrollToSelectedImageItem(index, true);
        }

        private void setWallpaperPreview()
        {
            Bitmap preview;
            preview = (Bitmap)ImageFunctions.LoadImage(fullPathCurrent, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            if (preview != null)
            {
                this.pbPreviewImage.Image.Dispose();
                this.pbPreviewImage.Image = (Image)preview.Clone();
                preview.Dispose();
            }
        }

        private void btnFinished_Click(object sender, EventArgs e)
        {
            if (cancelled == false) finished();
        }

        private void finished()
        {
            //doRename(true,true);
            doCamelCase();
            String newFileName = txtNewFilename.Text.Trim() + lblExtension.Text;

            // if the filename has changed
            //if (fullPathCurrent.ToUpperInvariant().Contains(newFileName.ToUpperInvariant()) == false)
            //if (fullPathCurrent.Contains(newFileName) == false)
            if (txtNewFilename.Text.Trim().Length != System.IO.Path.GetFileNameWithoutExtension(fullPathCurrent).Length)
            {
                doRename(true, true);
            }

            parentForm.redrawLists();

            //int index = parentForm.settings.Images.FindIndex(r => r.FullPath == fullPathCurrent);
            parentForm.ScrollToSelectedImageItem(fullPathCurrent, true, true);
            parentForm.SelectCurrentWallpaper(true);

            this.Hide();
        }

        // Aspect Ratio Stuff ========================

        private static Size GetMaxResolution()
        {
            int maxHResolution = 0;
            int maxVResolution = 0;

            foreach (Screen s in Screen.AllScreens)
            {
                int h = s.Bounds.Width; //int.Parse(item["HorizontalResolution"].ToString(CultureInfo.InvariantCulture));
                int v = s.Bounds.Height; //int.Parse(item["VerticalResolution"].ToString(CultureInfo.InvariantCulture));
                if (h > maxHResolution || v > maxVResolution)
                {
                    maxHResolution = h;
                    maxVResolution = v;
                }
            }
            return new Size(maxHResolution, maxVResolution);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (parentForm.DeleteSelectedImage(fullPathCurrent) == true)
            {
                parentForm.redrawLists();
                getNextImage();
                setWallpaperPreview();
            }
        }


        protected override void WndProc(ref Message message)
        {
            //// Aspect Ratio function here
            //if (message.Msg == WM_SIZING)
            //{
            //    int minWidth = Math.Min(Width, (int)(Height * AspectRatio));
            //    if (WindowState == FormWindowState.Normal)
            //        Size = new Size(minWidth, (int)(minWidth / AspectRatio));
            //}

            base.WndProc(ref message);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            // if the filename has changed
            //if (fullPathCurrent.Contains(txtNewFilename.Text.Trim()) == false)
            if (txtNewFilename.Text.Trim().Length != System.IO.Path.GetFileNameWithoutExtension(fullPathCurrent).Length)
            {
                doRename(false, false);
                //int index = parentForm.settings.Images.FindIndex(r => r.FullPath == fullPathCurrent);
                //refresh listview
                //parentForm.olvImages.Invalidate();
                //parentForm.redrawLists();
            }

            getLastImage();
            setWallpaperPreview();
        }

        private void txtNewFilename_TextChanged(object sender, EventArgs e)
        {
            parseFilename();
        }

        private void parseFilename()
        {
            int pos = txtNewFilename.SelectionStart;//MainFunctions.GetCaretPoint(txtNewFilename).X;

            //txtNewFilename.Text = txtNewFilename.Text.Trim();
            txtNewFilename.Text = RemoveInvalidCharacters(txtNewFilename.Text);
            doCamelCase();

            txtNewFilename.SelectionStart = pos;
        }


        private void doCamelCase()
        {
            if (cbTitleCase.Checked)
            {
                string newText = MainFunctions.ToCamelCase(txtNewFilename.Text);

                txtNewFilename.Text = newText;
                
                txtNewFilename.SelectionStart = newText.Length;
                txtNewFilename.SelectionLength = 0;
            }
        }

        private void cbTitleCase_CheckedChanged(object sender, EventArgs e)
        {
            parseFilename();
        }
        //protected override void WndProc(ref Message message)
        //{
        //    //http://www.vcskicks.com/maintain-aspect-ratio.php

        //    //// Aspect Ratio function here
        //    if (message.Msg == WM_SIZING)
        //    {
        //        RECT rc = (RECT)Marshal.PtrToStructure(message.LParam, typeof(RECT));
        //        int res = message.WParam.ToInt32();
        //        if (res == WMSZ_LEFT || res == WMSZ_RIGHT)
        //        {
        //            //Left or right resize -> adjust height (bottom)
        //            rc.Bottom = rc.Top + (int)(heightRatio * this.Width / widthRatio);
        //        }
        //        else if (res == WMSZ_TOP || res == WMSZ_BOTTOM)
        //        {
        //            //Up or down resize -> adjust width (right)
        //            rc.Right = rc.Left + (int)(widthRatio * this.Height / heightRatio);
        //        }
        //        else if (res == WMSZ_RIGHT + WMSZ_BOTTOM)
        //        {
        //            //Lower-right corner resize -> adjust height (could have been width)
        //            rc.Bottom = rc.Top + (int)(heightRatio * this.Width / widthRatio);
        //        }
        //        else if (res == WMSZ_LEFT + WMSZ_TOP)
        //        {
        //            //Upper-left corner -> adjust width (could have been height)
        //            rc.Left = rc.Right - (int)(widthRatio * this.Height / heightRatio);
        //        }
        //        Marshal.StructureToPtr(rc, message.LParam, true);
        //    }

        //    base.WndProc(ref message);
        //}


    }
}
