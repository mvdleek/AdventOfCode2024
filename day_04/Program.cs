



namespace day_04
{
    internal class Program
    {
        const bool doPart2 = true;

        // 6:38 start (na prepare)
        // 7:29 finished
        static void Main(string[] args)
        {
            var (grid, width, height) = ReadInput("input.txt");

            var count = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!doPart2)
                    {
                        count += GetXmasCountFrom(grid, 3 + i, 3 + j);
                    }
                    else
                    {
                        if (IsXmasAt(grid, 3 + i, 3 + j))
                        {
                            count++;
                        }
                    }
                }
            }


            Console.WriteLine($"Count: {count}");
            // 2575

            // part 2:
            // 2041
        }

        private static bool IsXmasAt(char[,] grid, int x, int y)
        {
            if (grid[x,y] != 'A') { return false; }

            var mas1found = (grid[x-1, y-1] == 'M' && grid[x+1, y+1] == 'S') || (grid[x-1, y-1] == 'S' && grid[x+1, y+1] == 'M');
            var mas2found = (grid[x+1, y-1] == 'M' && grid[x-1, y+1] == 'S') || (grid[x+1, y-1] == 'S' && grid[x-1, y+1] == 'M');

            return mas1found && mas2found;
        }

        private static int GetXmasCountFrom(char[,] grid, int x, int y)
        {
            var count = 0;

            if (grid[x,y] == 'X')
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) { continue; }


                        if (ExistsXmasInDir(grid, x, y, dx, dy))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private static bool ExistsXmasInDir(char[,] grid, int x, int y, int dx, int dy)
        {
            const string Xmas = "XMAS";

            for (int i = 0; i < Xmas.Length; i++)
            {
                if (grid[x,y] != Xmas[i])
                {
                    return false;
                }

                x += dx;
                y += dy;
            }

            return true;
        }

        private static (char[,] grid, int width, int height) ReadInput(string v)
        {
            var input = File.ReadAllLines(v);
            var width = input.Length;
            var height = input[0].Length;

            var grid = new char[height + 6, width + 6];

            for (int i = 0; i < height+6; i++)
            {
                for (int j = 0; j < width+6; j++)
                {
                    grid[i, j] = '.';
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grid[3+i, 3+j] = input[j][i];
                }
            }

            return (grid, height, width);
        }
    }
}
