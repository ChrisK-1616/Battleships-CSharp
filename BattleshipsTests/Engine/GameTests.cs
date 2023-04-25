using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.Engine;
using Battleships.Model;
using Battleships.UI;
using PlayerType = Battleships.Engine.Game.PlayerType;
using PlayerId = Battleships.Engine.Game.PlayerId;
using GameState = Battleships.Engine.Game.GameState;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace BattleshipsTests.Engine {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.Engine.Game class.
	/// </summary>
	[TestClass]
	public class GameTests {
		private class GoActionerMock : IGoActioner {
			public void Action(IPlayer player, IPlayer enemy) {
				// Stub
			}
		}

		private class AIGoActionerMock : IGoActioner {
			public void Action(IPlayer player, IPlayer enemy) {
				// Stub
			}
		}

		private class HumanGoActionerMock : IGoActioner {
			public void Action(IPlayer player, IPlayer enemy) {
				// Stub
			}
		}

		private class FleetBuilderMock : IFleetBuilder {
			public bool Build(IGame game, IPlayer player, Dictionary<ShipType, int> fleetComposition) {
				// Stub
				return true;
			}
		}

		private class InputHandlerMock : IInputHandler {
			public InputCommand GetInput() {
				// Stub
				return new InputCommand {Type = CommandType.None, Data = String.Empty};
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
			public IInputHandler InputHandler { get; private set;}
			public IGoActioner GoActioner {get; private set;}
			public List<string> Attacks {get; private set;}

			// The IsFleetSunk property holds either that all ships have been sunk or not dependent on the way the object instance is constructed
			public bool IsFleetSunk {get; private set;}

			public PlayerMock(bool isFleetSunk) {
				Fleet = null; // Not relevant to these tests
				FleetBuilder = null; // Not relevant to these tests
				OutputHandler = new OutputHandlerMock();
				InputHandler = new InputHandlerMock();
				GoActioner = new GoActionerMock(); // Not relevant to these tests
				Attacks = null; // Not relevant to these tests
				IsFleetSunk = isFleetSunk;
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
		public void Test_InitialisePlayer_MissingFleetComposition() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			// Grid size and injected dependencies are not relevant since the call to InitialisePlayer() will immediately return with a false value
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be false
			var actual = (bool)target.Invoke("InitialisePlayer", new object[] {PlayerId.One, // Not relevant
																			   PlayerType.Human, // Not relevant
																			   null});

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_InitialisePlayer_ValidFleetComposition() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be false
			var actual = (bool)target.Invoke("InitialisePlayer", new object[] {PlayerId.One, // Not relevant
																			   PlayerType.AI, // Not relevant
																			   new Dictionary<ShipType, int> {{ShipType.Destroyer, 1},
																											  {ShipType.Battleship, 1}}});

			// Assert
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void Test_InitialisePlayer_AddNewAIPlayer() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be a valid reference to an IPlayer object instance stored in field Battleships.Engine.Game.players
			// array at index 0
			target.Invoke("InitialisePlayer", new object[] {PlayerId.One, PlayerType.AI,
															new Dictionary<ShipType, int> {{ShipType.Destroyer, 1}, {ShipType.Battleship, 1}}});

			// Assert, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.players, and that this Player object instance has an AIGoActionerMock dependency injected into it
			Assert.IsNotNull(((IPlayer[])target.GetFieldOrProperty("players"))[(int)PlayerId.One]);
			Assert.IsInstanceOfType(((IPlayer[])target.GetFieldOrProperty("players"))[(int)PlayerId.One], typeof(IPlayer));
			Assert.IsInstanceOfType(((IPlayer[])target.GetFieldOrProperty("players"))[(int)PlayerId.One].GoActioner, typeof(AIGoActionerMock));
		}

		[TestMethod]
		public void Test_InitialisePlayer_AddNewHumanPlayer() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be a valid reference to an IPlayer object instance stored in field Battleships.Engine.Game.players
			// array at index 0, and that this Player object instance has a HumanGoActionerMock dependency injected into it
			target.Invoke("InitialisePlayer", new object[] {PlayerId.One, PlayerType.Human,
															new Dictionary<ShipType, int> {{ShipType.Destroyer, 1}, {ShipType.Battleship, 1}}});

			// Assert, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.players
			Assert.IsNotNull(((IPlayer[])target.GetFieldOrProperty("players"))[(int)PlayerId.One]);
			Assert.IsInstanceOfType(((IPlayer[])target.GetFieldOrProperty("players"))[(int)PlayerId.One], typeof(IPlayer));
			Assert.IsInstanceOfType(((IPlayer[])target.GetFieldOrProperty("players"))[(int)PlayerId.One].GoActioner, typeof(HumanGoActionerMock));
		}

		[TestMethod]
		public void Test_Quit() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));
			const GameState expected = GameState.Quit;

			// Act, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.state
			target.Invoke("Quit");
			var actual = (GameState)target.GetFieldOrProperty("state");

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_Run_SomePlayerWon() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var publicTarget = new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(), new FleetBuilderMock(),
										new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock());
			var privateTarget = new PrivateObject(publicTarget);
			privateTarget.SetFieldOrProperty("state", GameState.Won);
			const GameState expected = GameState.Quit;

			// Act, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.state
			publicTarget.Run(null, null);
			var actual = (GameState)privateTarget.GetFieldOrProperty("state");

			// Assert, the state field in the Game object instance should be set as GameState.Quit
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_Run_Quit() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var publicTarget = new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(), new FleetBuilderMock(),
										new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock());
			var privateTarget = new PrivateObject(publicTarget);
			privateTarget.SetFieldOrProperty("state", GameState.Quit);
			const string expected = "GAME OF BATTLESHIPS\n" + "===================\n\n" + "\n\nGame finished\n\n";

			// Act
			publicTarget.Run(null, null);

			// Assert, the expected string should be equal to the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_InitialiseGame_ValidFleetCompositions() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private,
			// grid size and injected dependencies are not relevant since the call to InitialiseGame() will immediately return with a false value
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be false
			var actual = (bool)target.Invoke("InitialiseGame", new object[] {new [] {PlayerType.Human, PlayerType.AI},
																			 new [] {new Dictionary<ShipType, int> {{ShipType.Destroyer, 1}, {ShipType.Battleship, 1}},
																					 new Dictionary<ShipType, int> {{ShipType.Destroyer, 1}, {ShipType.Battleship, 1}}}});

			// Assert
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void Test_InitialiseGame_NoFleetCompositions() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private,
			// grid size and injected dependencies are not relevant since the call to InitialiseGame() will immediately return with a false value
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be false
			var actual = (bool)target.Invoke("InitialiseGame", new object[] {new [] {PlayerType.Human, PlayerType.AI}, // Not relevant
																			 new Dictionary<ShipType, int>[] {null, null}});

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_InitialiseGame_InvalidFleetOneComposition() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private,
			// grid size and injected dependencies are not relevant since the call to InitialiseGame() will immediately return with a false value
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be false
			var actual = (bool)target.Invoke("InitialiseGame", new object[] {new [] {PlayerType.Human, PlayerType.AI}, // Not relevant
																			 new [] {null, new Dictionary<ShipType, int> {{ShipType.Destroyer, 1},
																														  {ShipType.Battleship, 1}}}});

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_InitialiseGame_InvalidFleetTwoComposition() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private,
			// grid size and injected dependencies are not relevant since the call to InitialiseGame() will immediately return with a false value
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be false
			var actual = (bool)target.Invoke("InitialiseGame", new object[] {new [] {PlayerType.Human, PlayerType.AI}, // Not relevant
																			 new [] {new Dictionary<ShipType, int> {{ShipType.Destroyer, 1},
																												    {ShipType.Battleship, 1}},
																					 null}});

			// Assert
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Test_ExecuteGameRound_PlayerOneWon() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));
			target.SetFieldOrProperty("players", new[] {new PlayerMock(false), new PlayerMock(true)}); // Player Two's fleet is recorded as sunk
			const int expected00 = 11;
			const GameState expected01 = GameState.Won;
			const PlayerId expected02 = PlayerId.One;
			const string expected03 = "Captain of fleet One, this is round 11\n" + "-----------------------------------\n" + "\n";

			// Act, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.state and Battleships.Engine.Game.winningPlayer
			var actual00 = target.Invoke("ExecuteGameRound", 10);
			var actual01 = (GameState)target.GetFieldOrProperty("state");
			var actual02 = (PlayerId)target.GetFieldOrProperty("winningPlayer");

			// Assert, return from method is expected to be 11, the state field in the Game object instance should be set as GameState.Won, the
			// winningPlayer field of the Game object instance should be set to PlayerId.One and the expected string should be equal to the string held
			// in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected00, actual00);
			Assert.AreEqual(expected01, actual01);
			Assert.AreEqual(expected02, actual02);
			Assert.AreEqual(expected03, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_ExecuteGameRound_PlayerTwoWon() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));
			target.SetFieldOrProperty("players", new[] {new PlayerMock(true), new PlayerMock(false)}); // Player One's fleet is recorded as sunk
			const int expected00 = 11;
			const GameState expected01 = GameState.Won;
			const PlayerId expected02 = PlayerId.Two;
			const string expected03 = "Captain of fleet One, this is round 11\n" + "-----------------------------------\n" +
									  "Captain of fleet Two, this is round 11\n" + "-----------------------------------\n" + "\n";

			// Act, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.state and Battleships.Engine.Game.winningPlayer
			var actual00 = target.Invoke("ExecuteGameRound", 10);
			var actual01 = (GameState)target.GetFieldOrProperty("state");
			var actual02 = (PlayerId)target.GetFieldOrProperty("winningPlayer");

			// Assert, return from method is expected to be 11, the state field in the Game object instance should be set as GameState.Won, the
			// winningPlayer field of the Game object instance should be set to PlayerId.Two and the expected string should be equal to the string held
			// in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected00, actual00);
			Assert.AreEqual(expected01, actual01);
			Assert.AreEqual(expected02, actual02);
			Assert.AreEqual(expected03, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_ExecuteGameRound_NeitherPlayerWon() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));
			target.SetFieldOrProperty("players", new[] { new PlayerMock(false), new PlayerMock(false) }); // Neither Player's fleet is recorded as sunk
			const int expected00 = 11;
			const PlayerId expected01 = PlayerId.None;
			const string expected02 = "Captain of fleet One, this is round 11\n" + "-----------------------------------\n" +
									  "Captain of fleet Two, this is round 11\n" + "-----------------------------------\n" + "\n";

			// Act, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.winningPlayer
			var actual00 = target.Invoke("ExecuteGameRound", 10);
			var actual01 = (PlayerId)target.GetFieldOrProperty("winningPlayer");

			// Assert, return from method is expected to be 11, the winningPlayer field of the Game object instance should be set to PlayerId.None and
			// the expected string should be equal to the string held in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected00, actual00);
			Assert.AreEqual(expected01, actual01);
			Assert.AreEqual(expected02, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_ExecuteGameRound_PlayerOneRequestedQuitImmediately() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));
			target.SetFieldOrProperty("players", new[] { new PlayerMock(false), new PlayerMock(false) }); // Neither Player's fleet is recorded as sunk
			target.SetFieldOrProperty("state", GameState.Quit); // Simulate Player One entering request to quit immediately
			const int expected00 = 11;
			const GameState expected01 = GameState.Quit;

			// Act, use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class to allow read access to private field
			// Battleships.Engine.Game.state
			var actual00 = target.Invoke("ExecuteGameRound", 10);
			var actual01 = (GameState)target.GetFieldOrProperty("state");

			// Assert, return from method is expected to be 11, the state field in the Game object instance should be set as GameState.Quit
			Assert.AreEqual(expected00, actual00);
			Assert.AreEqual(expected01, actual01);
		}

		[TestMethod]
		public void Test_GameWon() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private,
			// grid size and injected dependencies are not relevant since the call to InitialiseGame() will immediately return with a false value
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));
			target.SetFieldOrProperty("winningPlayer", PlayerId.One); // Record PlayerId.One as the winning player
			const GameState expected00 = GameState.Quit;
			const string expected01 = "Well done Captain of fleet One, you have sunk the enemy and won the day!!!\n\n";

			// Act
			target.Invoke("GameWon");
			var actual = (GameState)target.GetFieldOrProperty("state");

			// Assert, state field in the Game object instance should be set as GameState.Quit and the expected string should be equal to the string held
			// in the outputHandlerMessageBuffer field
			Assert.AreEqual(expected00, actual);
			Assert.AreEqual(expected01, outputHandlerMessageBuffer);
		}

		[TestMethod]
		public void Test_QuitGame() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject wrapper class since the exercised method is private,
			// grid size and injected dependencies are not relevant since the call to InitialiseGame() will immediately return with a false value
			var target = new PrivateObject(new Game(new Size(10, 10), new OutputHandlerMock(), new InputHandlerMock(), new InputHandlerMock(),
													new FleetBuilderMock(), new FleetBuilderMock(), new AIGoActionerMock(), new HumanGoActionerMock()));

			// Act, expected result should be true
			var actual = (bool)target.Invoke("QuitGame");

			// Assert
			Assert.IsTrue(actual);
		}
	}
}
