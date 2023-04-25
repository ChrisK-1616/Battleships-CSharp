using System;
using System.Collections.Generic;
using System.Windows;
using Battleships.Model;
using Battleships.UI;
using ShipType = Battleships.Model.Ship.ShipType;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace Battleships.Engine {
	/// <summary>
	/// Designed to deploy a fleet of ships in the game for the artificial intelligence (AI) player. This employs a very simplistic fleet
	/// deploying algorithm, it randomly selects locations and orientations of ships until they are all deployed within the game grid and
	/// do not overlap each other. 
	/// </summary>
	public class AIRandomFleetBuilder : IFleetBuilder {
		/// <summary>
		/// Reference to a System.Random object instance supplied during instantiation of objects of this class.
		/// </summary>
		private readonly Random rng; // Random number generator (RNG)

		/// <summary>
		/// Constructor, this version initialises the rng property with a System.Random object instance.
		/// </summary>
		public AIRandomFleetBuilder() {
			rng = new Random();
		}

		/// <summary>
		/// Constructor, this version initialises the rng property with a supplied object instance which can be or derive from an instance of
		/// the System.Random class.
		/// </summary>
		/// <param name="rng">Reference to System.Random object instance or to an instance of a class derived from System.Random</param>
		public AIRandomFleetBuilder(Random rng) {
			this.rng = rng;
		}

		/// <summary>
		/// Method tasked with determining a valid random location and random orientation when deploying a single ship within the game grid.
		/// </summary>
		/// <param name="game">Reference to a game object instance.</param>
		/// <param name="player">Reference to the player object instance associated with the AI.</param>
		/// <param name="shipSize">Size of the ship being deployed, in game grid cells.</param>
		/// <returns>Battleships.Model.Deployment struct reference, represents the location in the game grid and the orientation across the
		/// game grid of the deployed ship.</returns>
		private Deployment GetValidDeployment(IGame game, IPlayer player, int shipSize) {
			// Convenience locals
			var shipSizeH = new Size(shipSize - 1, 0);
			var shipSizeV = new Size(0, shipSize - 1);
			var xLimitH = (int)game.GridBounds.Width - shipSize;
			var xLimitV = (int)game.GridBounds.Width;
			var yLimitH = (int)game.GridBounds.Height;
			var yLimitV = (int)game.GridBounds.Height - shipSize;
			var deployment = new Deployment {Location = new Point(0, 0), Orientation = ShipOrientation.Horizontal};

			// Randomly select ship orientation
			deployment.Orientation = ((rng.Next() % 2) == 0) ? ShipOrientation.Horizontal : ShipOrientation.Vertical;
			
			// Randomly select a location within the game grid that fits for the given size and orientation of the ship
			deployment.Location = new Point(rng.Next((deployment.Orientation == ShipOrientation.Horizontal) ? xLimitH : xLimitV),
											rng.Next((deployment.Orientation == ShipOrientation.Horizontal) ? yLimitH : yLimitV));
			var shipBounds = new Rect(deployment.Location, (deployment.Orientation == ShipOrientation.Horizontal) ? shipSizeH : shipSizeV);

			// Re-select orientation and location of the ship if those currently selected make the ship clash with an existing ship already
			// deployed in the game grid 
			while(true) {
				if(!player.Fleet.DoesShipBoundsClash(shipBounds)) {
					// Does not clash, so return passing back the selected location and orientation
					return deployment;
				}

				// Re-select orientation and location of the ship
				deployment.Orientation = ((rng.Next() % 2) == 0) ? ShipOrientation.Horizontal : ShipOrientation.Vertical;
				deployment.Location = new Point(rng.Next((deployment.Orientation == ShipOrientation.Horizontal) ? xLimitH : xLimitV),
														 rng.Next((deployment.Orientation == ShipOrientation.Horizontal) ? yLimitH : yLimitV));
				shipBounds.Location = deployment.Location;
				shipBounds.Size = (deployment.Orientation == ShipOrientation.Horizontal) ? shipSizeH : shipSizeV;
			}
		}

		/// <summary>
		/// Establish a suitable orientation and location for the ship being deployed and then add it to the AI player fleet.
		/// </summary>
		/// <param name="game">Reference to a game object instance.</param>
		/// <param name="player">Reference to the player object instance associated with the AI.</param>
		/// <param name="shipType">Type of the ship being deployed.</param>
		/// <returns>Nothing.</returns>
		private void AddShipToFleet(IGame game, IPlayer player, ShipType shipType) {
			var deployment = GetValidDeployment(game, player, (int)shipType);

#if DEBUG
			// Cheat when debugging, this shows the type, orientation and location of all ships added to the AI fleet
			player.OutputHandler.Message(String.Format("Type: {0} Orientation: {1} Location: {2}\n\n", shipType, deployment.Orientation,
													   UIHelper.StringifyLocation(deployment.Location)));
#endif // DEBUG

			// Add ship to AI player's fleet
			player.AddShipToFleet(shipType, deployment.Location, deployment.Orientation);
		}

		/// <summary>
		/// Deploy each of the ships described within the fleetComposition parameter, this parameter is a directory that maps a ship type
		/// to a number of those types of ships within the AI player's fleet.  
		/// </summary>
		/// <param name="game">Reference to a game object instance.</param>
		/// <param name="player">Reference to the player object instance associated with the AI.</param>
		/// <param name="fleetComposition">Reference to dictionary object instance that holds the ship type to quantity mappings.</param>
		/// <returns>bool, true if the AI fleet build has been successful, false if not.</returns>
		public bool Build(IGame game, IPlayer player, Dictionary<ShipType, int> fleetComposition) {
			// Sanity check to ensure that there are valid object instances
			if(ReferenceEquals(null, game) || ReferenceEquals(null, player) || ReferenceEquals(null, fleetComposition)) {
				// Not able to build, return false
				return false;
			}

			// Iterate over the ship types from the fleet composition
			foreach(var shipType in fleetComposition.Keys) {
				// Use the quantity mapped to the ship type to add requisite ships to the AI fleet 
				for(var i = 0; i < fleetComposition[shipType]; i++) {
					AddShipToFleet(game, player, shipType);
				}
			}

			// Build successful, return true
			return true;
		}
	}
}
