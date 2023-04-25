namespace Battleships.UI {
	/// <summary>
	/// Interface to allow input handlers to be injected into the relevant dependent classes that expect to receive some form
	/// of data input, (eg. a Battleships.Engine.Player object instance). 
	/// </summary>
	public interface IInputHandler {
		/// <summary>
		/// This receives an input. Return value is encapsulated within a Battleship.UI.InputCommand struct reference, the form
		/// of which is determined by the implementation of this method.
		/// </summary>
		/// <returns>Battleship.UI.InputCommand struct reference, command type and data determined by the way the input is processed.</returns>
		InputCommand GetInput();
	}
}
