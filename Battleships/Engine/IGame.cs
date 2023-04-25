using System.Collections.Generic;
using System.Windows;
using Battleships.Model;

namespace Battleships.Engine {
	/// <summary>
	///  Exposes the public interface of Game class implementation.
	/// </summary>
	public interface IGame {
		/// <summary>
		/// Property that holds the game grid bounds, the top left location of this is at (0, 0) and all game grid cell co-ordinates are in
		/// respect to this with increasing column values going left to right and increasing row values going top to bottom. The game grid
		/// is the playing surface for the game, all player ships are located within the confines of this game grid.
		/// </summary>
		Rect GridBounds {get;}

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
		void Run(Game.PlayerType[] playerType, Dictionary<Ship.ShipType, int>[] playerFleets);

		/// <summary>
		/// Request the game to quit. This method enables other dependent objects to request the game to quit.
		/// </summary>
		/// <returns>Nothing.</returns>
		void Quit();
	}
}