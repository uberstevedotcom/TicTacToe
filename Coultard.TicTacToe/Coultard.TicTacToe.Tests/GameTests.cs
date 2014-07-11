namespace Coultard.TicTacToe.Tests
{
	using System.Linq;

	using NUnit.Framework;

	using Should.Fluent;

	/// <summary>
	/// Generally there could be more/better tests for the Game class. i.e. by injecting the IBoard as a Mocked/Faked
	/// object (e.g. using Moq) and thereby testing the Game class alone rather than using the Board class implementation.
	/// These are integration tests: integrating the Game and Board classes.
	/// </summary>
	[TestFixture]
	public class GameTests
	{
		#region Public Methods and Operators

		[Test]
		public void PlaceMoveTest()
		{
			// Arrange
			const int MoveRow = 2;
			const int MoveCol = 1;
			const Mark MoveMark = Mark.Nought;
			var game = new Game();

			// Act
			game.PlaceMove(Mark.Nought, MoveRow, MoveCol);

			// Assert
			game.Board[MoveRow, MoveCol].Should().Equal((int)MoveMark);
		}

		[Test]
		public void PlaceMoveInValidTest()
		{
			// Arrange
			const int MoveRow = 2;
			const int MoveCol = 2;
			const Mark InitialMark = Mark.Cross;
			const Mark MoveMark = Mark.Nought;

			var init = new int[Board.Rows, Board.Columns];
			init[MoveRow, MoveCol] = (int)InitialMark;
			var initBoard = new Board(init);
			var game = new Game(initBoard);

			// Act
			game.PlaceMove(MoveMark, MoveRow, MoveCol);

			// Assert
			game.Board[MoveRow, MoveCol].Should().Equal((int)InitialMark);
		}

		[Test]
		public void PlaceMoveAddsMoveToList()
		{
			// Arrange
			const Mark ExpectedMark = Mark.Nought;
			const int MoveRow = 2;
			const int MoveCol = 1;

			var game = new Game();

			// Act
			game.PlaceMove(ExpectedMark, MoveRow, MoveCol);

			// Assert
			game.Moves.Should().Not.Be.Null();
			var firstMove = game.Moves.FirstOrDefault();
			firstMove.Should().Not.Be.Null();
			firstMove.Mark.Should().Equal(ExpectedMark);
			firstMove.Row.Should().Equal(MoveRow);
			firstMove.Col.Should().Equal(MoveCol);
		}

		[Test]
		public void PlaceMoveInvalidEmptyDoesntAddMoveToList()
		{
			// Arrange
			var game = new Game();

			// Act
			game.PlaceMove(Mark.Empty, 1, 1);

			// Arrange
			game.Moves.Count().Should().Equal(0);
		}

		[Test]
		public void PlaceMoveInvalidUsedSquareDoesntAddMoveToList()
		{
			// Arrange
			var game = new Game();

			// Act
			game.PlaceMove(Mark.Nought, 1, 1);
			game.PlaceMove(Mark.Cross, 1, 1);

			// Arrange
			game.Moves.Count().Should().Equal(1);
		}


		[Test]
		public void PlayGameTest()
		{
			// Arrange
			var game = new Game();

			// Act
			game.PlayGame();

			// Assert
			game.Moves.Count().Should().Be.GreaterThan(4);
			game.Board.IsGameOver.Should().Be.True();

			// NB: we shouldn't be using another method to establish the outcome of this test.
			// However, as IsDraw has been tested I feel comfortable doing this.
			if (!game.Board.IsDraw)
			{
				game.Board.WinningMark.Should().Not.Equal(Mark.Empty);
			}
			else
			{
				game.Board.WinningMark.Should().Equal(Mark.Empty);
			}
		}

		[Test]
		public void StartGameTest()
		{
			// Arrange
			var game = new Game();

			// Assert
			game.Should().Not.Be.Null();
			game.Board.Should().Not.Be.Null();
			foreach (int i in game.Board)
			{
				i.Should().Equal(0);
			}
		}

		#endregion
	}
}