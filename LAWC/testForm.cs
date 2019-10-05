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
    public partial class testForm : Form
    {
        public testForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int g = Screen.AllScreens.Count();

            //Result CORRECT (Screen 1=125%; Screen 2=150%; Both 1920x1080): 
            //{Bounds = {{X=0,Y=0,Width=1920,Height=1080}} WorkingArea = {{X=0,Y=0,Width=0,Height=0}} Primary = true DeviceName = "\\\\.\\DISPLAY1"}
            //{Bounds = {{X=1920,Y=0,Width=1920,Height=1080}} WorkingArea = {{X=0,Y=0,Width=0,Height=0}} Primary = false DeviceName = "\\\\.\\DISPLAY2"}
        }
    }
}
