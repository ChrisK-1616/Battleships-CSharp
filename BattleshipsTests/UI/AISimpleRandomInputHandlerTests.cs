using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.UI;

namespace BattleshipsTests.UI {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.UI.AISimpleRandomInputHandler class.
	/// </summary>
	[TestClass]
	public class AISimpleRandomInputHandlerTests {
		/// <summary>
		/// This overrides the Next(...) methods of the System.Random class to deliver a supplied sequence of random numbers. Use an instance
		/// of this class to replace the true random number generator utilised by AIRandomFleetBuilder object instances during testing (to make
		/// any tests deterministic).
		/// </summary>
		private class RandomFake : Random {
			/// <summary>
			/// Sequence of numbers to be returned by the Next(...) methods, this is designed to used in a cyclic fashion if more than List.Count
			/// numbers have been returned.
			/// </summary>
			private readonly IList<int> numberSequence;

			/// <summary>
			/// Next number counter, incremented each time a new number is taken from the number sequence.
			/// </summary>
			private int nextNumberIndex;

			public RandomFake(IEnumerable<int> numberSequence) {
				// Initialise the sequence of numbers to be returned by the Next(...) methods
				this.numberSequence = new List<int>(numberSequence);
			}

			/// <summary>
			/// Overridden method to supply a number from the number sequence rather than a proper random number.
			/// </summary>
			/// <returns>int, number selected from number sequence.</returns>
			public override int Next() {
				// If the number sequence is empty, then return 0 (best that can be done)
				if(numberSequence.Count == 0) {
					return 0;
				}

				// Get current number index
				var numberIndex = nextNumberIndex % numberSequence.Count;

				// Increment next number index
				nextNumberIndex++;

				return numberSequence[numberIndex];
			}

			/// <summary>
			/// Overridden method to supply a number from the number sequence rather than a proper random number.
			/// </summary>
			/// <param name="maxValue">This is not used in this fake random implementation.</param>
			/// <returns>int, number selected from number sequence.</returns>
			public override int Next(int maxValue) {
				return Next();
			}
		}

		[TestMethod]
		public void Test_GetInput() {
			// Arrange
			var rng = new RandomFake(new[] {3, 7});
			var target = new AISimpleRandomInputHandler(rng, new Size(10, 10));
			var expected = new InputCommand {Type = InputCommand.CommandType.Location, Data = "D8"};

			// Act, expected return is InputCommand struct instance with Type property as CommandType.Location and Data property as "D8"
			var actual = target.GetInput();

			// Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
