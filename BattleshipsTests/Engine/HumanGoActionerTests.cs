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
	/// Unit test to exercise methods which are members of the Battleships.Engine.HumanGoActioner class.
	/// </summary>
	[TestClass]
	public class HumanGoActionerTests {
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
				Type = ShipType.Destroyer;
				Location = new Point(0, 0);
				Orientation = ShipOrientation.Horizontal;
				Condition = -1;
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
			private int currentInputCommand;
			private readonly InputCommand[] inputCommands = new InputCommand[2]; // Can handle 2 consequecutive input command requests using GetInput()

			public InputHandlerMock() {
				inputCommands[0] = new InputCommand {Type = CommandType.None, Data = String.Empty};
				inputCommands[1] = new InputCommand {Type = CommandType.None, Data = String.Empty};
			}
			
			public InputHandlerMock(InputCommand inputCommand) {
				inputCommands[0] = inputCommand;
				inputCommands[1] = new InputCommand {Type = CommandType.None, Data = String.Empty};
			}
			
			public InputHandlerMock(InputCommand inputCommand0, InputCommand inputCommand1) {
				inputCommands[0] = inputCommand0;
				inputCommands[1] = inputCommand1;
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

			// Use this mock when testing an entry that requires no specific input command to be returned from the input handler
			public PlayerMock() {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock();
				GoActioner = null; // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
			}

			// Use this mock when testing an entry that requires a single input command to be returned from the input Handler
			public PlayerMock(InputCommand inputCommand) {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock(inputCommand);
				GoActioner = null; // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
			}

			// Use this mock when testing an entry that requires a set of previous attack values
			public PlayerMock(IEnumerable<string> previousAttacks) {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock();
				GoActioner = null; // Not relevant to these tests
				Attacks = new List<string>(previousAttacks);
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

			// Use this mock when testing an entry that requires a sequence of 2 input commands to be returned from the input handler
			// and that requires previous attack information for the first input command
			public PlayerMock(InputCommand inputCommand0, IEnumerable<string> previousAttacks, InputCommand inputCommand1) {
				Fleet = new FleetMock();
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock(inputCommand0, inputCommand1);
				GoActioner = null; // Not relevant to these tests
				Attacks = new List<string>(previousAttacks);
			}

			public void AddShipToFleet(ShipType type, Point location, ShipOrientation orientation) {
				// Stub
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
		public void Test_Action_HelpRequestCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"  !         - Show your previous attacks on the enemy fleet\n" +
									"  *         - Show the enemy's previous attacks on your fleet\n" +
									"  X or x    - Quit game immediately\n" +
									"  ?         - Show help (what you are seeing now)\n" +
									"[A-J][1-10] - Grid location to attack (eg. A5, E10)\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a help request entry followed by an attack location entry (to force the
			// Action() method to return)
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.HelpRequest, Data = String.Empty},
										 new InputCommand {Type = CommandType.Location, Data = "A1"}), new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_ShowNoOwnPreviousAttacksCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"XO to Captain, sir, we have not made any attacks yet...\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a show own previous attacks entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.ShowAttacks, Data = "!"}, new string[] {},
										 new InputCommand {Type = CommandType.Location, Data = "A1"}),
						  new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_ShowSomeOwnPreviousAttacksCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"XO to Captain, sir, our attacks so far:-\n" + "A1_ E10* " + "\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a show own previous attacks entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.ShowAttacks, Data = "!"}, new[] {"A1_", "E10*"},
										 new InputCommand {Type = CommandType.Location, Data = "A1"}),
						  new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_ShowNoEnemyPreviousAttacksCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"XO to Captain, sir, we have not been attacked yet...\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a show enemy previous attacks entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.ShowAttacks, Data = "*"},
										 new InputCommand {Type = CommandType.Location, Data = "A1"}),
										 new PlayerMock(new string[] {}));

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_ShowSomeEnemyPreviousAttacksCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"XO to Captain, sir, the enemy's attacks so far:-\n" + "A1_ E10* " + "\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a show enemy previous attacks entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.ShowAttacks, Data = "*"},
										 new InputCommand {Type = CommandType.Location, Data = "A1"}),
										 new PlayerMock(new[] {"A1_", "E10*"}));

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_LocationValidCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a location entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "A1"}), new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_LocationInvalidCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"XO to Captain, sir, this is not a valid grid co-ordinate...\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a location entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "INVALID"},
						  new InputCommand {Type = CommandType.Location, Data = "A1"}), new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_LocationNotWithinRangeCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"XO to Captain, sir, grid co-ordinate is not within range our guns...\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a location entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.Location, Data = "T99"},
						  new InputCommand {Type = CommandType.Location, Data = "A1"}), new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_InvalidCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"XO to Captain, sir, invalid command! Please repeat...\n" +
									"(enter ? for help)\n\n" +
									"XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): " +
									"Gunnery to Captain, sir, we scored a hit on an enemy ship and it sank!\n\n";

			// Act, construct the player mock object instance to process a location entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.None, Data = String.Empty},
										 new InputCommand {Type = CommandType.Location, Data = "A1"}), new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_Action_QuitCommand() {
			// Arrange, the HumanGoActioner object instance needs a valid IGame object instance that has a suitable game grid size
			var target = new HumanGoActioner {Game = new GameMock()};
			const string expected = "XO to Captain, sir, what is your command?\n" + "(Enter X or x for help): ";

			// Act, construct the player mock object instance to process a location entry
			target.Action(new PlayerMock(new InputCommand {Type = CommandType.Quit, Data = String.Empty}), new PlayerMock());

			// Assert, check the expected string against the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}
	}
}
