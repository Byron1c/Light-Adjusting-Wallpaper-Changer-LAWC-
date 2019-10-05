using LAWC.Objects;
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
    public partial class frmWebsiteEdit : Form
    {


        internal EditMode FormMode = EditMode.Add;
        private readonly FrmMain parentForm;
        internal int Editndex;


        public frmWebsiteEdit(FrmMain vParent)
        {
            InitializeComponent();

            parentForm = vParent;
        }

        internal enum EditMode
        {
            Add,
            Edit
        }
        

        private void frmWebsiteEdit_Load(object sender, EventArgs e)
        {
            //FormMode = EditMode.Add;
            //Editndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (FormMode == EditMode.Add)
            {
                Add();
                this.Close();
            }
            else if (FormMode == EditMode.Edit)
            {
                Save();
                this.Close();
            }

        }

        private void Save()
        {
            parentForm.settings.Websites[Editndex].Name = txtName.Text;
            parentForm.settings.Websites[Editndex].URL = txtURL.Text;
            // do nothing with this one: parentForm.settings.Websites[Editndex].LastVisited 
            parentForm.settings.Websites[Editndex].Done = cbDone.Checked;
        }


        private void Add()
        {
            WebsiteInfo newInfo = new WebsiteInfo
            {
                Name = txtName.Text,
                URL = txtURL.Text,
                LastVisited = DateTime.Now,
                Done = cbDone.Checked
            };

            parentForm.settings.Websites.Add(newInfo);

        }







    }
}
