namespace Minesweeper.Core
{
    // Tile contains the information for an individual tile on the board.
    public class Tile
    {
        public bool isFlagged { get; set; }
        public bool isRevealed { get; set; }
        public bool isBomb { get; set; }
        public int AdjacentMines { get; set; }
    }
}
