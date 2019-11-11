using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleExtension
{
    public static class DoubleExtension
    {
        public static string ConvertToBinaryIntegerPart(double number)
        {

            int onlyIntegerPart = 0;

            try
            {
                onlyIntegerPart = Convert.ToInt32(Math.Floor(number));
            } catch (Exception e)
            {
                
            };


            string converted = string.Empty;
            while (onlyIntegerPart > 1)
            {
                int oneDigit = onlyIntegerPart % 2;
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

        public static string ConvertToBinaryFractionPart(double number)
        {
            string converted = string.Empty;
            double fraction = number - (int)number;
            int onlyIntegerPart = 10;
            while (fraction != 0 && converted.Length < 63)
            {
                fraction = fraction * 2;
                try
                {
                    onlyIntegerPart = Convert.ToInt32(Math.Floor(fraction));
                } catch (Exception e)
                {
                    
                }
                converted += onlyIntegerPart.ToString();
                fraction = fraction - (int)fraction;
            }
            return converted;
        }

        public static string ConvertDecimalToBinary(double number)
        {
            if (number - (int)number > 0)
            {
                return ConvertToBinaryIntegerPart(number) + "." + ConvertToBinaryFractionPart(number);
            }
            return ConvertToBinaryIntegerPart(number);
        }

        public static (int, string) GetScientificNotation(string binaryNumber)
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
                
            } else
            {
                mantissa = binaryNumber.Substring(1, binaryNumber.Length - 1);
                exponent = binaryNumber.Length - 1;
            }
            exponent += offset;
            return (exponent, mantissa);
        }

        public static string GetIEEE754(this double number)
        {
            string numberInIEEE754 = string.Empty;
            // Step 1 - Convert given number to binary
            string binaryNumber = ConvertDecimalToBinary(Math.Abs(number));
            // Step 2 - Represent given number in scientific notation
            var names = GetScientificNotation(binaryNumber);
            // Step 3 - Get IEEE754 Double precision
            string binaryExponent = ConvertDecimalToBinary(names.Item1);
            string mantissa = names.Item2;
            // Sign
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
            return numberInIEEE754;
        }
    }
}
