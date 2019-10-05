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
    public partial class frmThanks : Form
    {
        public frmThanks()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://openhardwaremonitor.org");
        }

        private void llSimpleWizard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://stackoverflow.com");
        }

        private void llCodeProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://codeproject.com");
        }

        private void llOpenWeathermap_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://openweathermap.org/");
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/drewnoakes/metadata-extractor");
        }

        private void llOpenListView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://objectlistview.sourceforge.net/cs/index.html");
        }

        private void llMultiWall_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://channel9.msdn.com/coding4fun/articles/MultiWall-Wallpaper-Tool-for-Multiple-Monitors");
        }

        private void llMadeBitsHSV_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://madebits.github.io/#r/msnet-colormatrix-hue-saturation.md");
        }

        private void llCustomScrollBar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.codeproject.com/Articles/41869/Custom-Drawn-Scrollbar");
        }

        private void llVSCommunity_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://visualstudio.microsoft.com/vs/community/"); 
        }

        private void llInno_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.jrsoftware.org/isinfo.php"); 
        }
    }
}
