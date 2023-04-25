namespace Battleships.UI {
	/// <summary>
	/// Encapsulates input as an input command type and associated data.
	/// </summary>
	public struct InputCommand {
		/// <summary>
		/// Type of the command.
		/// </summary>
		public enum CommandType {
			/// <summary>
			/// Catch all for when the input is not defined or invalid, the data is not relevant.
			/// </summary>
			None = 0,

			/// <summary>
			/// Input is a request for help to be displayed, the data is not relevant.
			/// </summary>
			HelpRequest,
			
			/// <summary>
			/// Input is a ship orientation, data will be "h" or "H" for horizontal orientation or "v" or "V" for vertical orientation. 
			/// </summary>
			Orientation,
			
			/// <summary>
			/// Input is a location in the the game grid, data is a grid location in the form letter and number, eg. "A1", "E10" etc.
			/// </summary>
			Location,
			
			/// <summary>
			/// Input is a request to show audit list of previous attack, the data will be "!" when requesting list of player attacks and
			/// '*' when requesting list of enemy attacks.
			/// </summary>
			ShowAttacks,
			
			/// <summary>
			/// Input is a request to quit the game immediately, the data is not relevant.
			/// </summary>
			Quit
		}

		/// <summary>
		/// Property holding the command type.
		/// </summary>
		public CommandType Type {get; set;}

		/// <summary>
		/// Property holding the data. When using this data, the actual meaning of the data's value is dependent on the command type.
		/// </summary>
		public string Data {get; set;}
	}
}
