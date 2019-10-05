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
    public partial class frmScreenInfoEdit : Form
    {
        internal Boolean OK = false;

        public frmScreenInfoEdit(int vScalePercent)
        {
            InitializeComponent();

            numScale.Value = vScalePercent;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            OK = true;
            this.Hide();
        }
    }
}
