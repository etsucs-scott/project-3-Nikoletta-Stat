namespace Minesweeper.Core
{
    public class Tile
    {
        public bool isFlagged { get; set; }
        public bool isRevealed { get; set; }
        public bool isBomb { get; set; }
        public int AdjacentMines { get; set; }
    }
}
