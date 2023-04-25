using System.Collections.Generic;
using System.Windows;

namespace Battleships.Model {
	public interface IFleet {
		/// <summary>
		/// Public accessor for the list of ships held within this fleet, this is provided as a copy of the original list to prevent
		/// it being able to be changed by the receiver of the reference.
		/// </summary>
		IList<IShip> Ships {get;}

		/// <summary>
		/// Property of the fleet that determines if all the ships have been sunk or not.
		/// </summary>
		bool AreAllShipsSunk {get;}

		/// <summary>
		/// Checks if any ship's location and orientation within the game grid intersects the supplied Rect struct, if any does then
		/// a clash is detected and reported back as a return of true, otherwise return false .
		/// </summary>
		/// <param name="shipBounds">Reference to a <see cref="System.Windows.Rect"/> struct instance that contains the bounds of a
		/// ship</param>
		/// <returns>bool, return ture if the supplied ship bounds intersect (ie. clash) with one or more bounds of ships held in the
		/// fleet, return false otherwise.</returns>
		bool DoesShipBoundsClash(Rect shipBounds);

		/// <summary>
		/// Add a ship to the fleet.
		/// </summary>
		/// <param name="ship">Reference to the ship object instance to add to the fleet.</param>
		/// <returns>Nothing.</returns>
		void AddShip(IShip ship);

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
		IShip CheckForAndRecordAnyHit(Point location);
	}
}