namespace day_15
{
    // https://adventofcode.com/2024/day/15

    // @955 start reading
    // @1000 start coding
    // @1100 finished coding
    // p1 example ok
    // p1 1476771 ok : 0 bugs, first time right !!
    // @1103 reading p2
    // @1108 break

    internal class Program
    {
        static void Main(string[] args)
        {
            var filename = "input.txt";

            var grid = new Grid(filename);

            grid.SolvePart1();
        }
    }

    class Grid
    {
        public int width;
        public int height;
        public int robotX;
        public int robotY;

        public List<char[]> rows = [];

        public List<int> moves = []; // N=0,E=1,S=2,W=3

        public Grid(string filename)
        {
            ReadInput(filename);
        }

        public void ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var inMap = true;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (!inMap) { break; }

                    inMap = false;
                }

                if (inMap)
                {
                    if (width == 0)
                    {
                        width = line.Length;
                    }
                    var row = new char[width];
                    for (int i = 0; i < width; i++)
                    {
                        row[i] = line[i];
                        if (row[i] == '@')
                        {
                            robotX = i;
                            robotY = rows.Count;
                        }
                    }
                    rows.Add(row);
                }
                else
                {
                    // line of move instructions
                    for (int i = 0; i < line.Length; i++)
                    {
                        var move = MoveCharToDirection(line[i]);
                        moves.Add(move);
                    }
                }
            }
            height = rows.Count;
        }

        private int MoveCharToDirection(char v)
        {
            if (v == '^')
            {
                return 0;
            }
            if (v == '>')
            {
                return 1;
            }
            if (v == 'v')
            {
                return 2;
            }
            return 3;
        }

        internal void SolvePart1()
        {
            foreach (var move in moves)
            {
                MoveRobot(move);
            }

            CalculateResultPart1();
        }

        private void MoveRobot(int move)
        {
            var dx = GetMoveDirectionX(move);
            var dy = GetMoveDirectionY(move);

            var newX = robotX + dx;
            var newY = robotY + dy;

            if (GetSquare(newX, newY) == '#') { return; }

            if (GetSquare(newX, newY) == '.')
            {
                SetSquare(newX, newY, '@');
                SetSquare(robotX, robotY, '.');
                robotX = newX;
                robotY = newY;
                return;
            }

            // so there is a box on newX,newY
            //
            // Now: look behind the box until you find a wall or empty square
            // if find a wall, do nothing
            // if find an empty square, move all boxes + robot before
            //    this is: put a box on the empty sqaure, and put robot on newX, newY
            //

            var checkX = newX;
            var checkY = newY;
            do
            {
                checkX = checkX + dx;
                checkY = checkY + dy;
            } while (GetSquare(checkX, checkY) == 'O');

            if (GetSquare(checkX, checkY) == '#') { return; }

            // so checkX, checkY must be an empty square
            SetSquare(checkX, checkY, 'O');
            SetSquare(newX, newY, '@');
            SetSquare(robotX, robotY, '.');
            robotX = newX;
            robotY = newY;
        }

        private void SetSquare(int x, int y, char c)
        {
            rows[y][x] = c;
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

        private void CalculateResultPart1()
        {
            long sumOfGpsOfAllBoxes = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (GetSquare(x, y) == 'O')
                    {
                        var boxGps = x + y * 100;
                        sumOfGpsOfAllBoxes += boxGps;
                    }
                }
            }

            Console.WriteLine($"Part1 - Sum of all boxes' GPS coordinates: {sumOfGpsOfAllBoxes}");
        }

        private char GetSquare(int x, int y)
        {
            return rows[y][x];
        }
    }
}
