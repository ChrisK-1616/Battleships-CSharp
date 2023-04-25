using System;
using System.Windows;
using Battleships.UI;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace Battleships.Engine {
	/// <summary>
	/// Manages the "go" undertaken (actioned) by the human player during each "round" of the game.
	/// </summary>
	public class HumanGoActioner : IGoActioner {
		/// <summary>
		/// Property holding reference to the Game object instance
		/// </summary>
		public IGame Game {get; set;}

		/// <summary>
		/// The action method that is designed to be called each "round" in the game and acts as the "go" of the AI. 
		/// </summary>
		/// <param name="player">Reference to the player object instance associated with the human.</param>
		/// <param name="enemy">Reference to the AI player object instance.</param>
		/// <returns>Nothing.</returns>
		public void Action(IPlayer player, IPlayer enemy) {
			while(true) {
				// Ask the human player to input their "go" for this game "round". This can be either a command to show a help message,
				// to show an audit of all the attacks the human player has made so far, an audit of all the attacks the AI player has
				// made so far, to quit the game immediately or a game grid location (in the form "A1", "E10" etc) which is to be subject
				// to attack 
				player.OutputHandler.Message("XO to Captain, sir, what is your command?\n");
				player.OutputHandler.Message("(Enter X or x for help): ");
				var inputCommand = player.InputHandler.GetInput();

				// Action the input command from the human player
				switch(inputCommand.Type) {
					// Request a help message 
					case CommandType.HelpRequest: {
						player.OutputHandler.Message("  !         - Show your previous attacks on the enemy fleet\n");
						player.OutputHandler.Message("  *         - Show the enemy's previous attacks on your fleet\n");
						player.OutputHandler.Message("  X or x    - Quit game immediately\n");
						player.OutputHandler.Message("  ?         - Show help (what you are seeing now)\n");
						player.OutputHandler.Message("[A-J][1-10] - Grid location to attack (eg. A5, E10)\n\n");
						break;
					}

					// Show audit of all attacks made either by the human or the AI player
					case CommandType.ShowAttacks: {
						if(inputCommand.Data == "!") {
							// Has there been any attacks made by the human player yet?
							if(player.Attacks.Count == 0) {
								player.OutputHandler.Message("XO to Captain, sir, we have not made any attacks yet...\n\n");
							}
							else {
								// Show the audit of human attacks
								player.OutputHandler.Message("XO to Captain, sir, our attacks so far:-\n");
								player.Attacks.ForEach(a => player.OutputHandler.Message(String.Format("{0} ", a)));
								player.OutputHandler.Message("\n\n");
							}
						}
						else {
							// Has there been any attacks made by the AI player yet?
							if(enemy.Attacks.Count == 0) {
								player.OutputHandler.Message("XO to Captain, sir, we have not been attacked yet...\n\n");
							}
							else {
								// Show the audit of AI attacks
								player.OutputHandler.Message("XO to Captain, sir, the enemy's attacks so far:-\n");
								enemy.Attacks.ForEach(a => player.OutputHandler.Message(String.Format("{0} ", a)));
								player.OutputHandler.Message("\n\n");
							}
						}

						break;
					}

					// A location to attack has been input, as a string in the form "A1", "E10" etc.
					case CommandType.Location: {
						Point location;

						try {
							// Parse this location string into a Point struct form
							location = UIHelper.ParseLocationInput(inputCommand.Data);
						}
						catch {
							// An invalid location string was input, tell the human player this and do not process it
							player.OutputHandler.Message("XO to Captain, sir, this is not a valid grid co-ordinate...\n\n");
							break;
						}

						// Sanity check for valid Game object instance
						if(ReferenceEquals(null, Game)) {
							// No valid Game object instance, so abort
							return;
						}
						
						// Is the location attacked within the game grid?
						if(!Game.GridBounds.Contains(location)) {
							// Not inside the game grid, tell the human player this and do not process the attack
							player.OutputHandler.Message("XO to Captain, sir, grid co-ordinate is not within range our guns...\n\n");
							break;
						}

						// Use the location input to see if it has hit a ship in the AI fleet, a null is returned by Fleet.CheckForAndRecordAnyHit()
						// if the attack missed, otherwise a reference to the object instance of the hit AI ship is returned
						var shipHit = enemy.Fleet.CheckForAndRecordAnyHit(location);
						var wasHit = !ReferenceEquals(null, shipHit); // If a valid Ship reference is returned, then a hit was scored
						// Add this attack to the audit list of attacks held in the human player object
						player.AddAttack(Player.StringifyAttackInfo(location, wasHit));

						// Output details of any hit or miss, and if a hit was scored then this may also have sunk the affected ship. If it was sunk
						// then articulate this
						player.OutputHandler.Message(wasHit ? String.Format("Gunnery to Captain, sir, we scored a hit on an enemy ship{0}!\n\n", shipHit.IsSunk ?
													 " and it sank" : String.Empty) : "Gunnery to Captain, sir, our attack missed...\n\n");

						return;
					}

					// Request to quit immediately
					case CommandType.Quit: {
						// After sanity check for valid Game object instance, indicate to the Game object instance that it should quit
						if(!ReferenceEquals(null, Game)) {
							Game.Quit();
						}

						return;
					}

					// Not a valid input
					default: {
						player.OutputHandler.Message("XO to Captain, sir, invalid command! Please repeat...\n");
						player.OutputHandler.Message("(enter ? for help)\n\n");
						break;
					}
				}
			}
		}
	}
}
