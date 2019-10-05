using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LAWC
{
    public partial class frmAbout : Form
    {

        private readonly FrmMain parentForm;

        public frmAbout(FrmMain vParentForm)
        {
            InitializeComponent();

            parentForm = vParentForm;
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            getAppInfo();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmMain.OpenAppURL();
        }

        private void lblDonate2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmMain.OpenDonateURL();
        }

        private void lblDonate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmMain.OpenDonateURL();
        }


        private void getAppInfo()
        {

            string company = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
            //string Title = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false)).Title;

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);


            //this.lblProductName.Text = Title; //this.GetType().Assembly.GetName().FullName.ToString(CultureInfo.InvariantCulture);  //this.GetType().Assembly.GetCustom.GetName().Name.ToString(CultureInfo.InvariantCulture);
            this.lblVersion.Text = "Version " + this.GetType().Assembly.GetName().Version.ToString();
            this.llCompany.Text = company; //"Strangetimez";
            this.lblCopyright.Text = versionInfo.LegalCopyright.ToString(CultureInfo.InvariantCulture);

            var descriptionAttribute = this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                .OfType<AssemblyDescriptionAttribute>()
                .FirstOrDefault();

            if (descriptionAttribute != null) this.txtDescription.Text = descriptionAttribute.Description;

            richTextBox2.Text = descriptionAttribute.Description;

            richTextBox1.Text = parentForm.GetLatestChanges();

            //var copmpanyAttribute = this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
            //                .OfType<AssemblyCompanyAttribute>()
            //                .FirstOrDefault();

            //if (copmpanyAttribute != null) this.lblCompany.Text = copmpanyAttribute.Company;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }

        private void btnThanks_Click(object sender, EventArgs e)
        {
            //frmThanks frmThanks = new frmThanks();

            parentForm.frmThanks.ShowDialog();

            //frmThanks.Dispose();
        }

        private void llCompany_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmMain.OpenAppURL();
        }

        private void llAds1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmMain.OpenAppURL();
        }

        private void llAds2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmMain.OpenAppURL();
        }

        private void pbDonate_Click(object sender, EventArgs e)
        {
            FrmMain.OpenDonateURL();
        }



    }
}
