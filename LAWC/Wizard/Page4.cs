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
    public partial class Page4 : UserControl, IWizardPage
    {
        //private readonly FrmMain parentForm;

        //private readonly String imageFolder = string.Empty;

        public Page4()//FrmMain vParentForm)
        {
            InitializeComponent();

            //parentForm = vParentForm;
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



        private void pbDonate_Click(object sender, EventArgs e)
        {
            FrmMain.OpenDonateURL();
        }


    }
}
