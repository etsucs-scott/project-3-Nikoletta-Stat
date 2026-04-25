using Minesweeper.Core;

namespace Minesweeper.Tests
{
    // 10 deterministic unit tests
    public class Tests
    {
        [Fact]
        public void CheckSize()
        {
            var board = new GameBoard(8, 10, 1);
            Assert.Equal(8, board.size);
        }
        [Fact]
        public void CheckMineCount()
        {
            var board = new GameBoard(8, 10, 2);
            board.PlaceMines(0, 0);
            int count = 0;
            foreach (var tile in board.board)
            {
                if (tile.isBomb)
                    count++;
            }
            Assert.Equal(10, count);
        }

        [Fact]
        public void FirstRevealIsNotMine()
        {
            var board = new GameBoard(8, 10, 3);
            board.PlaceMines(2, 2);
            Assert.False(board.board[2, 2].isBomb);
        }

        [Fact]
        public void CheckSeedGeneratesMineLayout()
        {
            var board1 = new GameBoard(8, 10, 4);
            var board2 = new GameBoard(8, 10, 4);

            board1.PlaceMines(0, 0);
            board2.PlaceMines(0, 0);

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                    Assert.Equal(board1.board[r, c].isBomb, board2.board[r, c].isBomb);
            }
        }

        [Fact]
        public void CheckAdjacentMinesTrue()
        {
            var board = new GameBoard(8, 10, 5);
            board.board[3, 3].isBomb = true;
            board.board[3, 5].isBomb = true;
            board.board[4, 4].isBomb = true;

            Assert.Equal(3, board.board[3, 4].AdjacentMines);
        }

        [Fact]
        public void CheckCascadeReveal()
        {
            var board = new GameBoard(8, 10, 6);
            board.PlaceMines(0, 0);
            board.SetAdjacentMines();
            board.CascadeReveal(0, 0);
            Assert.True(board.board[0, 0].isRevealed);
        }

        [Fact]
        public void HitMineTrue()
        {
            var board = new GameBoard(8, 10, 7);
            board.board[1, 1].isBomb = true;
            Assert.True(board.HitMine(1, 1));
        }

        [Fact]
        public void HitMineFalse()
        {
            var board = new GameBoard(8, 10, 8);
            board.board[1, 1].isBomb = false;
            Assert.False(board.HitMine(1, 1));
        }

        [Fact]
        public void CheckWin()
        {
            var scores = new HighScoreTracker();
            var game = new Game(scores);
            var board = new GameBoard(8, 10, 9);
            board.PlaceMines(0, 0);
            board.SetAdjacentMines();
            for (int r  = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (!board.board[r, c].isBomb)
                        board.board[r, c].isRevealed = true;
                }
            }
            Assert.True(game.CheckIfWin());
        }
        [Fact]
        public void CheckBounds()
        {
            var board = new GameBoard(12, 25, 10);
            Assert.False(board.inBounds(13, 0));
            Assert.True(board.inBounds(5, 5));
        }
        
    }
}