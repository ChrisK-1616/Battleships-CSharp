using System;
using System.Collections.Generic;
using System.Windows;
using Battleships.Model;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace Battleships.Engine {
	/// <summary>
	/// Concrete class designed to all the human player to deploy a fleet of ships in the game. This involves the human player
	/// selecting locations and orientations of ships until they deploy all their ships within the game grid without any ships
	/// overlapping each other. 
	/// </summary>
	public class HumanFleetBuilder : IFleetBuilder {
		/// <summary>
		/// Method tasked with deploying a single ship within the game grid.
		/// </summary>
		/// <param name="game">Reference to a game object instance.</param>
		/// <param name="player">Reference to the player object instance associated with the human.</param>
		/// <param name="shipType">Type of the ship being deployed.</param>
		/// <param name="ordinal">The current count number of this type of ship</param>
		/// <returns>Battleships.Model.Deployment struct nullable reference, returns a null if the human player has
		/// asked the game to quit immediately, otherwise return the valid Battleships.Model.Deployment struct instance.</returns>
		private static Deployment? GetValidDeployment(IGame game, IPlayer player, ShipType shipType, int ordinal) {
			var shipBounds = new Rect();
			var shipSizeH = new Size((int)shipType - 1, 0);
			var shipSizeV = new Size(0, (int)shipType - 1);

			while(true) {
				Point shipLocation;
				ShipOrientation shipOrientation;

				// Ask the human player to enter the location and orientation of the ship being deployed
				player.OutputHandler.Message(String.Format("Helm to Captain, sir, give me grid co-ordinate (E4, D10 etc) and orientation (using h or v) for {0}{1}\n", shipType,
											 ordinal));
				player.OutputHandler.Message("(enter X or x to quit the game immediately)\n");

				// Input location
				player.OutputHandler.Message("Grid co-ordinate: ");
				var inputCommand = player.InputHandler.GetInput();
				// Needs to be a location that is input
				if(inputCommand.Type != CommandType.Location) {
					// If not, then is it a quit command?
					if(inputCommand.Type == CommandType.Quit) {
						// Quit command, so return null to indicate this
						return null;
					}

					player.OutputHandler.Message("Helm to Captain, sir, this is not a valid grid co-ordinate!\n\n");
					// Otherwise ask for the location again
					continue;
				}

				try {
					// It is a location, but is it valid? The UIHelper.ParseLocationInput() method has the role of either parsing a valid
					// location or throwing an exception if the location is invalid
					shipLocation = UIHelper.ParseLocationInput(inputCommand.Data);
				}
				catch {
					// Not valid, so ask for it to be input again
					player.OutputHandler.Message("Helm to Captain, sir, this is not a valid grid co-ordinate!\n\n");
					continue;
				}

				// Input orientation
				player.OutputHandler.Message("Orientation: ");

				inputCommand = player.InputHandler.GetInput();
				// Needs to be an orientation that is input
				if(inputCommand.Type != CommandType.Orientation) {
					// If not, then is it a quit command?
					if(inputCommand.Type == CommandType.Quit) {
						// Quit command, so return null to indicate this
						return null;
					}

					player.OutputHandler.Message("Helm to Captain, sir, this is not a valid ship orientation!\n\n");
					// Otherwise ask for the orientation again
					continue;
				}

				try {
					// It is an orientation, but is it valid? The UIHelper.ParseOrientationInput() method has the role of either parsing
					// a valid orientation or throwing an exception if the orientation is invalid
					shipOrientation = UIHelper.ParseOrientationInput(inputCommand.Data);
				}
				catch {
					player.OutputHandler.Message("Helm to Captain, sir, this is not a valid ship orientation!\n\n");
					// Not valid, so ask for it to be input again
					continue;
				}

				// Now check that the input orientation and location (ie. the deployment pair) are inside the game grid and not overlapping
				// an existing ship in the fleet
				shipBounds.Location = shipLocation;
				shipBounds.Size = (shipOrientation == ShipOrientation.Horizontal) ? shipSizeH : shipSizeV;

				// Check against the game grid bounds
				if(!game.GridBounds.Contains(shipBounds)) {
					player.OutputHandler.Message("Helm to Captain, sir, ship orientation and grid co-ordinate does not fit into battle area!\n\n");
					// Not inside the game grid, so ask to input again
					continue;
				}

				// Check against overlap of existing ships
				if(player.Fleet.DoesShipBoundsClash(shipBounds)) {
					player.OutputHandler.Message("Helm to Captain, sir, ship orientation and grid co-ordinate rams an existing ship!\n\n");
					// Overlaps an existing ship, so ask to input again
					continue;
				}

				// Valid orientation and location provided
				player.OutputHandler.Message(String.Format("Helm to Captain, {0}{1} deployed sir.\n\n", shipType, ordinal));

				// Return this as a Deployment struct instance
				return new Deployment {Location = shipLocation, Orientation = shipOrientation};
			}
		}

		/// <summary>
		/// Input a suitable orientation and location for the ship being deployed and then add it to the human player fleet.
		/// </summary>
		/// <param name="game">Reference to a game object instance.</param>
		/// <param name="player">Reference to the player object instance associated with the human.</param>
		/// <param name="shipType">Type of the ship being deployed.</param>
		/// <param name="ordinal">The current count number of this type of ship</param>
		/// <returns>bool, return true if the ship was added to the fleet, otherwise return false to indicate that the human player has requested
		/// to quit the game immediately.</returns>
		private static bool AddShipToFleet(IGame game, IPlayer player, ShipType shipType, int ordinal) {
			var shipDeployment = GetValidDeployment(game, player, shipType, ordinal);

			// Is the Deployment struct instance valid?
			if(!shipDeployment.HasValue) {
				// If not then a request to quit immediately has occurred, return false
				return false;
			}

			// Add ship to human fleet
			player.AddShipToFleet(shipType, shipDeployment.Value.Location, shipDeployment.Value.Orientation);

			// Successfully added ship, return true
			return true;
		}


		/// <summary>
		/// Deploy each of the ships described within the fleetComposition parameter, this parameter is a directory that maps a ship type to a number
		/// of those types of ships within the human player's fleet.  
		/// </summary>
		/// <param name="game">Reference to a game object instance.</param>
		/// <param name="player">Reference to the player object instance associated with the human.</param>
		/// <param name="fleetComposition">Reference to dictionary object instance that holds the ship type to quantity mappings.</param>
		/// <returns>bool, true if the human fleet build has been successful, false if not.</returns>
		public bool Build(IGame game, IPlayer player, Dictionary<ShipType, int> fleetComposition) {
			if(ReferenceEquals(null, game) || ReferenceEquals(null, player) || ReferenceEquals(null, fleetComposition)) {
				return false;
			}

			player.OutputHandler.Message("Helm to Captain, sir, where shall we deploy the fleet?\n\n");

			foreach(var shipType in fleetComposition.Keys) {
				for(var i = 0; i < fleetComposition[shipType]; i++) {
					if(!AddShipToFleet(game, player, shipType, i + 1)) {
						return false;
					}
				}
			}

			player.OutputHandler.Message("Radar Room to Captain, sir, enemy detected!\n\n");

			return true;
		}
	}
}
