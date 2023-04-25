using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.Model;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace BattleshipsTests.Model {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.Model.Fleet class.
	/// </summary>
	[TestClass]
	public class FleetTests {
		private class ShipMock : IShip {
			public ShipType Type {get; private set;}

			public Point Location {get; private set;}

			public ShipOrientation Orientation {get; private set;}

			public int Condition {get; private set;}

			public Size Size {
				get {
					return ((Orientation == ShipOrientation.Horizontal) ? new Size((int)Type - 1, 0) : new Size(0, (int)Type - 1));
				}
			}

			public Rect Bounds {
				get {
					return new Rect(Location, Size);
				}
			}

			public bool IsSunk {
				get {
					return (Condition == 0);
				}
			}

			public ShipMock() {
				Type = ShipType.Destroyer; // Not relevant to tests
				Location = new Point(0, 0); // Not relevant to tests
				Orientation = ShipOrientation.Horizontal; // Not relevant to tests
				Condition = -1; // Not relevant to tests
			}

			public ShipMock(int condition) {
				Type = ShipType.Destroyer; // Not relevant to tests
				Location = new Point(0, 0); // Not relevant to tests
				Orientation = ShipOrientation.Horizontal; // Not relevant to tests
				Condition = condition;
			}

			public ShipMock(ShipType type, Point location, ShipOrientation orientation) {
				Type = type;
				Location = location;
				Orientation = orientation;
				Condition = -1; // Not relevant to tests
			}

			public void RecordAHit(int position) {
				// Stub
			}
		}

		[TestMethod]
		public void Test_AreAllShipsSunk_Yes() {
			// Arrange, initialise the ships list with three IShip object instances, all of which have a Condition property of 0 
			var target = new Fleet(new[] {new ShipMock(0), new ShipMock(0), new ShipMock(0)});

			// Act, expected returns true
			var actual = target.AreAllShipsSunk;

			// Assert
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void Test_AreAllShipsSunk_No() {
			// Arrange, initialise the ships list with three IShip object instances, one of which has a Condition property of 1 and the other two that
 			// have a Condition property of 0
			var target = new Fleet(new[] {new ShipMock(1), new ShipMock(0), new ShipMock(0)});

			// Act, expected returns true
			var actual = target.AreAllShipsSunk;

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_DoesShipBoundsClash_Yes() {
			// Arrange, initialise the ships list with one IShip object instance that has a Type property of ShipType.Battleship, a Location property of
			// Point(2, 2) and an Orientation property of ShipOrientation.Vertical 
			var target = new Fleet(new[] {new ShipMock(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical)});

			// Act, expected returns true
			var actual = target.DoesShipBoundsClash(new Rect(0, 4, 4, 0));

			// Assert
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void Test_DoesShipBoundsClash_No() {
			// Arrange, initialise the ships list with one IShip object instance that has a Type property of ShipType.Battleship, a Location property of
			// Point(2, 2) and an Orientation property of ShipOrientation.Vertical 
			var target = new Fleet(new[] {new ShipMock(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical)});

			// Act, expected returns false
			var actual = target.DoesShipBoundsClash(new Rect(3, 1, 0, 4));

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_AddShip_UseNull() {
			// Arrange
			var target = new Fleet();
			const int expected = 0;

			// Act
			target.AddShip(null);

			// Assert, expected that the target object instance has no IShip object instances in its Ships collection
			Assert.AreEqual(expected, target.Ships.Count);
		}

		[TestMethod]
		public void Test_AddShip_UseValidShip() {
			// Arrange
			var target = new Fleet();
			const int expected00 = 1;
			const ShipType expected01 = ShipType.Battleship;
			const ShipOrientation expected02 = ShipOrientation.Vertical;
			var expected03 = new Point(2, 2);

			// Act
			target.AddShip(new ShipMock(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical));

			// Assert, expected that the target object instance has one IShip object instances in its Ships collection and that this IShip object instance
			// has a Type property of value ShipType.Battleship, with an Orientation property of ShipOrientation.Vertical and a Location property of
			// Point(2, 2)
			Assert.AreEqual(expected00, target.Ships.Count);
			Assert.AreEqual(expected01, target.Ships[0].Type);
			Assert.AreEqual(expected02, target.Ships[0].Orientation);
			Assert.AreEqual(expected03, target.Ships[0].Location);
		}

		[TestMethod]
		public void Test_CheckForAndRecordAnyHit_HasHit() {
			// Arrange, initialise the ships list with one IShip object instance that has a Type property of ShipType.Battleship, a Location property of
			// Point(2, 2) and an Orientation property of ShipOrientation.Vertical
			var target = new Fleet(new[] {new ShipMock(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical)});
			var expected = target.Ships[0];

			// Act
			var actual = target.CheckForAndRecordAnyHit(new Point(2, 3));

			// Assert, expected is the only IShip object instance in the Ships collection 
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_CheckForAndRecordAnyHit_HasNotHit() {
			// Arrange, initialise the ships list with one IShip object instance that has a Type property of ShipType.Battleship, a Location property of
			// Point(2, 2) and an Orientation property of ShipOrientation.Vertical
			var target = new Fleet(new[] {new ShipMock(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical)});

			// Act, expected return is null
			var actual = target.CheckForAndRecordAnyHit(new Point(6, 8));

			// Assert
			Assert.IsNull(actual);
		}
	}
}
