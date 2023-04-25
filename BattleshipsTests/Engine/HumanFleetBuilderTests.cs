using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.Engine;
using Battleships.Model;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace BattleshipsTests.Engine {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.Engine.HumanFleetBuilder class.
	/// </summary>
	[TestClass]
	public class HumanFleetBuilderTests {
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

		private class InputHandlerMock : IInputHandler {
			private int currentInputCommand;
			private readonly InputCommand[] inputCommands = new InputCommand[4]; // Can handle upto 4 sequential input command requests from GetInput()

			public InputHandlerMock() {
				inputCommands[0] = new InputCommand {Type = CommandType.None, Data = String.Empty};
				inputCommands[1] = new InputCommand {Type = CommandType.None, Data = String.Empty};
				inputCommands[2] = new InputCommand {Type = CommandType.None, Data = String.Empty};
				inputCommands[3] = new InputCommand {Type = CommandType.None, Data = String.Empty};
			}

			public InputHandlerMock(InputCommand inputCommand) {
				inputCommands[0] = inputCommand;
				inputCommands[1] = new InputCommand {Type = CommandType.None, Data = String.Empty};
				inputCommands[2] = new InputCommand {Type = CommandType.None, Data = String.Empty};
				inputCommands[3] = new InputCommand {Type = CommandType.None, Data = String.Empty};
			}

			public InputHandlerMock(InputCommand inputCommand0, InputCommand inputCommand1) {
				inputCommands[0] = inputCommand0;
				inputCommands[1] = inputCommand1;
				inputCommands[2] = new InputCommand {Type = CommandType.None, Data = String.Empty};
				inputCommands[3] = new InputCommand {Type = CommandType.None, Data = String.Empty};
			}

			public InputHandlerMock(InputCommand inputCommand0, InputCommand inputCommand1, InputCommand inputCommand2) {
				inputCommands[0] = inputCommand0;
				inputCommands[1] = inputCommand1;
				inputCommands[2] = inputCommand2;
				inputCommands[3] = new InputCommand {Type = CommandType.None, Data = String.Empty};
			}

			public InputHandlerMock(InputCommand inputCommand0, InputCommand inputCommand1, InputCommand inputCommand2,
									InputCommand inputCommand3) {
				inputCommands[0] = inputCommand0;
				inputCommands[1] = inputCommand1;
				inputCommands[2] = inputCommand2;
				inputCommands[3] = inputCommand3;
			}

			public InputCommand GetInput() {
				// Return the current input command
				return inputCommands[currentInputCommand++];
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

			// Use this mock when testing an entry that requires a single input command to be returned from the input handler
			public PlayerMock(InputCommand inputCommand) {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock(inputCommand);
				GoActioner = null; // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
			}

			// Use this mock when testing an entry that requires a sequence of 2 input commands to be returned from the input handler
			public PlayerMock(InputCommand inputCommand0, InputCommand inputCommand1) {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock(inputCommand0, inputCommand1);
				GoActioner = null; // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
			}

			// Use this mock when testing an entry that requires a sequence of 3 input commands to be returned from the input handler
			public PlayerMock(InputCommand inputCommand0, InputCommand inputCommand1, InputCommand inputCommand2) {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock(inputCommand0, inputCommand1, inputCommand2);
				GoActioner = null; // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
			}

			// Use this mock when testing an entry that requires a sequence of 4 input commands to be returned from the input handler
			public PlayerMock(InputCommand inputCommand0, InputCommand inputCommand1, InputCommand inputCommand2, InputCommand inputCommand3) {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock(inputCommand0, inputCommand1, inputCommand2, inputCommand3);
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

		public static string outputHandlerMessageBuffer;

		[TestInitialize]
		public void TestInitialise() {
			outputHandlerMessageBuffer = String.Empty;
		}

		[TestMethod]
		public void Test_GetValidDeployment_ValidEntries() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));
			var expected00 = new Deployment {Location = new Point(2, 2), Orientation = ShipOrientation.Horizontal};
			const string expected01 = "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " +
									  "Orientation: " +
									  "Helm to Captain, Battleship1 deployed sir.\n\n";

			// Act, location and orientation are expected to be Point(2, 2) and ShipOrientation.Horizontal
			var actual = (Deployment?)target.InvokeStatic("GetValidDeployment", new GameMock(),
														   new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "C3"}, 
																		  new InputCommand {Type = CommandType.Orientation, Data = "h"}),
														   ShipType.Battleship, 1);

			// Assert, check expected location and orientation and check expected string against the string held in the outputHandlerMessageBuffer
			// field
			Assert.AreEqual(expected00, actual);
			Assert.AreEqual(expected01, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_GetValidDeployment_QuitAtLocationEntry() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));

			// Act, expected to return null
			var actual = (Deployment?)target.InvokeStatic("GetValidDeployment", new GameMock(),
														  new PlayerMock(new InputCommand {Type = CommandType.Quit, Data = String.Empty}),
														  ShipType.Battleship, 1);

			// Assert
			Assert.IsNull(actual);
		}

		[TestMethod]
		public void Test_GetValidDeployment_QuitAtOrientationEntry() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));

			// Act, expected to return null
			var actual = (Deployment?)target.InvokeStatic("GetValidDeployment", new GameMock(),
														  new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "C3"},
																		 new InputCommand {Type = CommandType.Quit, Data = String.Empty}),
														  ShipType.Battleship, 1);

			// Assert
			Assert.IsNull(actual);
		}

		[TestMethod]
		public void Test_GetValidDeployment_InvalidLocationWrongFormat() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));
			var expected00 = new Deployment { Location = new Point(2, 2), Orientation = ShipOrientation.Horizontal };
			const string expected01 = "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " +
									  "Helm to Captain, sir, this is not a valid grid co-ordinate!\n\n" +
									  "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " + "Orientation: " +
									  "Helm to Captain, Battleship1 deployed sir.\n\n";

			// Act, location and orientation are expected to be Point(2, 2) and ShipOrientation.Horizontal
			var actual = (Deployment?)target.InvokeStatic("GetValidDeployment", new GameMock(),
														  new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "INVALID"},
																		 new InputCommand {Type = CommandType.Location, Data = "C3"}, 
																		 new InputCommand {Type = CommandType.Orientation, Data = "h"}),
														  ShipType.Battleship, 1);

			// Assert, check expected location and orientation and check expected string against the string held in the outputHandlerMessageBuffer
			// field
			Assert.AreEqual(expected00, actual);
			Assert.AreEqual(expected01, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_GetValidDeployment_InvalidOrientationWrongFormat() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));
			var expected00 = new Deployment { Location = new Point(2, 2), Orientation = ShipOrientation.Horizontal };
			const string expected01 = "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " + "Orientation: " +
									  "Helm to Captain, sir, this is not a valid ship orientation!\n\n" +
									  "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " + "Orientation: " +
									  "Helm to Captain, Battleship1 deployed sir.\n\n";

			// Act, location and orientation are expected to be Point(2, 2) and ShipOrientation.Horizontal
			var actual = (Deployment?)target.InvokeStatic("GetValidDeployment", new GameMock(),
														  new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "C3"},
																		 new InputCommand {Type = CommandType.Orientation, Data = "a"},
																		 new InputCommand {Type = CommandType.Location, Data = "C3"},
																		 new InputCommand {Type = CommandType.Orientation, Data = "h"}),
														  ShipType.Battleship, 1);

			// Assert, check expected location and orientation and check expected string against the string held in the outputHandlerMessageBuffer
			// field
			Assert.AreEqual(expected00, actual);
			Assert.AreEqual(expected01, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_GetValidDeployment_ValidEntriesPartiallyOutOfGrid() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));
			var expected00 = new Deployment { Location = new Point(2, 2), Orientation = ShipOrientation.Horizontal };
			const string expected01 = "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " + "Orientation: " +
									  "Helm to Captain, sir, ship orientation and grid co-ordinate does not fit into battle area!\n\n" +
									  "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " + "Orientation: " +
									  "Helm to Captain, Battleship1 deployed sir.\n\n";

			// Act, location and orientation are expected to be Point(2, 2) and ShipOrientation.Horizontal
			var actual = (Deployment?)target.InvokeStatic("GetValidDeployment", new GameMock(),
														  new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "H3"},
																		 new InputCommand {Type = CommandType.Orientation, Data = "h"},
																		 new InputCommand {Type = CommandType.Location, Data = "C3"},
																		 new InputCommand {Type = CommandType.Orientation, Data = "h"}),
														  ShipType.Battleship, 1);

			// Assert, check expected location and orientation and check expected string against the string held in the outputHandlerMessageBuffer
			// field
			Assert.AreEqual(expected00, actual);
			Assert.AreEqual(expected01, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_GetValidDeployment_ValidEntriesClashesExistingShip() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));
			var expected00 = new Deployment { Location = new Point(2, 2), Orientation = ShipOrientation.Horizontal };
			const string expected01 = "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " + "Orientation: " +
									  "Helm to Captain, sir, ship orientation and grid co-ordinate rams an existing ship!\n\n" +
									  "Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for Battleship1\n" +
									  "(enter X or x to quit the game immediately)\n" +
									  "Grid co-ordinate: " + "Orientation: " +
									  "Helm to Captain, Battleship1 deployed sir.\n\n";

			// Act, location and orientation are expected to be Point(2, 2) and ShipOrientation.Horizontal
			var actual = (Deployment?)target.InvokeStatic("GetValidDeployment", new GameMock(),
														  new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "A1"},
																		 new InputCommand {Type = CommandType.Orientation, Data = "h"},
																		 new InputCommand {Type = CommandType.Location, Data = "C3"},
																		 new InputCommand {Type = CommandType.Orientation, Data = "h"}),
														  ShipType.Battleship, 1);

			// Assert, check expected location and orientation and check expected string against the string held in the outputHandlerMessageBuffer
			// field
			Assert.AreEqual(expected00, actual);
			Assert.AreEqual(expected01, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_AddShipToFleet() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(HumanFleetBuilder));
			const int expected00 = 2;
			const ShipType expected01 = ShipType.Battleship;
			const ShipOrientation expected02 = ShipOrientation.Vertical;
			var expected03 = new Point(2, 2);

			// Act, expected to return true and note that the Destroyer is already in the fleet as a consequence of the FleetMock object instance
			// construction
			var playerMock = new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "C3"},
			                                new InputCommand {Type = CommandType.Orientation, Data = "v"});
			var actual = (bool)target.InvokeStatic("AddShipToFleet", new GameMock(), playerMock, ShipType.Battleship, 1);

			// Assert, expected that the PlayerMock has two IShip object instances in the PlayerMock.Fleet collection and that the second IShip object
			// instance has a Type property of value ShipType.Battleship, with an Orientation property of ShipOrientation.Vertical and a Location
			// property of Point(2, 2)
			Assert.IsTrue(actual);
			Assert.AreEqual(expected00, playerMock.Fleet.Ships.Count);
			Assert.AreEqual(expected01, playerMock.Fleet.Ships[1].Type);
			Assert.AreEqual(expected02, playerMock.Fleet.Ships[1].Orientation);
			Assert.AreEqual(expected03, playerMock.Fleet.Ships[1].Location);
		}

		[TestMethod]
		public void Test_Build_FleetCompositionIsOneBattleship() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new HumanFleetBuilder();
			const int expected00 = 2;
			const ShipType expected01 = ShipType.Battleship;
			const ShipOrientation expected02 = ShipOrientation.Vertical;
			var expected03 = new Point(2, 2);

			// Act, expected to return true and note that the Destroyer is already in the fleet as a consequence of the FleetMock object instance
			// construction
			var playerMock = new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "C3"},
											new InputCommand {Type = CommandType.Orientation, Data = "v"});
			var actual = target.Build(new GameMock(), playerMock, new Dictionary<ShipType, int> {{ShipType.Destroyer, 0}, {ShipType.Battleship, 1}});

			// Assert, expected that the PlayerMock has two IShip object instances in its PlayerMock.Fleet collection and that the second IShip object
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
