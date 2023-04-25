using System.Windows;

namespace Battleships.Model {
	/// <summary>
	///  Exposes the public interface of Ship class implementation.
	/// </summary>
	public interface IShip {
		/// <summary>
		/// Property that holds the ship type.
		/// </summary>
		Ship.ShipType Type {get;}

		/// <summary>
		/// Property that holds the game grid cell location of the ship.
		/// </summary>
		Point Location {get;}

		/// <summary>
		/// Property that holds the way the ship is oriented across the game grid.
		/// </summary>
		Ship.ShipOrientation Orientation {get;}

		/// <summary>
		/// Property that holds the current health condition of the ship (see <see cref="Battleships.Model.Ship"/>).
		/// </summary>
		int Condition {get;}

		/// <summary>
		/// Property that holds the horizontal and vertical size that the ship occupies on the game grid, in game grid cells. Note, this
		/// size is determined by the orientation of the ship on the game grid.
		/// </summary>
		Size Size {get;}

		/// <summary>
		/// Property that holds the rectangular bounds of the ship within the game grid.
		/// </summary>
		Rect Bounds {get;}

		/// <summary>
		/// Property that holds whether the ship is sunk or not. A sunk ship has a condition of 0.
		/// </summary>
		bool IsSunk {get;}

		/// <summary>
		/// Record a hit on the ship by modifying the condition of the ship at the position on the ship that the hit occurred. Note, the
		/// condition will not change if the hit is at an already fully damaged position, see <see cref="Battleships.Model.Ship"/> for a
		/// detailed treatment of how the condition of the ship is represented and modified. 
		/// </summary>
		/// <param name="position">Position within the bounds of the ship at which the hit should be recorded. A position of 0 means the
		/// hit is recorded at the LSB of the condition property. Recording at other positions are done at the corresponding bit location
		/// in the condition property relative to the LSB, see <see cref="Battleships.Model.Ship"/>.</param>
		/// <returns>Nothing.</returns>
		void RecordAHit(int position);
	}
}