using System.Windows;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace Battleships.Model {
	/// <summary>
	/// Holds a ship orientation and ship location pairing
	/// </summary>
	public struct Deployment {
		/// <summary>
		/// Ship orientation, either Horizontal or vertical
		/// </summary>
		public ShipOrientation Orientation {get; set;}

		/// <summary>
		/// Location of the ship in the game grid using a Point struct
		/// </summary>
		public Point Location {get; set;}
	}
}
