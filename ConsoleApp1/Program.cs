using System;

class MinefieldGame
{
    static char[,] grid;
    static char[,] displayGrid;
    static int rows = 10;
    static int cols = 10;
    static int mineCount = 20;
    static bool gameOver = false;

    static void Main(string[] args)
    {
        InitializeGrids();
        PlaceMines();

        while (!gameOver)
        {
            DisplayGrid();
            Console.WriteLine("Enter your move (row and column): ");
            Console.Write("Row (0 to {0}): ", rows - 1);
            int row = int.Parse(Console.ReadLine());
            Console.Write("Column (0 to {0}): ", cols - 1);
            int col = int.Parse(Console.ReadLine());

            if (row < 0 || row >= rows || col < 0 || col >= cols)
            {
                Console.WriteLine("Invalid move. Try again.");
                continue;
            }

            if (grid[row, col] == 'M')
            {
                gameOver = true;
                Console.Clear();
                DisplayGrid(revealMines: true);
                Console.WriteLine("Boom! You hit a mine. Game Over.");
            }
            else
            {
                RevealCell(row, col);
                if (CheckWin())
                {
                    Console.Clear();
                    DisplayGrid(revealMines: true);
                    Console.WriteLine("Congratulations! You cleared the minefield!");
                    gameOver = true;
                }
            }
        }
    }

    static void InitializeGrids()
    {
        grid = new char[rows, cols];
        displayGrid = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = '0'; // Initialize grid with no mines
                displayGrid[i, j] = '-'; // Hide cells initially
            }
        }
    }

    static void PlaceMines()
    {
        Random rand = new Random();
        int placedMines = 0;

        while (placedMines < mineCount)
        {
            int row = rand.Next(rows);
            int col = rand.Next(cols);
            if (grid[row, col] != 'M')
            {
                grid[row, col] = 'M';
                placedMines++;
                UpdateNeighborCounts(row, col);
            }
        }
    }

    static void UpdateNeighborCounts(int mineRow, int mineCol)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int row = mineRow + i;
                int col = mineCol + j;
                if (row >= 0 && row < rows && col >= 0 && col < cols && grid[row, col] != 'M')
                {
                    grid[row, col]++;
                }
            }
        }
    }

    static void DisplayGrid(bool revealMines = false)
    {
        Console.Clear();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (revealMines)
                {
                    Console.Write(grid[i, j] == 'M' ? 'M' : displayGrid[i, j]);
                }
                else
                {
                    Console.Write(displayGrid[i, j]);
                }
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }

    static void RevealCell(int row, int col)
    {
        if (row < 0 || row >= rows || col < 0 || col >= cols || displayGrid[row, col] != '-')
        {
            return;
        }

        displayGrid[row, col] = grid[row, col];

        if (grid[row, col] == '0')
        {
            // Recursively reveal surrounding cells
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    RevealCell(row + i, col + j);
                }
            }
        }
    }

    static bool CheckWin()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] != 'M' && displayGrid[i, j] == '-')
                {
                    return false; // If there are still non-mine cells unrevealed
                }
            }
        }
        return true;
    }
}