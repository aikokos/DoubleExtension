// <copyright file="DoubleExtension.cs" company="LearningCompany">
// Copyright (c) Company. All rights reserved.
// </copyright>

namespace DoubleExtension
{
    using System;

    /// <summary>
    /// The main DoubleExtension class.
    /// Contains all methods for task for finding IEEE754 Encoding.
    /// </summary>
    public static class DoubleExtension
    {
        /// <summary>
        /// Method for converting integer part of decimal number to binary.
        /// </summary>
        /// <param name="number">double number</param>
        /// <returns>Returns converted to binary integer part.</returns>
        public static string ConvertToBinaryIntegerPart(double number)
        {
            long onlyIntegerPart = 0;
            onlyIntegerPart = Convert.ToInt64(Math.Floor(number));
            string converted = string.Empty;
            while (onlyIntegerPart > 1)
            {
                long oneDigit = onlyIntegerPart % 2;
                converted += oneDigit.ToString();
                onlyIntegerPart = onlyIntegerPart / 2;
            }

            if (number > 0)
            {
                converted += "1";
            }

            if (converted.Length == 0)
            {
                converted = "0";
            }

            char[] charArray = converted.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Method for converting fraction part of decimal number to binary.
        /// </summary>
        /// <param name="number">double number</param>
        /// <returns>Returns converted to binary fraction part.</returns>
        public static string ConvertToBinaryFractionPart(double number)
        {
            string converted = string.Empty;
            double fraction = number - Convert.ToInt64(number);
            long onlyIntegerPart = 10;
            while (fraction != 0 && converted.Length < 63)
            {
                fraction = fraction * 2;
                onlyIntegerPart = Convert.ToInt64(Math.Floor(fraction));
                converted += onlyIntegerPart.ToString();
                fraction = fraction - (long)fraction;
            }

            return converted;
        }

        /// <summary>
        /// Method for converting decimal number to binary.
        /// </summary>
        /// <param name="number">double number</param>
        /// <returns>Returns converted to binary number.</returns>
        public static string ConvertDecimalToBinary(double number)
        {
            if (number - (int)number > 0)
            {
                return ConvertToBinaryIntegerPart(number) + "." + ConvertToBinaryFractionPart(number);
            }

            return ConvertToBinaryIntegerPart(number);
        }

        /// <summary>
        /// Method for converting binary number to scientific notation.
        /// </summary>
        /// <param name="number">double number</param>
        /// <param name="binaryNumber">number represented in binary format</param>
        /// <returns>Returns converted to scientific notation binary number.</returns>
        public static (int, string) GetScientificNotation(double number, string binaryNumber)
        {
            int exponent;
            string mantissa;
            int offset = 1023;
            if (binaryNumber.IndexOf(".") > 0)
            {
                string beforePoint = binaryNumber.Substring(0, binaryNumber.IndexOf("."));
                string afterPoint = binaryNumber.Substring(binaryNumber.IndexOf(".") + 1, binaryNumber.Length - beforePoint.Length - 1);
                mantissa = beforePoint.Substring(1, beforePoint.Length - 1) + afterPoint;
                exponent = beforePoint.Length - 1;
            }
            else
            {
                mantissa = binaryNumber.Substring(1, binaryNumber.Length - 1);
                exponent = binaryNumber.Length - 1;
            }

            if (Math.Abs(number) > 1)
            {
                exponent += offset;
            }
   
            return (exponent, mantissa);
        }

        /// <summary>
        /// Method for converting number to IEEE754 notation.
        /// </summary>
        /// <param name="number">double number</param>
        /// <returns>Returns converted to IEEE754 number.</returns>
        public static string GetIEEE754(this double number)
        { 
            string numberInIEEE754 = string.Empty;
             if (number == double.MinValue)
            {
                numberInIEEE754 = "1111111111101111111111111111111111111111111111111111111111111111";
                return numberInIEEE754;
            }

            if (number == double.MaxValue)
            {
                numberInIEEE754 = "0111111111101111111111111111111111111111111111111111111111111111";
                return numberInIEEE754;
            }

            if (double.IsNaN(number))
            {
                numberInIEEE754 = "1111111111111000000000000000000000000000000000000000000000000000";
                return numberInIEEE754;
            }

            if (number == double.NegativeInfinity)
            {
                numberInIEEE754 = "1111111111110000000000000000000000000000000000000000000000000000";
                return numberInIEEE754;
            } 

            if (number == double.PositiveInfinity)
            {
                numberInIEEE754 = "0111111111110000000000000000000000000000000000000000000000000000";
                return numberInIEEE754;
            }

            if (number == double.Epsilon)
            {
                numberInIEEE754 = "0000000000000000000000000000000000000000000000000000000000000001";
                return numberInIEEE754;
            }

            //// Step 1 - Convert given number to binary
            string binaryNumber = ConvertDecimalToBinary(Math.Abs(number));
            //// Step 2 - Represent given number in scientific notation
            var names = GetScientificNotation(number, binaryNumber);
            //// Step 3 - Get IEEE754 Double precision
            string binaryExponent = ConvertDecimalToBinary(names.Item1);
            string mantissa = names.Item2;
            //// Sign
            string sign;
            sign = number >= 0 ? "0" : "1";
            if (mantissa.Length > 52)
            {
                mantissa = mantissa.Substring(0, 52);
            }

            if (mantissa.Length < 52)
            {
                for (int i = 0; i < 52 - mantissa.Length + 1; i++)
                {
                    mantissa += '0';
                }
            }

            numberInIEEE754 = sign + binaryExponent + mantissa;
            while (numberInIEEE754.Length < 64)
            {               
                numberInIEEE754 += '0';
            }

            return numberInIEEE754;
        }
    }
}
