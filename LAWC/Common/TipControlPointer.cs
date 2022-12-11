using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using LAWC.Objects;

namespace LAWC.Common
{
    public partial class TipControlPointer : Panel
    {
        //https://web.archive.org/web/20111110163706/http://www.switchonthecode.com/tutorials/csharp-creating-rounded-rectangles-using-a-graphics-path
        public enum RectangleCorners
        {
            None = 0, TopLeft = 1, TopRight = 2, BottomLeft = 4, BottomRight = 8,
            All = TopLeft | TopRight | BottomLeft | BottomRight
        }

        public RectangleCorners Corners = RectangleCorners.All;
        //public String Text = string.Empty;
        public int thickness = 5;//it's up to you
        public short Alpha = 128;
        public Color BorderColour = Color.Red;
        public Color FillColour = Color.Salmon;
        public int Round = 15;

        private const int WS_EX_TRANSPARENT = 0x20;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }


        public TipControlPointer()
        {
            InitializeComponent();

            //SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);

            //this.ResizeRedraw = true;
            //this.DoubleBuffered = true;
            
            this.BringToFront();

            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //make background 100% transparent 
            using (var brush = new SolidBrush(Color.FromArgb(0, this.BackColor)))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            //base.OnPaint(e);



            //BackColor = Color.FromArgb(0, Color.Black);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int halfThickness = thickness / 2;

            GraphicsPath path;// = RoundedRectangle.Create(5, 5, 20, 20);
            //e.Graphics.DrawPath(Pens.Black, path);

            //path = RoundedRectangle.Create(30, 5, 40, 40, 5);
            //e.Graphics.FillPath(Brushes.Blue, path);

            path = RoundedRectangle.Create(halfThickness, halfThickness, this.Size.Width - thickness, this.Size.Height - thickness, Round);
            e.Graphics.DrawPath(new Pen(new SolidBrush(Color.FromArgb(Alpha, BorderColour.R, BorderColour.G, BorderColour.B)), thickness), path);

            e.Graphics.FillPath(new SolidBrush(Color.FromArgb(Alpha, FillColour.R, FillColour.G, FillColour.B)), path);
            e.Graphics.SetClip(path);
            using (Font f = new Font("Tahoma", 12, FontStyle.Bold))
                e.Graphics.DrawString("Draw Me!!", f, Brushes.Red, 10, 70);
            e.Graphics.ResetClip();

            return;


            //int halfThickness = thickness / 2;

            using (Pen p = new Pen(Color.FromArgb(Alpha, BorderColour.R, BorderColour.G, BorderColour.B), thickness))
            {
                GraphicsPath path2 = RoundedRectangle.Create(halfThickness, halfThickness, this.Size.Width - thickness, this.Size.Height - thickness);
                e.Graphics.DrawPath(p, path);
                

                using (SolidBrush br = new SolidBrush(Color.FromArgb(Alpha, this.BackColor.R, this.BackColor.G, this.BackColor.B)))
                {                    
                    e.Graphics.FillPath(Brushes.Blue, path);
                    //e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                    //                                          halfThickness,
                    //                                          panel1.ClientSize.Width - thickness,
                    //                                          panel1.ClientSize.Height - thickness));
                }
                

                //path = RoundedRectangle.Create(30, 5, 40, 40, 5);
                //e.Graphics.FillPath(Brushes.Blue, path);

                //path = RoundedRectangle.Create(8, 50, 50, 50, 5);
                //e.Graphics.DrawPath(Pens.Black, path);

                e.Graphics.SetClip(path);
                //using (Font f = new Font("Tahoma", 12, FontStyle.Bold))
                    e.Graphics.DrawString(this.Text, this.Font, Brushes.ForestGreen, 0, 70);
                e.Graphics.ResetClip();


                //e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                //                                          halfThickness,
                //                                          this.ClientSize.Width - thickness,
                //                                          this.ClientSize.Height - thickness));

                //using (SolidBrush brush = new SolidBrush(BackColor)) e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            
            //e.Graphics.DrawRectangle(Pens.Yellow, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }


        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //    if (System.ComponentModel.LicenseManager.UsageMode ==System.ComponentModel.LicenseUsageMode.Designtime) //IS in design mode
        //    {
        //        base.OnPaintBackground(pevent);
        //    }
        //}


    }
}
