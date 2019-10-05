using System;
using System.Drawing;
using System.Windows.Forms;


namespace LAWC.Common
{
    internal static class BoundsUtilities
    {
        internal static Rectangle AddBounds(Rectangle sourceBounds, Rectangle newBounds)
        {
            if (newBounds.Right > sourceBounds.Right)
                sourceBounds.Width += (newBounds.Right - sourceBounds.Width);

            if (newBounds.Bottom > sourceBounds.Bottom)
                sourceBounds.Height += (newBounds.Bottom - sourceBounds.Height);

            if (newBounds.Left < sourceBounds.Left)
            {
                sourceBounds.X = newBounds.X;
            }

            if (newBounds.Top < sourceBounds.Top)
            {
                sourceBounds.Y = newBounds.Y;
            }

            return sourceBounds;
        }


        /// <summary>
        /// Creates a new Rectangle by translating an existing Rectangle based on a given Point
        /// </summary>
        /// <param name="r">The original rectangle</param>
        /// <param name="p">The reference point (representing 0,0)</param>
        /// <returns>The translated rectangle</returns>
        public static Rectangle ZeroRectangle(Rectangle r, Point p)
        {
            Rectangle output = new Rectangle(
                r.X + p.X,
                r.Y + p.Y,
                r.Width + p.X,
                r.Height + p.Y);

            return output;

            //return new Rectangle(
            //    r.X + p.X, 
            //    r.Y + p.Y,
            //    r.Width + p.X, 
            //    r.Height + p.Y);
        }

        internal static Point GetRefPoint(Rectangle vAllBounds)
        {
            Point refPoint = new Point();
            // Screens to the left or above the primary screen cause 0,0 to be other
            // than the top/left corner of the Bitmap
            if (vAllBounds.X < 0) refPoint.X = Math.Abs(vAllBounds.X);
            if (vAllBounds.Y < 0) refPoint.Y = Math.Abs(vAllBounds.Y);

            return refPoint;
        }

        internal static Rectangle GetAllMonitorsBounds()
        {
            Rectangle overallBounds = new Rectangle();
            //Point refPoint = new Point();

            foreach (Screen scr in Screen.AllScreens)
            {
                //dpi adjustment
                //System.Windows.Point dpi = GetDpi(scr);
                overallBounds = System.Drawing.Rectangle.Union(overallBounds, scr.Bounds);
            }

            return overallBounds;
        }




    }
}
