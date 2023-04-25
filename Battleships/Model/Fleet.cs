using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ShipOrientation = Battleships.Model.Ship.ShipOrientation;

namespace Battleships.Model {
	/// <summary>
	/// Holds and maintains a list of ships that constitute the fleet of a player. 
	/// </summary>
	public class Fleet : IFleet {
		/// <summary>
		/// Property that references the list of Ship object instances within the fleet.
		/// </summary>
		private readonly IList<IShip> ships;

		/// <summary>
		/// Public accessor for the list of ships held within this fleet, this is provided as a copy of the original list to prevent
		/// it being able to be changed by the receiver of the reference.
		/// </summary>
		public IList<IShip> Ships {
			get {
				return ships.Select(i => i).ToList();
			}
		}

		/// <summary>
		/// Property of the fleet that determines if all the ships have been sunk or not.
		/// </summary>
		public bool AreAllShipsSunk {
			get {
				return ships.All(s => s.IsSunk);
			}
		}

		/// <summary>
		/// Constructor, this version implements an empty ships list.
		/// </summary>
		public Fleet() {
			ships = new List<IShip>();
		}

		/// <summary>
		/// Constructor, this version implements an ships list that is initialised with the supplied enumerable of IShip object instances.
		/// </summary>
		/// <param name="ships">Reference to System.Collections.Generic.IEnumerable object instance that contains the IShip object instances
		/// with which to initialise the ships list.</param>
		public Fleet(IEnumerable<IShip> ships) {
			this.ships = new List<IShip>(ships);
		}

		/// <summary>
		/// Checks if any ship's location and orientation within the game grid intersects the supplied Rect struct, if any does then
		/// a clash is detected and reported back as a return of true, otherwise return false .
		/// </summary>
		/// <param name="shipBounds">Reference to a <see cref="System.Windows.Rect"/> struct instance that contains the bounds of a
		/// ship</param>
		/// <returns>bool, return ture if the supplied ship bounds intersect (ie. clash) with one or more bounds of ships held in the
		/// fleet, return false otherwise.</returns>
		public bool DoesShipBoundsClash(Rect shipBounds) {
			return Ships.Any(s => shipBounds.IntersectsWith(s.Bounds));
		}

		/// <summary>
		/// Add a ship to the fleet.
		/// </summary>
		/// <param name="ship">Reference to the ship object instance to add to the fleet.</param>
		/// <returns>Nothing.</returns>
		public void AddShip(IShip ship) {
			if(ReferenceEquals(null, ship)) {
				return;
			}

			ships.Add(ship);
		}

		/// <summary>
		/// <para>This performs two duties, checks if the supplied game grid location is within the game grid bounds of any ship in the fleet.
		/// If this is the case, then the ship in question has this hit recorded against it at the position within the bounds of the ship
		/// that has been hit. If a hit occurred, a reference to the ship hit is returned, otherwise a null is returned to indicate that
		/// there was no hit.</para>
		/// <para> </para>
		/// <para>Note: one of the requirements of the Battleships game is that only one ship will occupy any single game grid cell, therefore if
		/// there is a hit detected then this will be one and only ship that is hit.</para>
		/// </summary>
		/// <param name="location">Game grid location to check for hit on any ship.</param>
		/// <returns>Battleships.Model.IShip reference, null returned if no hit detected, otherwise the ship object instance that is hit.</returns>
		public IShip CheckForAndRecordAnyHit(Point location) {
			return ships.FirstOrDefault(s => {
				if(s.IsSunk) {
					// Cannot hit a ship that has been sunk
					return false;
				}

				// Check the ship to see if the location supplied lies within the bounds of the ship being checked
				var isHit = s.Bounds.Contains(location);

				if(isHit) {
					// Record the hit against the ship
					if(s.Orientation == ShipOrientation.Horizontal) {
						// Use the X co-ordinate of the supplied location to determine the hit position on a horizontally oriented ship  
						s.RecordAHit((int)location.X - (int)s.Location.X);
					}
					else {
						// Use the Y co-ordinate of the supplied location to determine the hit position on a vertically oriented ship  
						s.RecordAHit((int)location.Y - (int)s.Location.Y);
					}
				}

				return isHit;
			});
		}
	}
}
