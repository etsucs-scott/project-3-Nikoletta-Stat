using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    public class Game
    {
        public GameBoard board { get; }
        public HighScoreTracker highScores { get; }
        int size { get; set; }
        int seed { get; set; }
        int mineCount { get; set; }


        public int moveCount;
        public Stopwatch timer;
        public bool gameOver;
        public bool win;

        public Game (HighScoreTracker highScores)
        {
            GetSizeAndSeed();
            this.board = new GameBoard(size, mineCount, seed);
            this.highScores = highScores;
            this.timer = new Stopwatch();
        }

        public void GetSizeAndSeed ()
        {
                Console.Clear();
                Console.WriteLine("Menu:\n1) 8x8\n2) 12x12\n3) 16x16");
                Console.WriteLine("Choose board size: ");
                string ?sizeInput = Console.ReadLine();
                int.TryParse(sizeInput, out int chooseSize);
                if (string.IsNullOrEmpty(sizeInput) || chooseSize < 1 || chooseSize > 3)
                {
                    Console.WriteLine("Invalid input. Must choose option 1, 2, or 3.");
                }

                if (chooseSize == 1)
                {
                    size = 8;
                    mineCount = 10;
                }
                else if (chooseSize == 2)
                {
                    size = 12;
                    mineCount = 25;
                }
                else if (chooseSize == 3)
                {
                    size = 16;
                    mineCount = 40;
                }

                Console.WriteLine("Seed (blank = time): ");
                string ?seedInput = Console.ReadLine();
                if (string.IsNullOrEmpty(seedInput))
                {
                    seed = (int)DateTime.Now.Ticks;
                }
                else if (!int.TryParse(seedInput, out int seed))
                {
                    Console.WriteLine("Invalid input. Using current time as seed.");
                    seed = (int)DateTime.Now.Ticks;
                }  
        }

        public void RunGame()
        {
            moveCount = 0;
            gameOver = false;
            win = false;
            timer.Start();

            while (!gameOver)
            {
                Console.Clear();
                Console.WriteLine("Commands: r row col | f row col | q");
                board.Display();
                Console.Write("\n> ");
                string? input = Console.ReadLine();
                var splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (splitInput.Length == 1 && splitInput[0] == "q")
                {
                    gameOver = true;
                    break;
                }
                else if (splitInput.Length != 3)
                {
                    Console.WriteLine("Invalid command.");
                    continue;
                }

                string command = splitInput[0];

                bool checkRow = int.TryParse(splitInput[1], out int row);
                bool checkCol = int.TryParse(splitInput[2], out int col);

                if (!checkRow || !checkCol)
                {
                    Console.WriteLine("Invalid input. Row and col must be integers.");
                    continue;
                }
                else if (!board.inBounds(row, col))
                {
                    Console.WriteLine("Out of bounds.");
                    continue;
                }

                switch (command)
                {
                    case "r":
                        RevealTile(row, col);
                        break;
                    case "f":
                        FlagTile(row, col);
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }

                moveCount++;

                if (!gameOver && CheckIfWin() == true)
                {
                    gameOver = true;
                    win = true;
                }

            }

            timer.Stop();
            EndOfGame();
        }

        public void RevealTile (int r, int c)
        {
            if (board.board[r, c].isRevealed || board.board[r, c].isFlagged)
                return;
            bool hitMine = board.HitMine(r, c);
            if (hitMine)
            {
                gameOver = true;
                win = false;
            }
        }

        public void FlagTile (int r, int c)
        {
            if (board.board[r, c].isRevealed)
                return;
            board.board[r, c].isFlagged = !board.board[r, c].isFlagged;
        }

        public bool CheckIfWin ()
        {
            for (int r = 0; r < board.size;  r++)
            {
                for (int c = 0; c < board.size; c++)
                {
                    if (!board.board[r, c].isBomb && !board.board[r, c].isRevealed)
                        return false;
                }
            }
            return true;
        }

        public void EndOfGame()
        {
            Console.Clear();
            board.Display();
            int totalSeconds = (int)timer.Elapsed.TotalSeconds;
            if (win)
            {
                Console.WriteLine($"You won!\nSeconds: {totalSeconds}\nMoves: {moveCount}");
                highScores.AddNewScore(board.size, totalSeconds, moveCount, seed, DateTime.Now);
                highScores.SaveScoresToFile();
            }
            else
            {
                Console.WriteLine("You hit a mine! You lost!");
            }
        }
    }
}
