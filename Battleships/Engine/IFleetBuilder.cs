using System.Collections.Generic;
using ShipType = Battleships.Model.Ship.ShipType;

namespace Battleships.Engine {
	/// <summary>
	/// Interface designed to allow a particular fleet builder object instance to be injected into either the human or AI player
	/// objects.
	/// </summary>
	public interface IFleetBuilder {
		/// <summary>
		/// Deploy each of the ships described within the fleetComposition parameter, this parameter is a directory that maps a
		/// ship type to a number of those types of ships within the player's fleet.  
		/// </summary>
		/// <param name="game">Reference to a game object instance.</param>
		/// <param name="player">Reference to the player object instance this builder dependency has been injected into.</param>
		/// <param name="fleetComposition">Reference to dictionary object instance that holds the ship type to quantity mappings.</param>
		/// <returns>bool, true if the fleet build has been successful, false if not.</returns>
		bool Build(IGame game, IPlayer player, Dictionary<ShipType, int> fleetComposition);
	}
}
