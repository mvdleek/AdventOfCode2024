


using System;

namespace aoc25_day_04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid("input.txt");
            Solve(grid);

            // For part 2, we need to repeatedly find movable papers until none are left
            var totalMovables = 0;

            while (true)
            {
                var movables = Solve_2(grid);
                if (movables == 0)
                {
                    break;
                }
                totalMovables += movables;
            }

            Console.WriteLine($"totalMoveables part 2 = {totalMovables}");
        }

        private static void Solve(Grid grid)
        {
            var movables = 0;
            for (int y = 1; y < grid.height - 1; y++)
            {
                for (int x = 1; x < grid.width - 1; x++)
                {
                    var cell = grid.rows[y][x];
                    // process cell
                    if (cell == '@')
                    {
                        if (getPaperNeighborCount(grid, x,y) < 4)
                        {
                            movables++;
                        }
                    }
                }
            }
            Console.WriteLine($"movables={movables}");
        }

        private static int Solve_2(Grid grid)
        {
            var movables = 0;
            for (int y = 1; y < grid.height - 1; y++)
            {
                for (int x = 1; x < grid.width - 1; x++)
                {
                    var cell = grid.rows[y][x];
                    // process cell
                    if (cell == '@')
                    {
                        if (getPaperNeighborCount(grid, x, y) < 4)
                        {
                            movables++;
                            grid.rows[y][x] = '.'; // mark as moved immediately
                        }
                    }
                }
            }

            return movables;
        }

        private static int getPaperNeighborCount(Grid grid, int x, int y)
        {
            var c = 0;
            if (grid.rows[y-1][x] == '@') c++;
            if (grid.rows[y-1][x+1] == '@') c++;
            if (grid.rows[y][x+1] == '@') c++;
            if (grid.rows[y+1][x+1] == '@') c++;
            if (grid.rows[y+1][x] == '@') c++;
            if (grid.rows[y+1][x-1] == '@') c++;
            if (grid.rows[y][x-1] == '@') c++;
            if (grid.rows[y-1][x-1] == '@') c++;
            return c;
        }
    }

    class Grid
    {
        public int width;
        public int height;

        public List<char[]> rows = [];

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
                    width = line.Length + 2;
                    AddSentinelRow('.', width);
                }
                var row = new char[width];
                row[0] = '.';
                row[width - 1] = '.';
                for (int i = 0; i < width-2; i++)
                {
                    row[i+1] = line[i];
                }
                rows.Add(row);
            }

            AddSentinelRow('.', width);
            height = rows.Count;
        }

        private void AddSentinelRow(char v, int width)
        {
            var row = new char[width];
            for (int i = 0; i < width; i++)
            {
                row[i] = v;
            }
            rows.Add(row);
        }
    }
}