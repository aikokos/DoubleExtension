// <copyright file="DoubleExtensionTests.cs" company="LearningCompany">
// Copyright (c) Company. All rights reserved.
// </copyright>

namespace DoubleExtension
{
    using NUnit.Framework;

    /// <summary>
    /// The tests for main DoubleExtension class.
    /// Contains method for testing task for finding IEEE754 Encoding.
    /// </summary>
    public class DoubleExtensionTests
    {
        /// <summary>
        /// Method for testing DoubleExtension class converting to IEEE754.
        /// </summary>
        /// <param name="number">double number</param>
        /// <returns>Returns converted to binary integer part.</returns>
        [Test]
        [TestCase(-255.255, ExpectedResult = "1100000001101111111010000010100011110101110000101000111101011100")]
        [TestCase(255.255, ExpectedResult = "0100000001101111111010000010100011110101110000101000111101011100")]
        [TestCase(4294967295.0, ExpectedResult = "0100000111101111111111111111111111111111111000000000000000000000")]
        [TestCase(double.MinValue, ExpectedResult = "1111111111101111111111111111111111111111111111111111111111111111")]
        [TestCase(double.MaxValue, ExpectedResult = "0111111111101111111111111111111111111111111111111111111111111111")]
        [TestCase(double.Epsilon, ExpectedResult = "0000000000000000000000000000000000000000000000000000000000000001")]
        [TestCase(double.NaN, ExpectedResult = "1111111111111000000000000000000000000000000000000000000000000000")]
        [TestCase(double.NegativeInfinity, ExpectedResult = "1111111111110000000000000000000000000000000000000000000000000000")]
        [TestCase(double.PositiveInfinity, ExpectedResult = "0111111111110000000000000000000000000000000000000000000000000000")]
        [TestCase(-0.0, ExpectedResult = "1000000000000000000000000000000000000000000000000000000000000000")]
        [TestCase(0.0, ExpectedResult = "0000000000000000000000000000000000000000000000000000000000000000")]
        
        public string DoubleExtensionTest(double number)
        {
            return number.GetIEEE754();
        }
    }
}