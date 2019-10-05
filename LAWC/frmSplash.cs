using LAWC.Common;
using System;
using System.Windows.Forms;
using static LAWC.NativeMethods;

namespace LAWC
{
    public partial class frmSplash : Form
    {

        internal Boolean cancel = false;
        internal Boolean darkMode = false;
        private FrmMain parentForm;

        public frmSplash(FrmMain vParentForm)
        {
            InitializeComponent();
            parentForm = vParentForm;
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            if (parentForm.debugEnabled) ErrorHandling.WriteText("DEBUG: Loading splash screen", string.Empty);

            darkMode = ScreenFunctions.isWinDarkModeEnabled();

            if (darkMode)
            {
                this.BackColor = System.Drawing.Color.FromArgb(255, 15, 15, 15); // Darkest
                this.ForeColor = System.Drawing.Color.FromArgb(255, 250, 250, 250);
                this.Refresh();
                lblVersion.BackColor = this.BackColor;
                lblVersion.ForeColor = this.ForeColor;
                lblTitle.BackColor = this.BackColor;
                lblTitle.ForeColor = this.ForeColor;
                lblCurrentStep.BackColor = this.BackColor;
                lblCurrentStep.ForeColor = this.ForeColor;
                lblCurrentStep.BackColor = this.BackColor;
                lblCurrentStep.ForeColor = this.ForeColor;
            }

            this.pnlCancel.Click += PnlCancel_Click;
            this.lblCancel.Click += LblCancel_Click;
            pnlCancel.Cursor = Cursors.Hand;
            lblCancel.Cursor = Cursors.Hand;
            this.KeyPress += FrmSplash_KeyPress;
            this.KeyDown += FrmSplash_KeyDown;

            //moveable
            this.MouseDown += FrmSplash_MouseDown1;

            if (parentForm.debugEnabled) ErrorHandling.WriteText("DEBUG: Loading splash screen Finished", string.Empty);
        }

        private void FrmSplash_MouseDown1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessageW(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, "0");
            }
        }

        private void FrmSplash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode  == Keys.Escape)
            {
                this.cancel = true;
            }
        }

        internal void refresh()
        {
            this.Refresh();
        }

        private void FrmSplash_KeyPress(object sender, KeyPressEventArgs e)
        {
            //int g = e.KeyChar;
            
            if (e.KeyChar== 27) // ESCAPE
            {
                this.cancel = true;
            }
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }

        private void PnlCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }


        private void lblCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }

        
    }
}
