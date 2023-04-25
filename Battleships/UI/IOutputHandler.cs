namespace Battleships.UI {
	/// <summary>
	/// Interface to allow output handlers to be injected into the relevant dependent classes that expect to output some form
	/// of data, (eg. a Battleships.Engine.Player object instance). 
	/// </summary>
	public interface IOutputHandler {
		/// <summary>
		/// Output the supplied message according to the implementation of this method.
		/// </summary>
		/// <param name="message">Message to output.</param>
		/// <returns>Nothing.</returns>
		void Message(string message);
	}
}
