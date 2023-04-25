using System;
using System.Collections.Generic;
using System.Windows;
using Battleships.Engine;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;

namespace Battleships.Launcher {
	/// <summary>
	/// Main class.
	/// </summary>
	public class Program {
		/// <summary>
		/// Constant holding the width of the game grid (in number of cells)
		/// </summary>
		public const int GRID_WIDTH = 10;

		/// <summary>
		/// Constant holding the height of the game grid (in number of cells)
		/// </summary>
		public const int GRID_HEIGHT = 10;

		/// <summary>
		/// Number of Destroyer type ships in each player's fleets
		/// </summary>
		public const int NUMBER_OF_DESTROYERS = 2;
		
		/// <summary>
		/// Number of Battleship type ships in each player's fleets
		/// </summary>
		public const int NUMBER_OF_BATTLESHIPS = 1;

		/// <summary>
		/// Main method
		/// </summary>
		/// <returns>Nothing.</returns>
		static void Main() {
			var gridSize = new Size(GRID_WIDTH, GRID_HEIGHT);

			// Inject into the game object instance the size of the game grid, an input handler for the AI player, an input handler
			// for the human player, a fleet builder for the AI and human player and an actioner for handling the AI and human "go"
			// logic.
			// 
			// This allows a game to played with different AIs, so there could be a concept of "Difficulty" introduced where a more
			// challenging AI can be inject into the game when the human player chooses a higher difficulty.
			//
			// As it stands, the AISimpleRandomInputHandler and AIRandomFleetBuilder instances are incredibly "dumb", literally applying
			// randomness to the positioning of the fleet and the game grid location to attack on each "go". This can result in the AI
			// making multiple attacks on the same location despite the fact that it has already been attacked (a particularly pointless
			// strategy in the game of Battleships).
			//
			// Note, it is assumed that there would always be a human player, but by injecting a second AI fleet builder and AI go actioner
			// in place of the human versions, this AI could play against the first AI in an automated fashion.
			var humanGoActioner = new HumanGoActioner();
			var game = new Game(gridSize, new ConsoleBasedOutputHandler(), new AISimpleRandomInputHandler(gridSize), new ConsoleBasedInputHandler(),
								new AIRandomFleetBuilder(), new HumanFleetBuilder(), new AIGoActioner(), humanGoActioner);
			
			// The HumanGoActioner.Action() method makes use of the Game object instance, so it has to be set as a property in the HumanGoActioner
			// object instance, this is only possible after the game object instance has been instantiated. It may be a better option to use a
			// singleton for the game object instance rather than passing it around between dependent methods. There is only one game object
			// instance after all
			humanGoActioner.Game = game;

			// On instructing the game to run, the order in which the human and AI players take their turns and the composition of the human
			// and AI fleets are passed in. In the current configuration, the human player becomes "Player.One" and takes the first turn. The
			// composition of the fleets are determined by the NUMBER_OF_DESTROYERS and NUMBER_OF_BATTLESHIPS constants.
			//
			// It would be reasonably easy to provide a way to configure this (maybe via reading in a configuration file) and therefore provide
			// different game setting to play against.
			game.Run(new[] {Game.PlayerType.Human, Game.PlayerType.AI},
					 new[] {new Dictionary<ShipType, int> {{ShipType.Destroyer, NUMBER_OF_DESTROYERS}, {ShipType.Battleship, NUMBER_OF_BATTLESHIPS}},
							new Dictionary<ShipType, int> {{ShipType.Destroyer, NUMBER_OF_DESTROYERS}, {ShipType.Battleship, NUMBER_OF_BATTLESHIPS}}});

			// Prevent the console from disappearing when the game is quit - still needed for a .Net Framework project
			Console.WriteLine("<Press any key to continue>");
			Console.ReadKey(true);
		}
	}
}
