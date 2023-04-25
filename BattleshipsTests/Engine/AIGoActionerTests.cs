using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.Engine;
using Battleships.Model;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace BattleshipsTests.Engine {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.Engine.AIGoActioner class.
	/// </summary>
	[TestClass]
	public class AIGoActionerTests {
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

			public ShipMock() {
				Type = ShipType.Destroyer; // Not relevant to these tests
				Location = new Point(0, 0); // Not relevant to these tests
				Orientation = ShipOrientation.Horizontal; // Not relevant to these tests
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
				// Add one IShip object instance to the fleet mock object instance, so it can be hit
				Ships = new List<IShip> {new ShipMock()};
			}

			public bool DoesShipBoundsClash(Rect shipBounds) {
				// Stub
				return true;
			}

			public void AddShip(IShip ship) {
				// Stub
			}

			public IShip CheckForAndRecordAnyHit(Point location) {
				// Always hit a ship
				return Ships[0];
			}
		}

		private class InputHandlerMock : IInputHandler {
			public InputCommand GetInput() {
				// Return an InputCommand struct instance that provides a location type command that targets the game grid cell at "A1"   
				return new InputCommand {Type = CommandType.Location, Data = "A1"};
			}
		}

		private class OutputHandlerMock : IOutputHandler {
			// Use the static outputHandlerMessageBuffer to hold all messages output
			public void Message(string message) {
				outputHandlerMessageBuffer += message;
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
				InputHandler = new InputHandlerMock();
				GoActioner = null; // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
			}

			public void AddShipToFleet(ShipType type, Point location, ShipOrientation orientation) {
				// Stub
			}

			public void AddAttack(string attackInfo) {
				// Stub
			}
		}

		public static string outputHandlerMessageBuffer;

		[TestInitialize]
		public void TestInitialise() {
			outputHandlerMessageBuffer = String.Empty;
		}

		[TestMethod]
		public void Test_Action() {
			// Arrange
			var target = new AIGoActioner();
			const string expected = "AI scored a hit at A1 on enemy Destroyer and sank it!\n\n";

			// Act
			target.Action(new PlayerMock(), new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}
	}
}
