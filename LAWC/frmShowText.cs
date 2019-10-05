using LAWC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LAWC.Common.ErrorHandling;

namespace LAWC
{
    public partial class frmShowText : Form
    {
        private readonly FrmMain parentForm;// = null;

        public frmShowText()
        {
            InitializeComponent();
        }

        public frmShowText(FrmMain vParentForm)
        {
            InitializeComponent();

            parentForm = vParentForm;
            setColour(vParentForm);
        }

        internal void setColour(FrmMain vParentForm)
        {
            this.BackColor = vParentForm.colourDarkest;
            this.ForeColor = vParentForm.colourLightest;
            this.richText.BackColor = vParentForm.colourDarkest;
            this.richText.ForeColor = vParentForm.colourLightest;
            this.btnCancel.BackColor = vParentForm.colourDarkest;
            this.btnCancel.ForeColor = vParentForm.colourLightest;
            this.btnOK.BackColor = vParentForm.colourDarkest;
            this.btnOK.ForeColor = vParentForm.colourLightest;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //this.Hide();
        }

        private void btnSendError_Click(object sender, EventArgs e)
        {
            btnSendError.Text = "Sending...";
            btnSendError.Enabled = false;
            lblWorking.Visible = true;
            lblWorking.Refresh();

            SendSMTPMail(Constants.ErrorFromEmail, Constants.ErrorToEmail, "LAWC Error Received!", this.richText.Text, Setting.getSettingsFullPath(Properties.Settings.Default.Portable), MailPriority.High, Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, Setting.getSettingsFullPath(Properties.Settings.Default.Portable))); 

            btnSendError.Text = "Send Error";
            btnSendError.Enabled = true;
            lblWorking.Visible = false;
            lblWorking.Refresh();

            MessageBox.Show("Thank you for sending the error information.", "Information Sent", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //this.Hide();
        }

        private void frmShowText_Load(object sender, EventArgs e)
        {

        }
    }
}
