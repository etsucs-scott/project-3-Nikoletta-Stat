namespace Minesweeper.Core;

// GameBoard holds the array of tiles to make up the actual board and handles the mine logic.
public class GameBoard
{
    public int size { get; }
    public int mineCount { get; }
    public int seed { get; }

    public Random rand;
    public bool minesPlaced { get; set; }

    public Tile[,] board { get; }

    public GameBoard (int size, int mineCount, int seed)
    {
        this.size = size;
        this.mineCount = mineCount;
        this.seed = seed;
        rand = new Random(seed);
        rand.Next();
        board = new Tile[size, size];
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
                board[r, c] = new Tile();
        }
    }

    // Display() writes the board to the screen with each tile's symbol. It also writes a row and col header.
    public void Display()
    {
        Console.Write("  ");
        for (int c = 0; c < size; c++)
            Console.Write($"{c} ");
        Console.WriteLine();

        for (int r = 0; r < size; r++)
        {
            Console.Write($"{r} ");
            for (int c = 0; c < size; c++)
            {
                string symbol;

                if (board[r, c].isFlagged)
                    symbol = "f";
                else if (board[r, c].isBomb && board[r, c].isRevealed)
                    symbol = "b";
                else if (board[r, c].isRevealed && board[r, c].AdjacentMines > 0)
                    symbol = board[r, c].AdjacentMines.ToString();
                else if (board[r, c].isRevealed)
                    symbol = ".";
                else
                    symbol = "#";
                
                Console.Write(symbol + " ");
            }
            Console.WriteLine();
        }   
    }

    // PlaceMines() places a set number of mines based on the board size after the user reveals the first
    // row and col.
    public void PlaceMines(int initialRow, int initialCol)
    {
        int placed = 0;

        while (placed < mineCount)
        {
            int r = rand.Next(size);
            int c = rand.Next(size);

            if ((r == initialRow && c == initialCol) || board[r, c].isBomb)
                continue;

            board[r, c].isBomb = true;
            placed++;
        }
    }

    // inBounds() checks to make sure the row and col that the user chose is in the bounds of the board edges.
    public bool inBounds (int r, int c)
    {
        return (r >= 0) && (c >= 0) && (r < size) && (c < size);
    }

    // SetAdjacentMines() counts the number of mines around each tile and sets that tile's adjacent mine count
    // to that number.
    public void SetAdjacentMines ()
    {
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (board[r, c].isBomb)
                {
                    board[r, c].AdjacentMines = -1;
                    continue;
                }
                int count = 0;
                for (int rowChecker = -1; rowChecker <= 1; rowChecker++)
                {
                    for (int colChecker = -1; colChecker <= 1; colChecker++)
                    {
                        if (rowChecker == 0 && colChecker == 0)
                            continue;
                        int checkingRow = r + rowChecker;
                        int checkingCol = c + colChecker;

                        if (!inBounds(checkingRow, checkingCol))
                            continue;

                        if (board[checkingRow, checkingCol].isBomb)
                        {
                            count++;
                        }
                    }
                }
                board[r, c].AdjacentMines = count;
            }
        }
    }

    // HitMine() calls PlaceMines() and SetAdjacentMines() after the first reveal, then reveals the tile.
    // It checks for a bomb and calls CascadeReveal().
    public bool HitMine (int r, int c)
    {
        if (!minesPlaced)
        {
            PlaceMines(r, c);
            SetAdjacentMines();
            minesPlaced = true;
            CascadeReveal(r, c);
        }

        if (board[r, c].isRevealed || board[r, c].isFlagged)
            return false;

        board[r, c].isRevealed = true;

        if (board[r, c].isBomb)
            return true;

        if (board[r, c].AdjacentMines == 0)
            CascadeReveal(r, c);
        
        return false;

    }

    // CascadeReveal() checks every tile around the intial tile for adjacent mines. If one of the adjacent tiles
    // does not have adjacent mines, it is added to a queue and that tile's adjacent tiles are checked too.
    // It uses a breadth-first search.
    public void CascadeReveal (int r, int c)
    {
        Queue<(int row, int col)> queue = new Queue<(int, int)>();
        queue.Enqueue((r, c));
        
        int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

        while (queue.Count > 0)
        {
            var (currR,currC) = queue.Dequeue();
            for (int k = 0; k < 8; k++)
            {
                int nr = currR + dr[k];
                int nc = currC + dc[k];
                if (!inBounds(nr, nc))
                    continue;
                if (board[nr, nc].isRevealed || board[nr, nc].isFlagged || board[nr, nc].isBomb)
                    continue;
                board[nr, nc].isRevealed = true;
                if (board[nr, nc].AdjacentMines == 0)
                    queue.Enqueue((nr, nc));

             
            }

        }
    }
}

