namespace Minesweeper.Core;

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

    public bool inBounds (int r, int c)
    {
        return (r >= 0) && (c >= 0) && (r < size) && (c < size);
    }
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

