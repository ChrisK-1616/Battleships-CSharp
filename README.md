# Battleships-CSharp
Example solution for the Battleships exercise - implemented in vanilla C# using no additional libraries.

------------------------------------------------------------------------------------------------------------
Exercise:
=========

Programming Test: Battle ships
The purpose of this test is primarily to examine your problem solving
skills, it will not be analysed using automated tools, and you should not
spend any more than the bare minimum amount of time developing a user
interface.
Whilst you should not spend any time creating a beautiful user interface or
formatting output you are expected to make your code elegant / beautiful
and represent the best you can do.
You must write the console application in C#. Comment your code as
necessary. If you are familiar with unit testing you should write unit tests.
The Problem
Implement a very simple game of battle ships (https://en.wikipedia.org/wiki/Battleship_%28game%29).

You should create a simple console application to allow a single human player to play a
onesided game of battleships against the computer.
The program should create a 10x10 grid, and place a number of ships on the grid at
random with the following sizes:
● 1x Battleship (5 squares)
● 2x Destroyers (4 squares)
The console application should accept input from the user in the format “A5” to signify a
square to target, and feedback to the user whether the shot was success, and additionally
report on the sinking of any vessels.
Do not spend any time formatting output in the console window (displaying a grid etc.) focus
on the domain, not the presentation.
Please email your finished solution, zipped up (without binaries)

------------------------------------------------------------------------------------------------------------
Notes:
======

The solution to the test has been programmed in C# using Visual Studio 2022. There are two projects in the solution, "Battleships" which is the actual implementation of the game and "BattleshipsTests" which contains the unit tests to exercise the implementation.

The solution to the exercise has been provided in "out-of-the-box" C#. No third party code or DLLs are used This means that no IoC or Mocking frameworks have been used, with all the injected dependencies and the mocks used for testing included within the solution provided. 

If you run the debug version of the game then you will be told the starting locations of the AI player's ships (to help you to test). This is not the case for the release version of the game (when you have to play without cheating!)

The coverage on the unit testing is around 90%, I have not provided tests for all the trivial property getter/setters and for some of the constructors. There is also a lambda expression that is not fully covered in the Fleet class (inside the CheckForAndRecordAnyHit( ) method)  and this may need some further thought (it seems to work OK and is exercised by a number of more general tests which do pass).

The code comments are fairly verbose, which seems appropriate as I have not used any form of specification or design documents, but they do give detail on the design decisions behind and operation of the classes in the solution. You can use the Visual Studio object browser to read through these.


Assumptions
---------------
The solution implements the given specification for the size of the game grid and the type and number of ships within a player's fleet plus a number of assumptions have been applied as to further pertinent requirements of the Battleships game. These are:-

1. Each player has their own game grid onto which they place their ships. They then attack the game grid of the other player during their turn. This is opposed to having the two players place ships on a single game grid and attack that one, shared grid during their turn (which would be another way to represent the playing area).

2. No ship in a given player's fleet can overlay another ship. This means that any location in the game grid can only ever have either no ship or part of a specific ship contained within it.

3. Grid co-ordinates are provided using an uppercase letter to signify the column number followed by an integer number to signify the row number, as you specified. It is assumed that the letter "A" is column 1, "B" column 2 etc through to "J" as column 10. The row numbers begin at "1" and go through to "10".

4. It is assumed that if a part of a ship is "hit" then it can continue to be "hit" until the ship is sunk (ie. all part of the ship have been "hit") when it no longer can be "hit" again.

5. The automated AI player that the human player plays against is very dumb. It simply randomly distributes its ships across its game grid without any other algorithm to support this. Also, when it comes to the AI attacking, the game grid location to attack is simply a randomly selected column/row combination (even if that combination has already been attacked). However, it would be reasonably easy to replace this "dumb" AI with a more sophisticated one since the actual AI implementation is injected into the game class and can be replaced with another
version.

6. To aid the human player in remembering the attacks they ahve already made and those made against them by the AI, each attack is recorded in the Player class. Provided is a way of recalling this list of attacks as a display on the console when the human player is taking their go. Also included is a help display which can be requested during the human player's go. The form the record of each attack takes is the location attacked followed by whether that attack hit or missed a ship, for instance an attack made on location "A1" which missed an enemy ship would be recorded as "A1_", whilst if it had hit it would be recorded as "A1*".


How To Play
--------------
There are two main stages to the game, firstly the deployment of each player's "fleet" (a player's fleet consists of 2x Destroyers, of size 4, and 1x Battleship, of size 5) and then a sequence of rounds during which the players alternate to attack the each other, one grid location at a time until all of one player's fleet is sunk (or the game is quit by a player). The player with the surviving fleet is deemed the winner.

To deploy your fleet, for each ship in the fleet provide a grid location in uppercase letter followed by integer number format  (eg. "A1", "E10" etc.) and then provide an orientation of the ship across the grid ("h" or "H" for a horizontally oriented ship and "v" or "V" for a vertically oriented ship). Note, the supplied grid location is at the start of the ship and the whole ship then fills grid cells from left to right in the horizontal direction or top to bottom in the vertical direction. For example, a Destroyer (size 4) located at grid location "C3" and oriented horizontally would occupy grid cells "C3", "D3", "E3" and "F3", whilst a Battleship (size 5) located at grid location "G4" and oriented vertically would occupy grid cells "G4", "G5", "G6", "G7" and "G8". It is also possible to quit the game during fleet deployment by entering the character "x" or "X". Note, the AI player will deploy their fleet automatically after the human player has deployed theirs.

Once both player's have deployed their fleet, the game rounds begin, allowing each player to attack a single grid cell on the opposing player's game grid each round. Each player takes their go to attack sequentially, starting with the player designated as "Player One".

During the human player's go they can submit the following commands:-

? - display a help screen describing the commands available during a player's go.
! - display a list of all the previous attacks this player has made, ths is shown as a list of grid locations followed by the "_" character if that attack missed or the "*" character if it hit, eg C3* indicates an attack on grid location C3 that hit.
* - display a list of all previous attacks made by the enemy, shown in an identical fashion that detailed above.
[A-J][1-10] - make an attack at this grid location, eg. "A1", "E10" etc. (note, must be an uppercase letter)
x or X - quit from the game inmmediately

The AI player will make their attack automatically once it is their go.

The result of a player's attack can be either a "miss", if the selected grid cell is not occupied by an enemy ship or a "hit" if the grid cell is occupied by an enemy ship. Once a ship has been "hit" at each and every one of its occupied grid cells it is deemed "sunk" and cannot be "hit" again. If all a player's ships have been sunk then the opposing player is determined to have won the game.

Note, there are messages displayed to indicate ships that have been deployed, attacks that miss, attacks that hit, lists of previous attacks, ships that are sunk and invalid entries when deploying a ship or undertaking a go. The winning player is also congratulated before the game quits.
