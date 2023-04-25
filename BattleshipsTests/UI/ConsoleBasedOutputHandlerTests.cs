using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.UI;

namespace BattleshipsTests.UI {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.UI.ConsoleBasedOutputHandler class.
	/// </summary>
	[TestClass]
	public class ConsoleBasedOutputHandlerTests {
		[TestMethod]
		public void Test_Message() {
			// Arrange, change the Console.Out property to write to a System.Text.StringBuilder object instance, the string built into this can
			// then be tested
			var oldOut = Console.Out; // Store the current Console.Out stream writer
			var outputStringBuffer = new StringBuilder();
			Console.SetOut(new StringWriter(outputStringBuffer));
			var target = new ConsoleBasedOutputHandler();
			const string expected = "Test message";

			// Act
			target.Message("Test message");
			Console.SetOut(oldOut); // Restore the original Console.out stream writer
			var actual = outputStringBuffer.ToString();
			
			// Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
