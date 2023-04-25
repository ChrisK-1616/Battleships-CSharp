using System;
using System.Windows;
using System.Text.RegularExpressions;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace Battleships.UI {
	/// <summary>
	/// Helper class to check and parse ship orientation related input and to check, parse and stringify game grid location related input.
	/// </summary>
	public static class UIHelper {
		/// <summary>
		/// Check a supplied input string to see if this is a valid ship orientation value.
		/// </summary>
		/// <param name="input">Orientation as a string.</param>
		/// <returns>bool, return true if the supplied input is a valid ship orientation ("h", "H", "v" or "V"), false otherwise.</returns>
		public static bool CheckOrientationInput(string input) {
			const string regex = "^[H,V]{1}$";

			return Regex.IsMatch(input.ToUpper(), regex);
		}

		/// <summary>
		/// <para>Parse the supplied input string into a Battleships.Model.Ship.ShipOrientation value according to:-</para>
		/// <para> </para>
		/// <para>Input string is "h" or "H", map to Battleships.Model.Ship.ShipOrientation.Horizonal.</para>
		/// <para>Input string is "v" or "V", map to Battleships.Model.Ship.ShipOrientation.Vertical.</para>
		/// </summary>
		/// <param name="input">Input orientation as a string.</param>
		/// <returns>Battleships.Model.Ship.ShipOrientation enum value.</returns>
		/// <exception cref="System.FormatException">Throws a System.FormatException if the supplied string is not a valid ship orientation.</exception>
		public static ShipOrientation ParseOrientationInput(string input) {
			if(!CheckOrientationInput(input)) {
				throw new FormatException(String.Format("Orientation input: {0} is not valid", input));
			}

			return ((input.ToUpper() == "H") ? ShipOrientation.Horizontal : ShipOrientation.Vertical);
		}

		/// <summary>
		/// Check a supplied input string to see if this is a valid game grid location value. Note, this only checks for the format of the
		/// supplied string, not whether the location is actually contained within the bounds of the game grid (this would have to be done
		/// separately).
		/// </summary>
		/// <param name="input">Game grid location as a string.</param>
		/// <returns>bool, return true if the supplied input is a valid game grid location (letter and number, eg. "A1", "E10" etc), false
		/// otherwise.</returns>
		public static bool CheckLocationInput(string input) {
			const string regex = "^[A-Z][0-9]{1,2}$";

			return Regex.IsMatch(input, regex);
		}

		/// <para>Parse the supplied input string into a System.Windows.Point value according to:-</para>
		/// <para> </para>
		/// <para>Letter (A-Z) maps to System.Point.X value where "A" => 0, "B" => 1 ... "Z" => 25</para>
		/// <para>Number maps to System.Point.Y, with the value mapped being one less than that supplied</para>
		/// <param name="input">Input location as a string in form letter and number (eg. "A1", "E10" etc).</param>
		/// <para> </para>
		/// <para>The System.Point.X represents the "column" number in the game grid, the System.Point.Y represents the "row" number in the 
		/// game grid.</para>
		/// <returns>System.Windows.Point struct reference.</returns>
		/// <exception cref="System.FormatException">Throws a System.FormatException if the supplied string is not a valid game grid location.</exception>
		public static Point ParseLocationInput(string input) {
			if(!CheckLocationInput(input)) {
				throw new FormatException(String.Format("Location input: {0} is not valid", input));
			}

			return new Point(input[0] - 'A', Int32.Parse(input.Substring(1)) - 1);
		}

		/// <summary>
		/// <para>Takes a game grid location as a System.Point struct reference and stringifies it into a letter and number representation
		/// where:-</para>
		/// <para> </para>
		/// <para>System.Point.X value maps to the letter according to 0 => "A", 1 => "B" ... 25 => "Z"</para>
		/// <para>(System.Point.Y value plus one maps to the number</para>
 		/// </summary>
		/// <param name="location">Game grid location supplied as a System.Point struct reference.</param>
		/// <returns>System.String reference.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Throws System.ArgumentOutOfRangeException if the System.Point.X or System.Point.Y
		/// values are negative, or if the System.Point.X is greater than 25 (this allows A-Z letter values)</exception>
		public static string StringifyLocation(Point location) {
			if(location.X < 0 || location.Y < 0 || location.X > 25) {
				throw new ArgumentOutOfRangeException(String.Format("location input: {0} has negative values or X value greater than 25, which makes it invalid",
													  location));
			}

			return String.Format("{0}{1}", Char.ConvertFromUtf32('A' + (int)location.X), (int)location.Y + 1);
		}
	}
}
