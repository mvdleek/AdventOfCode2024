

namespace day_16
{
    // https://adventofcode.com/2024/day/16
    // maze is 141x141
    // find shortest path from S to E 141x141=19881 possibe locationa
    //
    // @630 start readuing
    // @642 start coding p1
    // "716 finish coding p1

    internal class Program
    {
        static void Main(string[] args)
        {
            var filename = "input_example.txt";

            var gridPart1 = new GridPart1(filename);

            gridPart1.SolvePart1();
        }
    }

    class GridPart1
    {
        public int width;
        public int height;
        public int startX;
        public int startY;
        public int endX;
        public int endY;
        public long lowestCost = long.MaxValue;
        public int[] rotateRight = [1, 2, 3, 0];
        public int[] rotateLeft = [3, 0, 1, 2];

        public List<char[]> rows = [];

        public List<int> moves = []; // N=0,E=1,S=2,W=3

        public GridPart1(string filename)
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
                    if (row[i] == 'S')
                    {
                        startX = i;
                        startY = rows.Count;
                    }
                    if (row[i] == 'E')
                    {
                        endX = i;
                        endY = rows.Count;
                    }
                }
                rows.Add(row);
            }
            height = rows.Count;
        }

        private void SetSquare(int x, int y, char c)
        {
            rows[y][x] = c;
        }

        private char GetSquare(int x, int y)
        {
            return rows[y][x];
        }

        private int GetMoveDirectionX(int move)
        {
            if (move == 1)
            {
                return 1;
            }
            if (move == 3)
            {
                return -1;
            }
            return 0;
        }

        private int GetMoveDirectionY(int move)
        {
            if (move == 0)
            {
                return -1;
            }
            if (move == 2)
            {
                return 1;
            }
            return 0;
        }


        internal void SolvePart1()
        {
            lowestCost = long.MaxValue;
            FindCheapestPathToE(startX, startY, 0, moveDir: 1); //bug 1  startX instead of startY 

            Console.WriteLine($"Part 1: { lowestCost }");
    }

        private void FindCheapestPathToE(int x, int y, long currentCost, int moveDir)
        {
            var sq = GetSquare(x, y);
            Console.WriteLine($"SquARE {sq}");
            if (sq == 'E')
            {
                if (currentCost < lowestCost)
                {
                    lowestCost = currentCost;
                }
                return;
            }

            SetSquare(x, y, 'X');

            // try 3 directions
            var directionsToTry = new int[3];
            directionsToTry[0] = moveDir;
            directionsToTry[1] = rotateLeft[moveDir];
            directionsToTry[2] = rotateRight[moveDir]; //bug 2: [1] instead of [2]

            for (int i = 0; i < 3; i++)
            {
                var newDir = directionsToTry[i];

                var dx = GetMoveDirectionX(newDir);
                var dy = GetMoveDirectionY(newDir);
                var newX = x + dx;
                var newY = y + dy;

                var newSq = GetSquare(newX, newY);

                if (newSq == '.') // untried square                
                {
                    Console.WriteLine($"Step { newDir}");
                    long newCost = currentCost + 1;
                    if (i != 0)
                    {
                        newCost += 1000;
                    }

                    FindCheapestPathToE(newX, newY, newCost, newDir);
                }
            }

            SetSquare(x, y, '.');
        }
    }

}
