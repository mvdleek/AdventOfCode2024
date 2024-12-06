
namespace day_06
{
    internal class Program
    {
        // start@17:40
        // p1   @18:55
        static void Main(string[] args)
        {
            //var grid = ReadInput("input_example.txt"); // 41  only 1 bug 'OldY=CurrentX'
            var grid = ReadInput("input.txt"); // 4778 2nd time right   only 1 bug 'OldY=CurrentX'

            while (grid.IsGuardOnMap)
            {
                grid.MakeStep();
            }

            Console.WriteLine($"Distinct positions visited: {grid.distinctPositionsVisited}");
        }

        private static Grid ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);

            return new Grid(lines);
        }
    }

    class Grid
    {
        int currentX = -1;
        int currentY = -1;
        Direction currentD;

        List<char[]> rows = new();

        public int distinctPositionsVisited;

        public bool IsGuardOnMap => GetSquare(currentX, currentY) != '@';

        public Grid(string[] lines)
        {
            var width = lines[0].Length + 2;

            rows.Add(new string('@', width).ToArray());

            foreach (var line in lines)
            {
                var row = new char[width];
                row[0] = '@';
                row[width - 1] = '@';

                for (var i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    if (c == '^')
                    {
                        currentX = 1 + i;
                        currentY = rows.Count;
                        currentD = Direction.N;
                        c = 'X';
                        distinctPositionsVisited = 1;
                    }

                    row[1 + i] = c;
                }

                rows.Add(row);
            }

            rows.Add(new string('@', width).ToArray());
        }

        private char GetSquare(int x, int y)
        {
            return rows[y][x];
        }

        internal void MakeStep()
        {
            var oldX = currentX;
            var oldY = currentY;

            if (currentD == Direction.N)
            {
                currentY -= 1;
            }
            if (currentD == Direction.E)
            {
                currentX += 1;
            }
            if (currentD == Direction.S)
            {
                currentY += 1;
            }
            if (currentD == Direction.W)
            {
                currentX -= 1;
            }

            var sq = GetSquare(currentX, currentY);
            if (sq == '#')
            {
                currentX = oldX;
                currentY = oldY;

                currentD = (Direction)((((int)currentD) + 1) % 4);

                MakeStep(); 
            }
            else
            {
                if (sq == '.')
                {
                    rows[currentY][currentX] = 'X';
                    distinctPositionsVisited++;
                }
            }
        }
    }

    enum Direction { N, E, S, W }
}
