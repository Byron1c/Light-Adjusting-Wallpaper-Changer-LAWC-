using System;
using System.Windows.Forms;

namespace LAWC
{
    public partial class frmDebugScreen : Form
    {
        public frmDebugScreen()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
