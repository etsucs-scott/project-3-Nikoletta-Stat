[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/ozVFrFMv)
# CSCI 1260 — Project 3

## How To Build and Run
In terminal: 
dotnet build
dotnet run --project src/Minesweeper.Console

OR by pressing green run Minesweeper.Console button in Visual Studio.


---

## Board Sizes

There are 3 board size options in the start menu: 
1) 8x8 --> 10 mines
2) 12x12 --> 25 mines
3) 16x16 -- 40 mines

### Input Commands
r row col --> Reveal a tile at coordinates (row, col)

f row col --> Flag or unflag a tile at coordinates (row, col)

q --> Quit game at any time

###  Seed Usage
Users can enter an integer for a seed to make mine placement deterministic and random.

If no integer or an invalid seed is entered, the current timestamp is used as the seed.

### High Scores File
The top high scores for each board are based on lowest completion time in seconds, then moves as a tiebreaker.

The scores are stored in a .csv file in the data folder.

File path: data/highscores.csv

Formatted: size,seconds,moves,seed,timestamp

### Board Symbols
'#' = unrevealed tile

'.' = revealed tile with no adjacent mines

'b' = revealed bomb, only shown if player lost the game

'f' = flagged tile

'1-8' = the number of mines adjacent to that tile

## Unit Tests
Run unit tests in Visual Studio Test Explorer tab, run all tests.
