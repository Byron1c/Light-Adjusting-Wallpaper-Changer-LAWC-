using System.Drawing;
using System.Windows.Forms;


namespace LAWC.Common
{
    /// <summary>
    /// https://stackoverflow.com/questions/23057651/change-backcolor-of-toolstripseparator-in-submenue
    /// </summary>
    public class ExtendedToolStripSeparator : ToolStripSeparator
    {
        internal Color ForeColour = Control.DefaultForeColor;
        internal Color BackColour = Control.DefaultBackColor;
               
        public ExtendedToolStripSeparator()
        {
            this.Paint += ExtendedToolStripSeparator_Paint;
        }

        private void ExtendedToolStripSeparator_Paint(object sender, PaintEventArgs e)
        {
            // Get the separator's width and height.
            ToolStripSeparator toolStripSeparator = (ToolStripSeparator)sender;
            int width = toolStripSeparator.Width;
            int height = toolStripSeparator.Height;

            // Choose the colors for drawing.
            // I've used Color.White as the foreColor.
            Color foreColor = ForeColour; //Color.FromName(ForeColour);
            // Color.Teal as the backColor.
            Color backColor = BackColour; // Color.FromName(BackColour);

            // Fill the background.
            e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, width, height);

            // Draw the line.
            e.Graphics.DrawLine(new Pen(foreColor), 4, height / 2, width - 4, height / 2);
        }
    }


/// EXAMPLES:
/// add the separators:

//ToolStripSeparator toolStripSeparator = new ExtendedToolStripSeparator();

//this.DropDownItems.Add(newGameToolStripMenuItem);
//this.DropDownItems.Add(addPlayerToolStripMenuItem);
//this.DropDownItems.Add(viewResultsToolStripMenuItem);
//// Add the separator here.
//this.DropDownItems.Add(toolStripSeparator);
//this.DropDownItems.Add(exitToolStripMenuItem);
}
