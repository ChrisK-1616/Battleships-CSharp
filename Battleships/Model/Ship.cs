using System.Windows;

namespace Battleships.Model {
	/// <summary>
	/// <para>Represents a ship object instance in the game. Ships are added to each player object to form their fleets.</para>
	/// <para> </para>
	/// <para>Each ship is defined by a type, an orientation and a location on the game grid. The type also defines the size of the
	/// ship in game grid cells. This relationship of type to size is achieved using the ShipType enum were each enum entry uses its
	/// ordinal to describe its size. This is a somewhat fragile implementation if more ship types were to be introduced (with ships
	/// of the same size having to be of the same ordinal in the enum), but for just two distinct ships and sizes it works.</para>
	/// <para> </para>
	/// <para>The ship orientation is relative to the game grid, with a horizontally oriented ship occupying cells from its location
	/// across cell columns to the right for the size of the ship, for example, a Destroyer ship located at cell E3 and oriented horizontally
	/// will occupy cells E3, F3, G3 and H3. A vertically oriented ship occupies cell from top to bottom, so using the same example, the
	/// Destroyer would occupy cells E3, E4, E5 and E6.</para>
	/// <para> </para>
	/// <para>A ship's condition records the healthiness of the ship. At the beginning of the game all ships have a health equivalent to
	/// their size and then as they are hit during attacks by the enemy this health is reduced. A bit-wise representation of this health
	/// is used to describe the ship's condition, so a Destroyer's initial condition is given the value 15 or 0000000000001111 in 32-bit
	/// integer binary. The LSB then represents a "healthy" game grid cell at the location that the ship occupies, and then the following
	/// bits describe the condition at the subsequent game grid cells, which are determined by the orientation of the ship on the game grid.</para>
	/// <para> </para>
	/// <para>For example, we know that a Destroyer located at game grid cell E3 and having a horizontal orientation will occupy cells E3, F3, G3
	/// and H3 and therefore the way this maps to the condition value in a bit-wise fashion is:-</para>
	/// <para> </para>
	///	<para>H3 G3 F3 E3</para>
	///	<para>1  1  1  1 = 15 (32-bit int)</para>
	/// <para> </para>
	/// <para>If this ship was to be hit at cell F3, then the condition would become 13, ie.</para>
	/// <para> </para>
	///	<para>H3 G3 F3 E3</para>
	///	<para>1  1  0  1 = 13 (32-bit int)</para>
	/// <para> </para>
	/// <para>Then if this ship was to be hit at cell H3, the condition would become 5, ie.</para>
	/// <para> </para>
	///	<para>H3 G3 F3 E3</para>
	///	<para>0  1  0  1 = 5 (32-bit int)</para>
	/// <para> </para>
	/// <para>and so on until the condition becomes 0 and the ship is regarded as sunk. For a vertically oriented ship, the logic remains the same
	/// but with the cell occupancy being top to bottom, so in the above example, a vertically oriented Destroyer would have an initial condition
	/// again of 15, but with the bit-wise cell mapping becoming:-</para>
	/// <para> </para>
	///	<para>E6 E5 E4 E3</para>
	///	<para>1  1  1  1 = 15 (32-bit int)</para>
	/// <para> </para>
	/// <para>Note that the condition at any position can only be either "full health" or "fully damaged" due to the nature of the bit-wise
	/// representation. Therefore, a further hit at a particular position along the ship does not change the condition at that position as it will
	/// already be fully damaged and show a 0 at the bit location in the condition property.</para>
	/// </summary>
	public class Ship : IShip {
		/// <summary>
		/// Represents the type of the ship. Ordinal values of each type also provide the size, in game grid cells, of the ship type.
		/// </summary>
		public enum ShipType {
			/// <summary>
			/// Destroyer type ship, of size 4.
			/// </summary>
			Destroyer = 4,

			/// <summary>
			/// Battleship type ship, of size 5.
			/// </summary>
			Battleship = 5
		}

		/// <summary>
		/// Orientation of the ship on the game grid.
		/// </summary>
		public enum ShipOrientation {
			/// <summary>
			/// Horizontal orientation, occupied cells go from left to right across the columns of the game grid.
			/// </summary>
			Horizontal = 0,

			/// <summary>
			/// Vertical orientation, occupied cells go from top to bottom across the rows of the game grid.
			/// </summary>
			Vertical
		}

		/// <summary>
		/// Property that holds the ship type.
		/// </summary>
		public ShipType Type {get; private set;}

		/// <summary>
		/// Property that holds the game grid cell location of the ship.
		/// </summary>
		public Point Location {get; private set;}

		/// <summary>
		/// Property that holds the way the ship is oriented across the game grid.
		/// </summary>
		public ShipOrientation Orientation {get; private set;}

		/// <summary>
		/// Property that holds the current health condition of the ship (see <see cref="Battleships.Model.Ship"/>).
		/// </summary>
		public int Condition {get; private set;}

		/// <summary>
		/// Property that holds the horizontal and vertical size that the ship occupies on the game grid, in game grid cells. Note, this
		/// size is determined by the orientation of the ship on the game grid.
		/// </summary>
		public Size Size {
			get {
				return ((Orientation == ShipOrientation.Horizontal) ? new Size((int)Type - 1, 0) : new Size(0, (int)Type - 1));
			}
		}

		/// <summary>
		/// Property that holds the rectangular bounds of the ship within the game grid.
		/// </summary>
		public Rect Bounds {
			get {
				return new Rect(Location, Size);
			}
		}

		/// <summary>
		/// Property that holds whether the ship is sunk or not. A sunk ship has a condition of 0.
		/// </summary>
		public bool IsSunk {
			get {
				return (Condition == 0);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">Type of the ship.</param>
		/// <param name="location">Game grid location of the ship.</param>
		/// <param name="orientation">Orientation of the ship.</param>
		public Ship(ShipType type, Point location, ShipOrientation orientation) {
			Type = type;
			Location = location;
			Orientation = orientation;

			// Initially the condition has all its relevant bits set to 1  
			Condition = ((1 << (int)Type) - 1);
		}

		/// <summary>
		/// Record a hit on the ship by modifying the condition of the ship at the position on the ship that the hit occurred. Note, the
		/// condition will not change if the hit is at an already fully damaged position, see <see cref="Battleships.Model.Ship"/> for a
		/// detailed treatment of how the condition of the ship is represented and modified. 
		/// </summary>
		/// <param name="position">Position within the bounds of the ship at which the hit should be recorded. A position of 0 means the
		/// hit is recorded at the LSB of the condition property. Recording at other positions are done at the corresponding bit location
		/// in the condition property relative to the LSB, see <see cref="Battleships.Model.Ship"/>.</param>
		/// <returns>Nothing.</returns>
		public void RecordAHit(int position) {
			// Make sure that any hit position is within the bounds of the ship's size by zeroing out any extra bit beyond these bounds
			position &= ((1 << (int)Type) - 1);

			// Use bit manipulation to set the hit bit in the condition property to 0
			Condition &= ~(1 << position);
		}
	}
}
