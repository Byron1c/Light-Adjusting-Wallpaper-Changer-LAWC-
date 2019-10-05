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

namespace LAWC.Wizard
{
    public partial class Page1 : UserControl, IWizardPage
    {
        private readonly FrmMain parentForm;

        public Page1(FrmMain vParentForm)
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
            get { return false; }
            //get { throw new NotImplementedException(); }

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

        private void cbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.cbAutoStart.Checked = cbAutoStart.Checked;
            parentForm.SetStartAutomatically(cbAutoStart.Checked);
        }

        private void numChangeFrequencyMinutes_ValueChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.numWallpaperChangeMins.Value = numChangeFrequencyMinutes.Value;
        }

        private void cbShowToolTips_CheckedChanged(object sender, EventArgs e)
        {
            this.parentForm.frmSettingsAdvanced.cbShowToolTips.Checked = cbShowToolTips.Checked;
        }
    }
}
