using System;
using System.Drawing;
using System.Globalization;


namespace LAWC.Objects
{
    public static class Taskbar
    {

        //// PARTS FROM http://stackoverflow.com/questions/17848090/how-to-get-a-smooth-color-change

        /// Helper method to convert from a Win32 BGRA-format color to a .NET color.
        internal static Color BgraToColor(uint color)
        {
            return Color.FromArgb(Int32.Parse(color.ToString("X", CultureInfo.InvariantCulture), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
        }

        // Helper method to convert from a .NET color to a Win32 BGRA-format color.
        internal static uint ColorToBgra(Color color)
        {
            return (uint)(color.B | (color.G << 8) | (color.R << 16) | (color.A << 24));
        }


        internal struct DWM_COLORIZATION_PARAMS
        {
            internal uint clrColor;
            internal uint clrAfterGlow;
            internal uint nIntensity;
            internal uint clrAfterGlowBalance;
            internal uint clrBlurBalance;
            internal uint clrGlassReflectionIntensity;
            internal bool fOpaque;
        }


        internal static Color ColorizationColor
        {
            get
            {
                // Call the DwmGetColorizationParameters function to fill in our structure.
                NativeMethods.DwmGetColorizationParameters(out DWM_COLORIZATION_PARAMS parameters);

                // Convert the colorization color to a .NET color and return it.
                return BgraToColor(parameters.clrColor);
            }
            set
            {
                // Retrieve the current colorization parameters, just like we did above.
                NativeMethods.DwmGetColorizationParameters(out DWM_COLORIZATION_PARAMS parameters);

                // Then modify the colorization color.
                // Note that the other parameters are left untouched, so they will stay the same.
                // You can also modify these; that is left as an exercise.
                parameters.clrColor = ColorToBgra(value);

                // Call the DwmSetColorizationParameters to make the change take effect.
                NativeMethods.DwmSetColorizationParameters(ref parameters, false);
            }
        }

        /// <summary>
        /// Gets Is current OS Windows Vista or later
        /// </summary>
        internal static Boolean AeroSupport
        {
            get { return (System.Environment.OSVersion.Version.Major >= 6); }
        }



    }
}
