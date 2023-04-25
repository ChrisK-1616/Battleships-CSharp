using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Battleships.Model;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace Battleships.Engine {
	/// <summary>
	/// <para>Represents a player in the game, can be human or AI player.</para>
	/// <para> </para>
	/// <para>The way a player builds their fleet and then operate through the "rounds" of the game are determined by dependencies injected
	/// into the object at its construction (see <see cref="Battleships.Engine.Player.FleetBuilder"/> and 
	/// <see cref="Battleships.Engine.Player.GoActioner"/> properties).</para>
	/// <para> </para>
	/// <para>Also, the way the player object interacts with the actual game player (either human or AI) is determined by the output and input
	/// handler dependencies injected in at construction (see <see cref="Battleships.Engine.Player.OutputHandler"/> and 
	/// <see cref="Battleships.Engine.Player.InputHandler"/> properties).</para>
	/// <para> </para>
	/// <para>This Player class also holds the fleet of ships that are deployed around the game grid and an audit list of the attacks the player
	/// has made throughout the operation of the game.</para>
	/// </summary>
	public class Player : IPlayer {
		/// <summary>
		/// Property that holds the list of ships in the player's fleet, this fleet of ships is encapsulated within an object instance of the
		/// Fleet class.
		/// </summary>
		public IFleet Fleet {get; private set;}

		/// <summary>.
		/// Property that holds a reference to the dependency responsible for building the player's fleet.
		/// </summary>
		public IFleetBuilder FleetBuilder {get; private set;}

		/// <summary>
		/// Property that holds a reference to the dependency responsible for handling output from the player's actions.
		/// </summary>
		public IOutputHandler OutputHandler {get; private set;}

		/// <summary>
		/// Property that holds a reference to the dependency responsible for handling input by the player.
		/// </summary>
		public IInputHandler InputHandler {get; private set;}

		/// <summary>
		/// Property that holds a reference to the dependency responsible for actioning each "go" undertaken by the player as the game operates
		/// its "rounds".
		/// </summary>
		public IGoActioner GoActioner {get; private set;}

		/// <summary>
		/// Property that holds a list of the attacks that the player has made throughout the game so far, this is stored as a list of strings
		/// where each string instance shows the game grid location of the attack and the result of the attack (either a miss using the '_' character
		/// or a hit using the '*' character. For example, an attack made on the game grid at location "C5" that hit an enemy ship positioned within
		/// this cell would be recorded as "C5*", a miss would be recorded as "C5_".
		/// </summary>
		private readonly List<string> attacks;

		/// <summary>
		/// Public accessor for the list of attacks held by this player, this is provided as a copy of the original list to prevent it being able to
		/// be changed by the receiver of the reference.
		/// </summary>
		public List<string> Attacks {
			get {
				return attacks.Select(i => i).ToList();
			}
		}

		/// <summary>
		/// Public accessor that indicates if the whole fleet has been sunk, interrogates the AreAllShipsSunk property of the fleet object instance.  
		/// </summary>
		public bool IsFleetSunk {
			get {
				return Fleet.AreAllShipsSunk;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fleet">Reference to dependency that is the player's fleet.</param>
		/// <param name="fleetBuilder">Reference to dependency resposible for building the player's fleet.</param>
		/// <param name="outputHandler">Reference to dependency resposible for handling output from the player's actions.</param>
		/// <param name="inputHandler">Reference to the dependency responsible for handling input by the player.</param>
		/// <param name="goActioner">Reference to the dependency responsible for actioning each "go" undertaken by the player as the game operates
		/// its "rounds".</param> 
		public Player(IFleet fleet, IFleetBuilder fleetBuilder, IOutputHandler outputHandler, IInputHandler inputHandler, IGoActioner goActioner) {
			Fleet = fleet;
			FleetBuilder = fleetBuilder;
			OutputHandler = outputHandler;
			InputHandler = inputHandler;
			GoActioner = goActioner;

			attacks = new List<string>();
		}

		/// <summary>
		/// Adds a ship to the player's fleet.
		/// </summary>
		/// <param name="type">Type of ship to add to the player's fleet.</param>
		/// <param name="location">Game grid location of the ship being added to the player's fleet.</param>
		/// <param name="orientation">Orientation of the ship being added to the player's fleet.</param>
		/// <returns>Nothing.</returns>
		public void AddShipToFleet(ShipType type, Point location, ShipOrientation orientation) {
			Fleet.AddShip(new Ship(type, location, orientation));
		}

		/// <summary>
		/// Adds the information associated with an attack to the audit list of previous attacks undertaken by this player. 
		/// </summary>
		/// <param name="attackInfo">Attacked game grid location in the form "A1_", "E10*" etc to be added to the audit list of previous
		/// attacks.</param>
		/// <returns>Nothing.</returns>
		public void AddAttack(string attackInfo) {
			attacks.Add(attackInfo);
		}

		/// <summary>
		/// Helper method that will stringify an attack location and whether that resulted in a hit into attack information for adding to audit
		/// list of previous attacks. 
		/// </summary>
		/// <param name="location">Game grid location of the attack.</param>
		/// <param name="wasHit">Did the attack result in a hit or miss?</param>
		/// <returns>System.String reference, stringified version of the supplied attack information.</returns>
		public static string StringifyAttackInfo(Point location, bool wasHit) {
			return String.Format("{0}{1}", UIHelper.StringifyLocation(location), wasHit ? "*" : "_");
		}
	}
}
