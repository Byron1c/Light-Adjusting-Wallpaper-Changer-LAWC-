using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;


//https://rosettacode.org/wiki/Random_number_generator_(device)#C.23
//https://codereview.stackexchange.com/questions/184125/cryptographic-range-based-random-number-generator-class

public static class Randomizer
{
    const double MAX_RANGE = (double)UInt64.MaxValue + 1;

    /// <summary>
    /// Get a cryptographic random integer in the range from
    /// min(inclusive) to max(exclusive)            
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double Next(double min, double max)
    {
        if (max < min)
        {
            throw new ArgumentException("max is less than min");
        }
        if (min < 0)
        {
            throw new ArgumentException("min and max must be positive integers");
        }
        if (min == max)
        {
            return min;
        }
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomNumber = new byte[8];

            rng.GetBytes(randomNumber);
            double baseNum = BitConverter.ToUInt64(randomNumber, 0) / MAX_RANGE;

            double range = max - min;

            return (int)(baseNum * range) + min;
        }

    }


    /// <summary>
    /// Get a cryptographic random integer in the range from
    /// 0 to max(exclusive)
    /// </summary>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double Next(float max)
    {
        return Next(0, max);
    }


    /// <summary>
    /// Get a cryptographic random 32-bit integer
    /// </summary>
    /// <returns></returns>
    public static double Next()
    {
        return Next(0, double.MaxValue); //Int32.MaxValue);
    }
    
    
    /// <summary>
    /// A cryptographic random shuffle method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    public static ICollection<T> Shuffle<T>(ICollection<T> input)
    {
        return input.OrderBy(x => Next()).ToArray();
    }

}
