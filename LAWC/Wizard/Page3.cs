using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MBG.SimpleWizard;
using LAWC;
using System.Globalization;

namespace LAWC.Wizard
{
    public partial class Page3 : UserControl, IWizardPage
    {
        private readonly FrmMain parentForm;

        //private readonly String imageFolder = string.Empty;

        public Page3(FrmMain vParentForm)
        {
            InitializeComponent();

            parentForm = vParentForm;
        }

        #region IWizardPage Members

        public UserControl Content
        {
            get { return this; }
        }

        public new void Load()
        {

        }

        public void Save()
        {
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public bool IsBusy
        {
            //get { throw new NotImplementedException(); }
            get { return false; }
            //
        }

        public bool PageValid
        {
            get { return true; }
        }

        public string ValidationMessage
        {
            //get { throw new NotImplementedException(); }
            get { return ""; }
            //
        }

        #endregion

        


        private void button1_Click_1(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer //Environment.SpecialFolder.MyPictures;
            };
            if (!string.IsNullOrEmpty(parentForm.settings.LastFolderPath)) dialog.SelectedPath = parentForm.settings.LastFolderPath;
            DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.parentForm.wizardInitialAddFolder = dialog.SelectedPath;
                parentForm.settings.LastFolderPath = dialog.SelectedPath;
                MessageBox.Show("Your folder (" + parentForm.wizardInitialAddFolder + ") will be scanned and added.  See the bottom left of the main LAWC screen to see the progress bar when the Wizard is completed.", "Folder Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.parentForm.wizardInitialAddFolder = string.Empty;
            }
        }

        private void cbResetImageOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbResetImageOptions.SelectedItem.ToString().Equals("Please Choose (Optional)", StringComparison.InvariantCulture))
            {
                return; // do nothing as they cancelled
            }

            //            DialogResult result = MessageBox.Show("Are you sure you want to reset the Image Options (to the right)?\n\nNote: You can still adjust the settings once they have been reset.", "Reset Image Options", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

            //            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (cbResetImageOptions.SelectedItem.ToString().Equals("None", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 0;
                    parentForm.settings.Brightness = 0;
                    parentForm.settings.Gamma = 100;
                    parentForm.settings.Alpha = 0;
                    parentForm.settings.Contrast = 0;
                    parentForm.settings.ImageAdjustmentName = "None";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("A Little Darker", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 2;
                    parentForm.settings.Brightness = 5;
                    parentForm.settings.Contrast = 5;
                    parentForm.settings.Alpha = 9;
                    parentForm.settings.Gamma = 125;
                    parentForm.settings.ImageAdjustmentName = "A Little Darker";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("Darker", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 5;
                    parentForm.settings.Brightness = 10;
                    parentForm.settings.Contrast = 10;
                    parentForm.settings.Alpha = 16;
                    parentForm.settings.Gamma = 150;
                    parentForm.settings.ImageAdjustmentName = "Darker";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("Much Darker", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 8;
                    parentForm.settings.Brightness = 15;
                    parentForm.settings.Contrast = 15;
                    parentForm.settings.Gamma = 200;
                    parentForm.settings.Alpha = 23;
                    parentForm.settings.ImageAdjustmentName = "Much Darker";
                }

                if (cbResetImageOptions.SelectedItem.ToString().Equals("Very Dark", StringComparison.InvariantCulture))
                {
                    parentForm.settings.TintStrength = 10;
                    parentForm.settings.Brightness = 20;
                    parentForm.settings.Contrast = 20;
                    parentForm.settings.Gamma = 250;
                    parentForm.settings.Alpha = 30;
                    parentForm.settings.ImageAdjustmentName = "Very Dark";
                }

            }

            //reset it to the first item, no matter what was selected
            //cbResetImageOptions.SelectedItem = "Please Choose (Optional)";
        }
    }
}
