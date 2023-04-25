using System;
using System.Windows;
using System.Collections.Generic;
using Battleships.Model;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;

namespace Battleships.Engine {
	/// <summary>
	/// <para>This is the class that provides the operation of the game of Battleships. This is achieved by calling the Run() method.
	/// This Run() method allows the game to progress as a series of "rounds" with each player having (or "actioning") their "go".
	/// The game continues until either all ships from a player's fleet are sunk, in which case the other player is deemed the winner,
	/// or the human player quits from the game.</para>
	/// <para> </para>
	/// <para>Prior to the start of this sequence of rounds, each player must "deploy" their fleet. This deployment entails adding the
	/// requisite number of each ship type to their fleet at a supplied game grid cell location and orientation across this game grid.
	/// Once this deployment has occurred for both the human and artificial intelligence (AI) player, the rounds begin. The game grid
	/// is the main playing surface, and each "cell" within this grid is identified with a letter and number combination. The letter
	/// represents the column of the grid, the number represents the row of the grid. Letters begin at "A" whilst the numbers begin at
	/// "1". For example, to identify the cell located at the third column and fifth row on the grid, the letter and number combination
	/// used would be "C5".</para>
	/// <para> </para>
	/// <para>The AI player has an automated deployment process injected into the game object at construction along with an automated "go"
	/// actioning process. These injected dependencies allow for the potential to utilise various AI strategies for playing the game see
	/// <see cref="Battleships.Launcher.Program"/>.</para>
	/// <para> </para>
	/// <para>The human player manually deploys their fleet (by supplying a location and orientation for each of their ships), and manually
	/// inputs a command for their "go" during a round. The handler for achieving the human input is injected into the game object during
	/// its construction, and this allows the user interface (UI) of the game to be adapted for various implementation strategies, for
	/// instance using console device input, GUI based input or even input from a file system (to possibly simulate the human player) or
	/// across a network connection.</para>
	/// <para> </para>
	/// <para>Likewise, the output from the game is implemented using a handler injected into the game object during construction. This output
	/// handler can then be adapted in a similar fashion to the input handler, allowing for output to a console device, a GUI or again to
	/// the file system or a network connection.</para>
	/// <para> </para>
	/// <para>The commands available to the human player are implemented in the input handler and go actioner objects. These are injected into
	/// the human player object instance, and for this implementation of the game can be found in Battleships.UI.ConsoleBasedInputHandler and
	/// Battleships.Engine.HumanGoActioner. Refer to these classes for a full description of this.</para>
	/// <para> </para>
	/// <para>There are always two players in the game, an AI and a human player, and the order in which they take their "go" in a round is
	/// determined at construction of the game object.</para>
	/// </summary>
	public class Game : IGame {
		/// <summary>
		/// Constant to hold the number of players of the game.
		/// </summary>
		public const int NUMBER_OF_PLAYERS = 2;

		/// <summary>
		/// Each player is given an ID using a value from this enum.
		/// </summary>
		public enum PlayerId {
			/// <summary>
			/// Player one, first to have a "go" each round.
			/// </summary>
			One = 0,

			/// <summary>
			/// Player two, second to "go" each round.
			/// </summary>
			Two,
			
			/// <summary>
			/// No player ID provided.
			/// </summary>
			None = Int32.MaxValue
		}

		/// <summary>
		/// Used to describe the type of player, either human or artificial intelligence (AI).
		/// </summary>
		public enum PlayerType {
			/// <summary>
			/// Human player.
			/// </summary>
			Human = 0,

			/// <summary>
			/// AI player.
			/// </summary>
			AI
		}

		/// <summary>
		/// As the game progresses it enters a number of game states, these are the values for each of these.
		/// </summary>
		public enum GameState {
			/// <summary>
			/// The game is initialising before being able to be played.
			/// </summary>
			Initialising = 0,

			/// <summary>
			/// The game is cycling through its rounds.
			/// </summary>
			Playing,

			/// <summary>
			/// The game has been won by a player.
			/// </summary>
			Won,

			/// <summary>
			/// The game is about to quit.
			/// </summary>
			Quit
		}

		/// <summary>
		/// Property that holds an array of references to Player objects, there are always two players so an array is adequate (no need
		/// for a dynamic collection).
		/// </summary>
		private readonly IPlayer[] players;

		/// <summary>
		/// Property that holds the current state of the game.
		/// </summary>
		private GameState state;

		/// <summary>
		/// Property that holds the ID of the winning player, or PlayerId.None if no current winning player.
		/// </summary>
		private PlayerId winningPlayer;

		/// <summary>
		/// Property that holds a reference to the injected dependency that implements the output handler for the game.
		/// </summary>
		private readonly IOutputHandler outputHandler;
	
		/// <summary>
		/// Property that holds a reference to the injected dependency that implements the input handler for the AI player.
		/// </summary>
		private readonly IInputHandler aiInputHandler;
		
		/// <summary>
		/// Property that holds a reference to the injected dependency that implements the input handler for the human player.
		/// </summary>
		private readonly IInputHandler humanInputHandler;
		
		/// <summary>
		/// Property that holds a reference to the injected dependency that implements the fleet builder for the AI player.
		/// </summary>
		private readonly IFleetBuilder aiFleetBuilder;

		/// <summary>
		/// Property that holds a reference to the injected dependency that implements the fleet builder for the human player.
		/// </summary>
		private readonly IFleetBuilder humanFleetBuilder;
		
		/// <summary>
		/// Property that holds a reference to the injected dependency that implements the go actioner for the AI player.
		/// </summary>
		private readonly IGoActioner aiGoActioner;

		/// <summary>
		/// Property that holds a reference to the injected dependency that implements the go actioner for the human player.
		/// </summary>
		private readonly IGoActioner humanGoActioner;

		/// <summary>
		/// Property that holds the game grid bounds, the top left location of this is at (0, 0) and all game grid cell co-ordinates are in
		/// respect to this with increasing column values going left to right and increasing row values going top to bottom. The game grid
		/// is the playing surface for the game, all player ships are located within the confines of this game grid.
		/// </summary>
		public Rect GridBounds {get; private set;}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="gridSize">Number of columns and rows that give the game grid its size.</param>
		/// <param name="outputHandler">Reference to the injected dependency that implements the output handler for the game.</param>
		/// <param name="aiInputHandler">Reference to the injected dependency that implements the input handler for the AI player.</param>
		/// <param name="humanInputHandler">Reference to the injected dependency that implements the input handler for the human player.</param>
		/// <param name="aiFleetBuilder">Reference to the injected dependency that implements the fleet builder for the AI player.</param>
		/// <param name="humanFleetBuilder">Reference to the injected dependency that implements the fleet builder for the human player.</param>
		/// <param name="aiGoActioner">Reference to the injected dependency that implements the go actioner for the AI player.</param>
		/// <param name="humanGoActioner">Reference to the injected dependency that implements the go actioner for the human player.</param>
		public Game(Size gridSize, IOutputHandler outputHandler, IInputHandler aiInputHandler, IInputHandler humanInputHandler,
					IFleetBuilder aiFleetBuilder, IFleetBuilder humanFleetBuilder, IGoActioner aiGoActioner, IGoActioner humanGoActioner) {
			GridBounds = new Rect(0, 0, gridSize.Width - 1, gridSize.Height - 1);
			
			this.outputHandler = outputHandler;
			this.aiInputHandler = aiInputHandler;
			this.humanInputHandler = humanInputHandler;
			this.aiFleetBuilder = aiFleetBuilder;
			this.humanFleetBuilder = humanFleetBuilder;
			this.aiGoActioner = aiGoActioner;
			this.humanGoActioner = humanGoActioner;
			
			players = new IPlayer[NUMBER_OF_PLAYERS];

			// Initial state of the game is GameState.Initialising and with no player having yet won the game
			state = GameState.Initialising;
			winningPlayer = PlayerId.None;
		}

		/// <summary>
		/// Main operating method of the game within which the game repeatedly executes rounds of player "go" actions until either a player wins
		/// the game or the game is quit. This method reacts each cycle to the particular game state and also moves the game through its various
		/// states as they transition due to game and player events.
		/// </summary>
		/// <param name="playerType">First player type (PlayerType.Human or PlayerType.AI) in this array becomes the first player (PlayerId.One)
		/// in each round.</param>
		/// <param name="playerFleets">Fleet composition for each player's fleet. Each reference in this array of Dictionary objects maps a ship
		/// type to the quantity of ship of that type within a player's fleet.</param>
		/// <returns>Nothing.</returns>
		public void Run(PlayerType[] playerType, Dictionary<ShipType, int>[] playerFleets) {
			// Use this local variable to control the game rounds loop
			var finished = false;

			// Count the number of rounds undertaken
			var roundsCount = 0;

			// Hello message to players
			outputHandler.Message("GAME OF BATTLESHIPS\n");
			outputHandler.Message("===================\n\n");

			// Game rounds loop, use the finished variable to control this
			while(!finished) {
				// Perform various actions based upon the state of the game
				switch(state) {
					case GameState.Initialising: {
						// Initialise each player in turn, if initialisation fails for either player then transition the game state to GameState.Quit
						// since it is now not possible to continue, otherwise transition to GameState.Playing
						state = InitialiseGame(playerType, playerFleets) ? GameState.Playing : GameState.Quit;
						
						break;
					}

					case GameState.Playing: {
						// Execute a game round, returning an incremented value for the number of game rounds executed so far
						roundsCount = ExecuteGameRound(roundsCount);

						break;
					}

					case GameState.Won: {
						// Process that the game has been won
						GameWon();

						break;
					}

					case GameState.Quit: {
						// Process that the game has been quit
						finished = QuitGame();

						break;
					}
				}
			}
		}

		/// <summary>
		/// Initialises each player in turn according to the InitialisePlayer() method.
		/// </summary>
		/// <param name="playerType">First player type (PlayerType.Human or PlayerType.AI) in this array becomes the first player (PlayerId.One)
		/// in each round.</param>
		/// <param name="playerFleets">Fleet composition for each player's fleet. Each reference in this array of Dictionary objects maps a ship
		/// type to the quantity of ship of that type within a player's fleet.</param>
		/// <returns>bool, return true if both players initialised successfully, false otherwise.</returns>
		private bool InitialiseGame(IList<PlayerType> playerType, IList<Dictionary<ShipType, int>> playerFleets) {
			// Initialise each player
			var success = InitialisePlayer(PlayerId.One, playerType[(int)PlayerId.One], playerFleets[(int)PlayerId.One]);

			// Only initialise the second player if the first succeeded
			if(success) {
				success &= InitialisePlayer(PlayerId.Two, playerType[(int)PlayerId.Two], playerFleets[(int)PlayerId.Two]);
			}

			return success;
		}

		/// <summary>
		/// Execute a round in the game by allowing each player to have their "go". If one player sinks the other player's fleet then that
		/// player is recorded as the winning player in the WinningPlayer property and the game state is set to GameState.Won.
		/// </summary>
		/// <param name="roundsCount">Current number of game rounds that have been executed so far.</param>
		/// <returns>int, incremented count of game rounds executed so far.</returns>
		private int ExecuteGameRound(int roundsCount) {
			// Each player has their "go" in the sequence of PlayerId.One then PlayerId.Two
			roundsCount++;

			// PlayerId.One has their "go"
			outputHandler.Message(String.Format("Captain of fleet One, this is round {0}\n", roundsCount));
			outputHandler.Message("-----------------------------------\n");

			players[(int)PlayerId.One].GoActioner.Action(players[(int)PlayerId.One], players[(int)PlayerId.Two]);

			// Check to see if PlayId.One won by sinking PlayerId.Two's fleet
			if(players[(int)PlayerId.Two].IsFleetSunk) {
				winningPlayer = PlayerId.One;

				// Transition to GameState.Won
				state = GameState.Won;
			}

			// If PlayerId.One did not win or request the game to quit immediately then PlayerId.Two has their "go"
			if((state != GameState.Won) && (state != GameState.Quit)) {
				outputHandler.Message(String.Format("Captain of fleet Two, this is round {0}\n", roundsCount));
				outputHandler.Message("-----------------------------------\n");
				players[(int)PlayerId.Two].GoActioner.Action(players[(int)PlayerId.Two], players[(int)PlayerId.One]);

				// Check to see if PlayId.Two won by sinking PlayerId.One's fleet
				if(players[(int)PlayerId.One].IsFleetSunk) {
					winningPlayer = PlayerId.Two;

					// Transition to GameState.Won
					state = GameState.Won;
				}
			}

			outputHandler.Message("\n");
			return roundsCount;
		}

		/// <summary>
		/// The game has been won, so congratulate the winning player and transition the game state to GameState.Quit.
		/// </summary>
		/// <returns>Nothing.</returns>
		private void GameWon() {
			// Congratulate the winning player, then transition to GameState.Quit
			outputHandler.Message(String.Format("Well done Captain of fleet {0}, you have sunk the enemy and won the day!!!\n\n", winningPlayer));
			state = GameState.Quit;
		}

		/// <summary>
		/// The game has been quit so say goodbye to the player and return true to set the finished control variable in the Run() method 
		/// </summary>
		/// <returns>bool, always returns true to indicate that the Run() method while(...) loop can be exited.</returns>
		private bool QuitGame() {
			// Goodbye message
			outputHandler.Message("\n\nGame finished\n\n");

			// Return true so that the Run() method while(...) loop can be exited  
			return true;
		}

		/// <summary>
		/// Instantiate and initialise a Player object. This involves injecting the fleet builder, output handler, input handler and go
		/// actioner dependencies in during construction and then calling the Build() method of the fleet builder dependency. The player
		/// type supplied determines the particular dependencies that are injected, with the human player using an instance of the
		/// Battleships.Engine.HumanFleetBuilder class and an instance of the Battleships.Engine.HumanGoActioner class. The other instances
		/// to use for the injected dependencies are held in their respective properties. 
		/// </summary>
		/// <param name="playerId">ID of the player being initialised.</param>
		/// <param name="playerType">Type of the player (Human or AI).</param>
		/// <param name="playerFleetComposition">Reference to a Dictionary that maps a ship type to the quantity of that ship type in the
		/// player's fleet.</param>
		/// <returns>bool, returns true if the player is initialised successfully or false otherwise.</returns>
		private bool InitialisePlayer(PlayerId playerId, PlayerType playerType, Dictionary<ShipType, int> playerFleetComposition) {
			// Sanity check on the supplied Dictionary to ensure it is valid
			if(ReferenceEquals(null, playerFleetComposition)) {
				// Not valid so no Player object instantiated
				players[(int)playerId] = null;

				// Return false to indicate an unsuccessful initialisation
				return false;
			}

			// Human player and AI player instances are instantiated using different dependencies 
			if(playerType == PlayerType.Human) {
				players[(int)playerId] = new Player(new Fleet(), humanFleetBuilder, outputHandler, humanInputHandler, humanGoActioner);
			}
			else {
				players[(int)playerId] = new Player(new Fleet(), aiFleetBuilder, outputHandler, aiInputHandler, aiGoActioner);
			}

			// Return the result of the Battleships.Engine.IFleetBuilder.Build() method call, this will be true if the fleet build was successful
			// and false otherwise
			return players[(int)playerId].FleetBuilder.Build(this, players[(int)playerId], playerFleetComposition);
		}

		/// <summary>
		/// Request the game to quit. This method enables other dependent objects to request the game to quit.
		/// </summary>
		/// <returns>Nothing.</returns>
		public void Quit() {
			// Transition to GameState.Quit
			state = GameState.Quit;
		}
	}
}
