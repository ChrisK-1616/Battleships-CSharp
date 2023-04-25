namespace Battleships.Engine {
	/// <summary>
	/// Manages the "go" undertaken (actioned) by the human player during each "round" of the game.
	/// </summary>
	public interface IGoActioner {
		/// <summary>
		/// The action method that is designed to be called each "round" in the game and acts as the "go" of the player this
		/// dependency has been injected into. 
		/// </summary>
		/// <param name="player">Reference to the player object instance this go actioner dependency has been injected into.</param>
		/// <param name="enemy">Reference to the other player object instance (regarded as "enemies" to each other).</param>
		/// <returns>Nothing.</returns>
		void Action(IPlayer player, IPlayer enemy);
	}
}
