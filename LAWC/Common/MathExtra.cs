using System;
using System.Globalization;

namespace LAWC.Common
{

    internal static class MathExtra
    {

        internal static decimal RoundCustom(decimal source, decimal modulus)
        {
            return (Math.Round(source / modulus) * modulus);
        }
        internal static double RoundCustom(double source, double modulus)
        {
            return (Math.Round(source / modulus) * modulus);
        }


        internal static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        internal static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / 1024f;
        }


        //From: https://stackoverflow.com/questions/14488796/does-net-provide-an-easy-way-convert-bytes-to-kb-mb-gb-etc
        // use: Console.WriteLine(SizeSuffix(100005000L));

        internal static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };


        internal static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException(decimalPlaces.ToString(CultureInfo.InvariantCulture)); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format(CultureInfo.InvariantCulture, "{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        


    }
}
