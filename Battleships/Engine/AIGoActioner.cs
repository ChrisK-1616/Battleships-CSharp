using System;
using Battleships.UI;

namespace Battleships.Engine {
	/// <summary>
	/// Manages the "go" undertaken (actioned) by the artificial intelligence (AI) during each "round" of the game.
	/// </summary>
	public class AIGoActioner : IGoActioner {
		/// <summary>
		/// The action method that is designed to be called each "round" in the game and acts as the "go" of the AI. 
		/// </summary>
		/// <param name="player">Reference to the player object instance associated with the AI.</param>
		/// <param name="enemy">Reference to the human player object instance.</param>
		/// <returns>Nothing.</returns>
		public void Action(IPlayer player, IPlayer enemy) {
			// Get the location to be attacked as determined by the current AI InputHandler's GetInput() method
			var attackedLocation = player.InputHandler.GetInput().Data;
			var location = UIHelper.ParseLocationInput(attackedLocation);

			// Check to see if the AI's "go" has resulted in a hit on any of the human player's ships
			var shipHit = enemy.Fleet.CheckForAndRecordAnyHit(location);
			var wasHit = !ReferenceEquals(null, shipHit); // If a valid Ship reference is returned, then a hit was scored
 			// Add this attack to the audit list of attacks held in the AI player object 
			player.AddAttack(Player.StringifyAttackInfo(location, wasHit));

			// Output details of any hit or miss, and if a hit was scored then this may also have sunk the affected ship. If it was sunk
			// then articulate this
			player.OutputHandler.Message(wasHit ? String.Format("AI scored a hit at {0} on enemy {1}{2}!\n\n", attackedLocation, shipHit.Type,
										 shipHit.IsSunk ? " and sank it" : String.Empty) : String.Format("AI attack at {0} missed...\n\n",
										 attackedLocation));
		}
	}
}
