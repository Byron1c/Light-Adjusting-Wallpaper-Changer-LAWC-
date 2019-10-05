using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAWC
{

    // TIP FROM: https://stackoverflow.com/questions/4398667/c-sharp-hide-on-startup-without-showintaskbar-to-false

    public partial class frmTaskbarIcon : Form
    {
        private readonly FrmMain parentForm;

        public frmTaskbarIcon(FrmMain vParentForm)
        {
            InitializeComponent();

            parentForm = vParentForm;
        }

        private void frmTaskbarIcon_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
            this.GotFocus += FrmTaskbarIcon_GotFocus;

            this.Location = new Point(99999, 99999); // MOVE the screen WAY off the users desktop(s) so its not visible
        }

        private void FrmTaskbarIcon_GotFocus(object sender, EventArgs e)
        {
            parentForm.Focus();
        }
    }
}
