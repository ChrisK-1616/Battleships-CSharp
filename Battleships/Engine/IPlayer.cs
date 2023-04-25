using System.Collections.Generic;
using System.Windows;
using Battleships.Model;
using Battleships.UI;

namespace Battleships.Engine {
	/// <summary>
	///  Exposes the public interface of the Player class implementation.
	/// </summary>
	public interface IPlayer {
		/// <summary>
		/// Property that holds the list of ships in the player's fleet, this fleet of ships is encapsulated within an object instance of the
		/// Fleet class.
		/// </summary>
		IFleet Fleet {get;}

		/// <summary>.
		/// Property that holds a reference to the dependency responsible for building the player's fleet.
		/// </summary>
		IFleetBuilder FleetBuilder {get;}

		/// <summary>
		/// Property that holds a reference to the dependency responsible for handling output from the player's actions.
		/// </summary>
		IOutputHandler OutputHandler {get;}

		/// <summary>
		/// Property that holds a reference to the dependency responsible for handling input by the player.
		/// </summary>
		IInputHandler InputHandler {get;}

		/// <summary>
		/// Property that holds a reference to the dependency responsible for actioning each "go" undertaken by the player as the game operates
		/// its "rounds".
		/// </summary>
		IGoActioner GoActioner {get;}

		/// <summary>
		/// Public accessor for the list of attacks held by this player, this is provided as a copy of the original list to prevent it being able to
		/// be changed by the receiver of the reference.
		/// </summary>
		List<string> Attacks {get;}

		/// <summary>
		/// Public accessor that indicates if the whole fleet has been sunk, interrogates the AreAllShipsSunk property of the fleet object instance.  
		/// </summary>
		bool IsFleetSunk {get;}

		/// <summary>
		/// Adds a ship to the player's fleet.
		/// </summary>
		/// <param name="type">Type of ship to add to the player's fleet.</param>
		/// <param name="location">Game grid location of the ship being added to the player's fleet.</param>
		/// <param name="orientation">Orientation of the ship being added to the player's fleet.</param>
		/// <returns>Nothing.</returns>
		void AddShipToFleet(Ship.ShipType type, Point location, Ship.ShipOrientation orientation);

		/// <summary>
		/// Adds the information associated with an attack to the audit list of previous attacks undertaken by this player. 
		/// </summary>
		/// <param name="attackInfo">Attacked game grid location in the form "A1_", "E10*" etc to be added to the audit list of previous
		/// attacks.</param>
		/// <returns>Nothing.</returns>
		void AddAttack(string attackInfo);
	}
}
