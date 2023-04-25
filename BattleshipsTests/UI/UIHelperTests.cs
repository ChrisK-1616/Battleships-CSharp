using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.UI;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace BattleshipsTests.UI {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.UI.UIHelper class.
	/// </summary>
	[TestClass]
	public class UIHelperTests {
		[TestMethod]
		public void Test_CheckOrientationInput_InvalidAlphasTooMany() {
			// Arrange
			const string input = "HH";

			// Act, expected result should be false
			var actual = UIHelper.CheckOrientationInput(input);

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_CheckOrientationInput_InvalidAlphasNotHhorVv() {
			// Arrange
			const string input = "X";

			// Act, expected result should be false
			var actual = UIHelper.CheckOrientationInput(input);

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_CheckOrientationInput_ValidHhOrVv() {
			// Arrange
			const string input0 = "h";
			const string input1 = "v";

			// Act, expected result should be true
			var actual00 = UIHelper.CheckOrientationInput(input0);
			var actual01 = UIHelper.CheckOrientationInput(input0.ToUpper());

			// Assert
			Assert.IsTrue(actual00);
			Assert.IsTrue(actual01);

			// Act, expected result should be true
			var actual10 = UIHelper.CheckOrientationInput(input1);
			var actual11 = UIHelper.CheckOrientationInput(input1.ToUpper());

			// Assert
			Assert.IsTrue(actual10);
			Assert.IsTrue(actual11);
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void Test_ParseOrientationInput_InvalidAlphasTooMany() {
			// Arrange
			const string input = "HH";

			// Act, should throw System.FormatException
			UIHelper.ParseOrientationInput(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void Test_ParseOrientationInput_InvalidAlphasNotHhorVv() {
			// Arrange
			const string input = "X";

			// Act, should throw System.FormatException
			UIHelper.ParseOrientationInput(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		public void Test_ParseOrientationInput_ValidHhOrVv() {
			// Arrange
			const string input0 = "h";
			const string input1 = "v";
			const ShipOrientation expected0 = ShipOrientation.Horizontal;
			const ShipOrientation expected1 = ShipOrientation.Vertical;

			// Act, expected result should be ShipOrientation.Horizontal
			var actual00 = UIHelper.ParseOrientationInput(input0);
			var actual01 = UIHelper.ParseOrientationInput(input0.ToUpper());

			// Assert
			Assert.AreEqual(expected0, actual00);
			Assert.AreEqual(expected0, actual01);

			// Act, expected result should be ShipOrientation.Vertical
			var actual10 = UIHelper.ParseOrientationInput(input1);
			var actual11 = UIHelper.ParseOrientationInput(input1.ToUpper());

			// Assert
			Assert.AreEqual(expected1, actual10);
			Assert.AreEqual(expected1, actual11);
		}

		[TestMethod]
		public void Test_CheckLocationInput_InvalidAlphasTooMany() {
			// Arrange
			const string input = "AA10";

			// Act, expected result should be false
			var actual = UIHelper.CheckLocationInput(input);

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_CheckLocationInput_InvalidAlphasNone() {
			// Arrange
			const string input = "10";

			// Act, expected result should be false
			var actual = UIHelper.CheckLocationInput(input);

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_CheckLocationInput_InvalidNumericsTooMany() {
			// Arrange
			const string input = "A110";

			// Act, expected result should be false
			var actual = UIHelper.CheckLocationInput(input);

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_CheckLocationInput_InvalidNumericsNone() {
			// Arrange
			const string input = "A";

			// Act, expected result should be false
			var actual = UIHelper.CheckLocationInput(input);

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_CheckLocationInput_ValidOneNumeric() {
			// Arrange
			const string input = "C5";

			// Act, expected result should be true
			var actual = UIHelper.CheckLocationInput(input);

			// Assert
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void Test_CheckLocationInput_ValidTwoNumerics() {
			// Arrange
			const string input = "E10";

			// Act, expected result should be true
			var actual = UIHelper.CheckLocationInput(input);

			// Assert
			Assert.IsTrue(actual);
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void Test_ParseLocationInput_InvalidAlphasTooMany() {
			// Arrange
			const string input = "AA10";

			// Act, should throw System.FormatException
			UIHelper.ParseLocationInput(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void Test_ParseLocationInput_InvalidAlphasNone() {
			// Arrange
			const string input = "10";

			// Act, should throw System.FormatException
			UIHelper.ParseLocationInput(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void Test_ParseLocationInput_InvalidNumericsTooMany() {
			// Arrange
			const string input = "A110";

			// Act, should throw System.FormatException
			UIHelper.ParseLocationInput(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void Test_ParseLocationInput_InvalidNumericsNone() {
			// Arrange
			const string input = "A";

			// Act, should throw System.FormatException
			UIHelper.ParseLocationInput(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		public void Test_ParseLocationInput_ValidOneNumeric() {
			// Arrange
			const string input = "C5";
			var expected = new Point(2, 4);

			// Act, expected result should be a Point instance with properties X = 2 and Y = 4
			var actual = UIHelper.ParseLocationInput(input);

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_ParseLocationInput_ValidTwoNumerics() {
			// Arrange
			const string input = "E10";
			var expected = new Point(4, 9);

			// Act, expected result should be a Point instance with properties X = 4 and Y = 9
			var actual = UIHelper.ParseLocationInput(input);

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_StringifyLocation_SingleNumeric() {
			// Arrange
			var input = new Point(2, 4);
			const string expected = "C5";

			// Act, expected result should be the string "C5"
			var actual = UIHelper.StringifyLocation(input);

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_StringifyLocation_DoubleNumeric() {
			// Arrange
			var input = new Point(4, 9);
			const string expected = "E10";

			// Act, expected result should be the string "E10"
			var actual = UIHelper.StringifyLocation(input);

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Test_StringifyLocation_NegativeXValue() {
			// Arrange
			var input = new Point(-4, 9);

			// Act, should throw System.ArgumentOutOfRangeException
			UIHelper.StringifyLocation(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Test_StringifyLocation_NegativeYValue() {
			// Arrange
			var input = new Point(4, -9);

			// Act, should throw System.ArgumentOutOfRangeException
			UIHelper.StringifyLocation(input);

			// Assert
			Assert.Fail("Should not get here");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Test_StringifyLocation_XValueGreaterThan25() {
			// Arrange
			var input = new Point(44, 9);

			// Act, should throw System.ArgumentOutOfRangeException
			UIHelper.StringifyLocation(input);

			// Assert
			Assert.Fail("Should not get here");
		}
	}
}
