using System;
using System.Windows;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace Battleships.UI {
	/// <summary>
	/// Simplest form of AI when determining the input from an automated game "go" action. The algorithm is to simply establish a
	/// valid random game grid cell location to attack. This algorithm takes no notice of any previous attack results and can therefore
	/// attack the same cell many times (which is actually a rather poor strategy for the game of Battleships).
	/// </summary>
	public class AISimpleRandomInputHandler : IInputHandler {
		/// <summary>
		/// Reference to a System.Random object instance supplied during instantiation of objects of this class.
		/// </summary>
		private readonly Random rng; // Random number generator (RNG)

		/// <summary>
		/// Property that holds the game grid size so the algorithm used in the GetInput() method can establish a valid game grid cell
		/// location.
		/// </summary>
		private readonly Size gridSize;

		/// <summary>
		/// Constructor, this version initialises the rng property with a System.Random object instance.
		/// </summary>
		/// <param name="gridSize">Game grid size for use in the GetInput() algorithm.</param> 
		public AISimpleRandomInputHandler(Size gridSize) {
			this.rng = new Random();
			this.gridSize = gridSize;
		}

		/// <summary>
		/// Constructor, this version initialises the rng property with a supplied object instance which can be or derive from an instance of
		/// the System.Random class.
		/// </summary>
		/// <param name="rng"></param>
		/// <param name="gridSize">Game grid size for use in the GetInput() algorithm.</param> 
		public AISimpleRandomInputHandler(Random rng, Size gridSize) {
			this.rng = rng;
			this.gridSize = gridSize;
		}

		/// <summary>
		/// Randomly forms an AI attack command by instantiating an Battleships.UI.InputCommand which is given a command type value of 
		/// Battleships.UI.InputCommand.CommandType.Location and which has it's data set as the randomly determined game grid cell location. 
		/// </summary>
		/// <returns>Battleships.UI.InputCommand struct reference, consists of a Battleships.UI.InputCommandCommandType.Location and randomly
		/// determined game grid cell location as it's data.</returns>
		public InputCommand GetInput() {
			// Randomly determine the game grid cell location
			var location = new Point(rng.Next((int)gridSize.Width - 1), rng.Next((int)gridSize.Height - 1));

			// Instantiate an appropriate InputCommand strut instance
			return new InputCommand {Type = CommandType.Location, Data = UIHelper.StringifyLocation(location)};
		}
	}
}
