


namespace aoc25_day_07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid("input.txt");

            RunBeams(grid);

            //grid.print();

            TelSplitsingen(grid);

            //p2

            grid = new Grid("input.txt");

            SolvePart2_Smart(grid);
        }

        private static void SolvePart2_BruteForce(Grid grid)
        {
            var startX = grid.rows[0].IndexOf('S');

            var aantalPaden = FindNumberOfPathsFrom(0, startX, grid);

            Console.WriteLine($"Aantal paden: {aantalPaden}"); // ai generated this line
        }

        private static void SolvePart2_Smart(Grid grid)
        {
            var startX = grid.rows[0].IndexOf('S');

            var aantalPaden = FindNumberOfPathsFrom_Smart(0, startX, grid);

            Console.WriteLine($"Aantal paden: {aantalPaden}"); // ai generated this line
        }

        static long pathCount = 0;
        private static int FindNumberOfPathsFrom(int rowNumber, int startX, Grid grid)
        {
            if (rowNumber == grid.height - 1)
            {
                pathCount++;
                return 1; // Reached the bottom row, count as one valid path
            }

            if (grid.rows[rowNumber+1][startX] == '^') //ai genereerde zonder+1
            {
                // Split into two paths
                int leftPaths = FindNumberOfPathsFrom(rowNumber + 1, startX - 1, grid);
                int rightPaths = FindNumberOfPathsFrom(rowNumber + 1, startX + 1, grid);
                return leftPaths + rightPaths;
            }

            return FindNumberOfPathsFrom(rowNumber + 1, startX, grid);
        }

        private static long FindNumberOfPathsFrom_Smart(int rowNumber, int startX, Grid grid)
        {
            if (rowNumber == grid.height - 1)
            {
                pathCount++;
                return 1; // Reached the bottom row, count as one valid path
            }

            if (grid.rows[rowNumber + 1][startX] == '^') //ai genereerde zonder+1
            {
                if (grid.pathCounts[rowNumber + 1][startX] == 0)
                {
                    // Split into two paths
                    long leftPaths = FindNumberOfPathsFrom_Smart(rowNumber + 1, startX - 1, grid);
                    long rightPaths = FindNumberOfPathsFrom_Smart(rowNumber + 1, startX + 1, grid);
                    grid.pathCounts[rowNumber + 1][startX] = leftPaths + rightPaths;
                }
                //else
                //{
                //     Console.WriteLine($"Using cached value for row {rowNumber + 1}, x {startX}: {grid.pathCounts[rowNumber + 1][startX]}");
                //}

                return grid.pathCounts[rowNumber + 1][startX];
            }

            return FindNumberOfPathsFrom_Smart(rowNumber + 1, startX, grid);
        }

        private static void TelSplitsingen(Grid grid)
        {
            var splitsingen = 0;
            // splitsing als boven een ^ een | wordt gevonden
            for (int i = 1; i < grid.height; i++)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    if (grid.rows[i][j] == '^')
                    {
                        if (grid.rows[i - 1][j] == '|')
                        {
                            splitsingen++;
                        }
                    }
                }
            }

            Console.WriteLine($"Aantal splitsingen: {splitsingen}");
        }

        private static void RunBeams(Grid grid)
        {
            int row = 0;
            while (row < grid.height - 1)
            {
                for (int i = 0; i < grid.width; i++)
                {
                    var c = grid.rows[row][i];
                    if (c == '|' || c == 'S')
                    {
                        var d = grid.rows[row + 1][i];
                        if (d == '.')
                        {
                            grid.rows[row + 1][i] = '|';
                        }
                        if (d == '^')
                        {
                            grid.rows[row + 1][i - 1] = '|';
                            grid.rows[row + 1][i + 1] = '|';
                        }
                    }
                }
                row++;
            }
        }
    }

    class Grid
    {
        public int width;
        public int height;

        public List<char[]> rows = [];

        public List<long[]> pathCounts = [];

        public Grid(string filename)
        {
            ReadInput(filename);
        }

        public void ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                if (width == 0)
                {
                    width = line.Length;
                }
                var row = new char[width];
                for (int i = 0; i < width; i++)
                {
                    row[i] = line[i];
                }
                rows.Add(row);

                var path = new long[width];
                for (int i = 0; i < width; i++)
                {
                    path[i] = 0;
                }
                pathCounts.Add(path);
            }
            height = rows.Count;
        }

        internal void print()
        {
            foreach (var row in rows)
            {
                Console.WriteLine(new string(row));
            }
        }
    }
}
