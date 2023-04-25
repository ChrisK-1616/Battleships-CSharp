using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.UI;
using CommandType = Battleships.UI.InputCommand.CommandType;

namespace BattleshipsTests.UI {
	/// <summary>
	/// Unit test to exercise methods which are members of the Battleships.UI.ConsoleBasedInputHandler class.
	/// </summary>
	[TestClass]
	public class ConsoleBasedInputHandlerTests {
		[TestMethod]
		public void Test_GetInput_Valid() {
			// Arrange, change the Console.In property to read from a supplied string, this supplied string will be the input tested against
			var oldIn = Console.In; // Store the current Console.In stream reader
			Console.SetIn(new StringReader("!"));
			var target = new ConsoleBasedInputHandler();
			var expected = new InputCommand {Type = CommandType.ShowAttacks, Data = "!"};
	
			// Act, expected return is InputCommand struct instance with Type property as CommandType.ShowAttacks and Data property as "!" 
			var actual = target.GetInput();
			Console.SetIn(oldIn); // Restore the original Console.In stream reader

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_GetInput_Invalid() {
			// Arrange, change the Console.In property to read from a supplied string, this supplied string will be the input tested against
			var oldIn = Console.In; // Store the current Console.In stream reader
			Console.SetIn(new StringReader("INVALID"));
			var target = new ConsoleBasedInputHandler();
			var expected = new InputCommand {Type = CommandType.None, Data = String.Empty};

			// Act, expected return is InputCommand struct instance with Type property as CommandType.ShowAttacks and Data property as "!" 
			var actual = target.GetInput();
			Console.SetIn(oldIn); // Restore the original Console.In stream reader

			// Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test_ParseInputCommand_Valid() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(ConsoleBasedInputHandler));
			var expected00 = new InputCommand {Type = CommandType.HelpRequest, Data = "?"};
			var expected01 = new InputCommand {Type = CommandType.ShowAttacks, Data = "!"};
			var expected02 = new InputCommand {Type = CommandType.ShowAttacks, Data = "*"};
			var expected03 = new InputCommand {Type = CommandType.Quit, Data = "x"};
			var expected04 = new InputCommand {Type = CommandType.Location, Data = "E10"};
			var expected05 = new InputCommand {Type = CommandType.Orientation, Data = "v"};

			// Act, expected return is InputCommand struct instance with Type property as CommandType.HelpRequest and Data property as "?"
			var actual00 = (InputCommand)target.InvokeStatic("ParseInputCommand", "?");

			// Assert
			Assert.AreEqual(expected00, actual00);

			// Act, expected return is InputCommand struct instance with Type property as CommandType.ShowAttacks and Data property as "!" 
			var actual01 = (InputCommand)target.InvokeStatic("ParseInputCommand", "!");

			// Assert
			Assert.AreEqual(expected01, actual01);

			// Act, expected return is InputCommand struct instance with Type property as CommandType.ShowAttacks and Data property as "*" 
			var actual02 = (InputCommand)target.InvokeStatic("ParseInputCommand", "*");

			// Assert
			Assert.AreEqual(expected02, actual02);

			// Act, expected return is InputCommand struct instance with Type property as CommandType.Quit and Data property as "x" 
			var actual03 = (InputCommand)target.InvokeStatic("ParseInputCommand", "x");

			// Assert
			Assert.AreEqual(expected03, actual03);

			// Act, expected return is InputCommand struct instance with Type property as CommandType.Location and Data property as "E10"
			var actual04 = (InputCommand)target.InvokeStatic("ParseInputCommand", "E10");

			// Assert
			Assert.AreEqual(expected04, actual04);

			// Act, expected return is InputCommand struct instance with Type property as CommandType.Orientation and Data property as "v" 
			var actual05 = (InputCommand)target.InvokeStatic("ParseInputCommand", "v");

			// Assert
			Assert.AreEqual(expected05, actual05);
		}

		[TestMethod]
		public void Test_ParseInputCommand_Invalid() {
			// Arrange, need to use Microsoft.VisualStudio.TestTools.UnitTesting.PrivateType wrapper class since the exercised method is private 
			// and static
			var target = new PrivateType(typeof(ConsoleBasedInputHandler));
			var expected = new InputCommand {Type = CommandType.None, Data = String.Empty};

			// Act, expected return is InputCommand struct instance with Type property as CommandType.None and Data property as System.String.Empty 
			var actual = (InputCommand)target.InvokeStatic("ParseInputCommand", "INVALID");

			// Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
