using System.Windows;
using Battleships.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace BattleshipsTests.Model {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.Model.Ship class.
	/// </summary>
	[TestClass]
	public class ShipTests {
		[TestMethod]
		public void Test_RecordAHit_ValidPosition() {
			// Arrange
			var target = new Ship(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical);
			const int expected = 0x001B; // In binary 11011, ie. hit at position 2

			// Act
			target.RecordAHit(2);
			var actual = target.Condition;

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_RecordAHit_InvalidPosition() {
			// Arrange
			var target = new Ship(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical);
			const int expected = 0x001F; // In binary 11111, ie. hit was not recorded because position was beyond the 5th binary digit

			// Act
			target.RecordAHit(6);
			var actual = target.Condition;

			// Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
