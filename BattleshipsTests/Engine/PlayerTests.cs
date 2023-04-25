using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.Engine;
using Battleships.Model;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace BattleshipsTests.Engine {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.Engine.Player class.
	/// </summary>
	[TestClass]
	public class PlayerTests {
		private class ShipMock : IShip {
			public ShipType Type {get; private set;}

			public Point Location {get; private set;}

			public ShipOrientation Orientation {get; private set;}

			public int Condition {get; private set;}

			public Size Size {
				get {
					// Stub
					return new Size(0, 0);
				}
			}

			public Rect Bounds {
				get {
					// Stub
					return new Rect(0, 0, 0, 0);
				}
			}

			public bool IsSunk {
				get {
					// Stub
					return true;
				}
			}

			public ShipMock(ShipType type, Point location, ShipOrientation orientation) {
				Type = type;
				Location = location;
				Orientation = orientation;
				Condition = -1; // Not relevant to these tests
			}

			public void RecordAHit(int position) {
				// Stub
			}
		}

		private class FleetMock : IFleet {
			public IList<IShip> Ships {get; private set;}

			public bool AreAllShipsSunk {
				get {
					// Stub
					return true;
				}
			}

			public FleetMock() {
				// Initially there is one IShip object instances contained within the fleet, a Destroyer oriented horizontally at location (0, 0)
				Ships = new List<IShip> {new ShipMock(ShipType.Destroyer, new Point(0, 0), ShipOrientation.Horizontal)};
			}

			public bool DoesShipBoundsClash(Rect shipBounds) {
				// Stub
				return true;
			}

			public void AddShip(IShip ship) {
				// Use this to test if a ship is successfully added to the fleet
				Ships.Add(ship);
			}

			public IShip CheckForAndRecordAnyHit(Point location) {
				// Stub
				return null;
			}
		}

		[TestMethod]
		public void Test_AddShipToFleet() {
			// Arrange
			var target = new Player(new FleetMock(), null, null, null, null); // Injected dependencies are not relevant to the test
			const int expected00 = 2;
			const ShipType expected01 = ShipType.Battleship;
			const ShipOrientation expected02 = ShipOrientation.Vertical;
			var expected03 = new Point(2, 2);

			// Act, note that the Destroyer is already in the fleet as a consequence of the FleetMock object instance construction
			target.AddShipToFleet(ShipType.Battleship, new Point(2, 2), ShipOrientation.Vertical);

			// Assert, expected that the target object instance has two IShip object instances in its Fleet collection and that the second IShip object
			// instance has a Type property of value ShipType.Battleship, with an Orientation property of ShipOrientation.Vertical and a Location
			// property of Point(2, 2)
			Assert.AreEqual(expected00, target.Fleet.Ships.Count);
			Assert.AreEqual(expected01, target.Fleet.Ships[1].Type);
			Assert.AreEqual(expected02, target.Fleet.Ships[1].Orientation);
			Assert.AreEqual(expected03, target.Fleet.Ships[1].Location);
		}

		[TestMethod]
		public void Test_AddAttack() {
			// Arrange
			var target = new Player(new FleetMock(), null, null, null, null); // Injected dependencies are not relevant to the test
			const int expected00 = 1;
			const string expected01 = "A1*";

			// Act
			target.AddAttack("A1*");

			// Assert, expected that the target object instance has one System.String object instances in its Attacks collection and that the value of
			// this is "A1*"
			Assert.AreEqual(expected00, target.Attacks.Count);
			Assert.AreEqual(expected01, target.Attacks[0]);
		}

		[TestMethod]
		public void Test_StringifyAttackInfo_WasNotHit() {
			// Arrange
			const string expected = "A1_";

			// Act
			var actual = Player.StringifyAttackInfo(new Point(0, 0), false);

			// Assert, expected is "A1_"
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_StringifyAttackInfo_WasHit() {
			// Arrange
			const string expected = "A1*";

			// Act
			var actual = Player.StringifyAttackInfo(new Point(0, 0), true);

			// Assert, expected is "A1*"
			Assert.AreEqual(expected, actual);
		}
	}
}
