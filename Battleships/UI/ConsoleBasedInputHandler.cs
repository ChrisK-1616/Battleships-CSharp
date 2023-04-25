using System;
using System.Text.RegularExpressions;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace Battleships.UI {
	/// <summary>
	/// <para>Uses a console device to read input from the player. This input can form one of three command types:-</para>
	/// <para> </para>
	/// <para>Help request by the player inputing a '?' character</para>
	/// <para>Request to see the audit list of previous attacks made by the player by inputing a '!' character</para>
	/// <para>Request to see the audit list of previous attacks made by the enemy player by inputing a '*' character</para>
	/// <para>Make an attack by inputing a game grid cell location in the letter and number format (eg. "A1", "E10" etc), this must be an upper
	/// case letter A-Z</para>
	/// <para>Request to quit the game immediately by inputing an 'x' or 'X' character</para>
	/// <para> </para>
	/// <para>Each valid input from the player results in an instantiation of an Battleships.UI.InputCommand.InputCommand struct object instance
	/// which has it's type property and data property set as follows:-</para>
	/// <para> </para>
	/// <para>Help request,</para> 
	/// <para>Request for previous player attacks,</para>
	/// <para>Request for previous enemy attacks,</para>
	/// <para>Attack location,</para>
	/// <para>Quit immediately,</para>
	/// <para> </para>
	/// <para>If the input is in anyway invalid, then a command type of Battleships.UI.InputCommand.CommandType.None is returned from the GetInput()
	/// method.</para>
	/// </summary>
	public class ConsoleBasedInputHandler : IInputHandler {
		/// <summary>
		/// This receives input from a console device via System.Console.Readline() blocking method call. Return value is encapsulated within a
		/// Battleship.UI.InputCommand struct reference, the form of which is determined by the ParseInputCommand() return value.
		/// </summary>
		/// <returns>Battleship.UI.InputCommand struct reference, command type and data determined by the way the input is parsed.</returns>
		public InputCommand GetInput() {
			return ParseInputCommand(Console.ReadLine());
		}

		/// <summary>
		/// Parses the input according to the format of the supplied input string, the resultant Battleships.UI.InputCommand struct instance is
		/// formed according to which pattern (through use of System.Text.RegularExpressions.Regex class) the input matches with.
		/// </summary>
		/// <param name="input">Input string that is to be parsed.</param>
		/// <returns>Battleship.UI.InputCommand struct reference, command type and data determined by the way the input is parsed.</returns>
		private static InputCommand ParseInputCommand(string input) {
			const string helpRequestRegex = "^[?]{1}$"; // Request help command
			const string showAttemptsRegex = "^[!,*]{1}$"; // Request audit list of previous attacks command
			const string quitRegex = "^[X,x]{1}$"; // Request quit immediately command

			// Pattern match the input string
			if(Regex.IsMatch(input, helpRequestRegex)) {
				return new InputCommand {Type = CommandType.HelpRequest, Data = input};
			}
			
			if(Regex.IsMatch(input, showAttemptsRegex)) {
				return new InputCommand {Type = CommandType.ShowAttacks, Data = input};
			}
			
			if(Regex.IsMatch(input, quitRegex)) {
				return new InputCommand {Type = CommandType.Quit, Data = input};
			}

			if(UIHelper.CheckLocationInput(input)) {
				return new InputCommand {Type = CommandType.Location, Data = input};
			}

			if(UIHelper.CheckOrientationInput(input)) {
				return new InputCommand {Type = CommandType.Orientation, Data = input};
			}

			// Not a valid input string, so return CommandType.None and empty string as data
			return new InputCommand {Type = CommandType.None, Data = String.Empty};
		}
	}
}
