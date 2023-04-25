using System;

namespace Battleships.UI {
	/// <summary>
	/// Simple console device output handler.
	/// </summary>
	public class ConsoleBasedOutputHandler : IOutputHandler {
		/// <summary>
		/// Write the supplied message to the console device using System.Console.Write() method.
		/// </summary>
		/// <param name="message">Message to output onto the console device.</param>
		/// <returns>Nothing.</returns>
		public void Message(string message) {
			Console.Write(message);
		}
	}
}
