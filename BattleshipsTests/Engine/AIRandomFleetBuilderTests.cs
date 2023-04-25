using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.Engine;
using Battleships.Model;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace BattleshipsTests.Engine {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.Engine.AIRandomFleetBuilder class.
	/// </summary>
	[TestClass]
	public class AIRandomFleetBuilderTests {
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
				return Ships.Any(s => shipBounds.IntersectsWith(s.Bounds));
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

		private class OutputHandlerMock : IOutputHandler {
			public void Message(string message) {
				// Stub
			}
		}

		private class PlayerMock : IPlayer {
			public IFleet Fleet {get; private set;}
			public IFleetBuilder FleetBuilder {get; private set;}
			public IOutputHandler OutputHandler {get; private set;}
			public IInputHandler InputHandler {get; private set;}
			public IGoActioner GoActioner {get; private set;}
			public List<string> Attacks {get; private set;}
			public bool IsFleetSunk {
				get {
					// Stub
					return true;
				}
			}

			public PlayerMock() {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = null; // Not relevant to these tests
				GoActioner = null; // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
			}

			public void AddShipToFleet(ShipType type, Point location, ShipOrientation orientation) {
				Fleet.AddShip(new ShipMock(type, location, orientation));
			}

			public void AddAttack(string attackInfo) {
				// Stub
			}
		}

		private class GameMock : IGame {
			public Rect GridBounds {
				get {
					// Suitable game grid bounds for testing
					return new Rect(0, 0, 10, 10);
				}
			}

			public void Run(Game.PlayerType[] playerType, Dictionary<ShipType, int>[] playerFleets) {
				// Stub
			}

			public void Quit() {
				// Stub
			}
		}

		/// <summary>
		/// This overrides the Next(...) methods of the System.Random class to deliver a supplied sequence of random numbers. Use an instance
		/// of this class to replace the true random number generator utilised by AIRandomFleetBuilder object instances during testing (to make
		/// any tests deterministic).
		/// </summary>
		private class RandomFake : Random {
			/// <summary>
			/// Sequence of numbers to be returned by the Next(...) methods, this is designed to used in a cyclic fashion if more than List.Count
			/// numbers have been returned.
			/// </summary>
			private readonly IList<int> numberSequence;

			/// <summary>
			/// Next number counter, incremented each time a new number is taken from the number sequence.
			/// </summary>
			private int nextNumberIndex;

			public RandomFake(IEnumerable<int> numberSequence) {
				// Initialise the sequence of numbers to be returned by the Next(...) methods
				this.numberSequence = new List<int>(numberSequence);
			}

			/// <summary>
			/// Overridden method to supply a number from the number sequence rather than a proper random number.
			/// </summary>
			/// <returns>int, number selected from number sequence.</returns>
			public override int Next() {
				// If the number sequence is empty, then return 0 (best that can be done)
				if(numberSequence.Count == 0) {
					return 0;
				}

				// Get current number index
				var numberIndex = nextNumberIndex % numberSequence.Count;

				// Increment next number index
				nextNumberIndex++;

				return numberSequence[numberIndex];
			}

			/// <summary>
			/// Overridden method to supply a number from the number sequence rather than a proper random number.
			/// </summary>
			/// <param name="maxValue">This is not used in this fake random implementation.</param>
			/// <returns>int, number selected from number sequence.</returns>
			public override int Next(int maxValue) {
				return Next();
			}
		}

		[TestMethod]
		public void Test_GetValidDeployment() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var rng = new RandomFake(new[] {0, 0, 0, // This sequence fails as it clashes with an existing ship's occupancy in the game grid
											1, 2, 2}); // This sequence succeeds as it does not clash with an existing ship's game grid occupancy

			var target = new PrivateObject(new AIRandomFleetBuilder(rng));
			var expected = new Deployment {Location = new Point(2, 2), Orientation = ShipOrientation.Vertical};

			// Act, accepted location and orientation are expected to be Point(2, 2) and ShipOrientation.Vertical
			var actual = (Deployment)target.Invoke("GetValidDeployment", new object[] {new GameMock(), new PlayerMock(), (int)ShipType.Battleship});

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_AddShipToFleet() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var rng = new RandomFake(new[] {1, 2, 2}); // Sequence that does not clash with existing ship's game grid occupancy

			var target = new PrivateObject(new AIRandomFleetBuilder(rng));
			const int expected00 = 2;
			const ShipType expected01 = ShipType.Battleship;
			const ShipOrientation expected02 = ShipOrientation.Vertical;
			var expected03 = new Point(2, 2);

			// Act, note that the Destroyer is already in the fleet as a consequence of the FleetMock object instance construction
			var playerMock = new PlayerMock();
			target.Invoke("AddShipToFleet", new object[] {new GameMock(), playerMock, ShipType.Battleship});

			// Assert, expected that the PlayerMock has two IShip object instances in the PlayerMock.Fleet collection and that the second IShip object
			// instance has a Type property of value ShipType.Battleship, with an Orientation property of ShipOrientation.Vertical and a Location
			// property of Point(2, 2)
			Assert.AreEqual(expected00, playerMock.Fleet.Ships.Count);
			Assert.AreEqual(expected01, playerMock.Fleet.Ships[1].Type);
			Assert.AreEqual(expected02, playerMock.Fleet.Ships[1].Orientation);
			Assert.AreEqual(expected03, playerMock.Fleet.Ships[1].Location);
		}

		[TestMethod]
		public void Test_Build_FleetCompositionIsOneBattleship() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var rng = new RandomFake(new[] {1, 2, 2}); // Sequence that does not clash with existing ship's game grid occupancy
	
			var target = new AIRandomFleetBuilder(rng);
			const int expected00 = 2;
			const ShipType expected01 = ShipType.Battleship;
			const ShipOrientation expected02 = ShipOrientation.Vertical;
			var expected03 = new Point(2, 2);

			// Act, the Destroyer is already in the fleet as a function of the FleetMock object instance construction
			var playerMock = new PlayerMock();
			var actual = target.Build(new GameMock(), playerMock, new Dictionary<ShipType, int> {{ShipType.Destroyer, 0}, {ShipType.Battleship, 1}});

			// Assert, expected that the PlayerMock has two IShip object instances in the PlayerMock.Fleet collection and that the second IShip object
			// instance has a Type property of value ShipType.Battleship, with an Orientation property of ShipOrientation.Vertical and a Location
			// property of Point(2, 2)
			Assert.IsTrue(actual);
			Assert.AreEqual(expected00, playerMock.Fleet.Ships.Count);
			Assert.AreEqual(expected01, playerMock.Fleet.Ships[1].Type);
			Assert.AreEqual(expected02, playerMock.Fleet.Ships[1].Orientation);
			Assert.AreEqual(expected03, playerMock.Fleet.Ships[1].Location);
		}
	}
}
