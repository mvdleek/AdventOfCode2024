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
    // @1148 p2 start coding
    // @1254 p2 finish coding (even though i had the algorithm already from the start!)
    // p2 example ok
    // @1256 p2 1468005 ok : 0 bugs, first time right !!

    internal class Program
    {
        static void Main(string[] args)
        {
            var filename = "input.txt";

            var gridPart1 = new GridPart1(filename);

            gridPart1.SolvePart1();


            var gridPart2 = new GridPart2(filename);

            gridPart2.SolvePart2();
        }
    }

    class GridPart1
    {
        public int width;
        public int height;
        public int robotX;
        public int robotY;

        public List<char[]> rows = [];

        public List<int> moves = []; // N=0,E=1,S=2,W=3

        public GridPart1(string filename)
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

    class GridPart2
    {
        public int width;
        public int height;
        public int robotX;
        public int robotY;

        public List<char[]> rows = [];

        public List<int> moves = []; // N=0,E=1,S=2,W=3

        public GridPart2(string filename)
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
                        width = line.Length * 2;
                    }
                    var row = new char[width];
                    for (int i = 0; i < line.Length; i++)
                    {
                        var j = i * 2;
                        var c = line[i];

                        if (c == 'O')
                        {
                            row[j] = '[';
                            row[j + 1] = ']';
                        }
                        else if (c == '@')
                        {
                            row[j] = c;
                            row[j + 1] = '.';

                            robotX = j;
                            robotY = rows.Count;

                        }
                        else
                        {
                            row[j] = c;
                            row[j + 1] = c;
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

        internal void SolvePart2()
        {
            foreach (var move in moves)
            {
                MoveRobot(move);
            }

            CalculateResultPart2();
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
                SetSquare(robotX, robotY, '.');
                robotX = newX;
                robotY = newY;
                SetSquare(robotX, robotY, '@');
                return;
            }

            // so there is a box on newX,newY
            //
            // Now:

            // - If move direction is E or W:
            // look behind the box until you find a wall or empty square
            // if find a wall, do nothing
            // if find an empty square, move all boxes + robot before

            //- If move direction is N or S:
            // determine if the box can be moved
            //    look behind the box 
            //       - if it is all free space: yes can be moved
            //       - if it has a wall : no
            //       - it has one or two boxes: the box can be moved only if the other box(es) can be moved.
            //
            // if you maintain a list of all the boxes found when checking movebality (in found order) you can move
            // the boxes in reverse order (marking out the new free space too)
            //
            // this is recursive solution
            //
            // finally, move the robot.

            if (dy == 0)
            {
                // E or W, (newX, newY) is a box

                var checkX = newX;
                do
                {
                    checkX = checkX + dx;
                } while (GetSquare(checkX, robotY) == '[' || GetSquare(checkX, robotY) == ']');

                if (GetSquare(checkX, robotY) == '#') { return; }

                // so checkX, checkY must be an empty square, move the boxes
                while (checkX != robotX)
                {
                    SetSquare(checkX, robotY, GetSquare(checkX - dx, robotY));
                    SetSquare(checkX - dx, robotY, '.');

                    checkX = checkX - dx;
                }
                robotX = newX;
            }
            else
            {
                // N or S, (newX, newY) is a box

                if (MoveBox(newX, newY, dy, onlyCheck: true))
                {
                    MoveBox(newX, newY, dy, onlyCheck: false);

                    SetSquare(robotX, robotY, '.');
                    robotY = newY;
                    SetSquare(robotX, robotY, '@');
                }
            }
        }

        private bool MoveBox(int x, int y, int dy, bool onlyCheck)
        {
            // (x, y) is on part of a box that must (checked if can) be  moved
            if (GetSquare(x, y) == ']')
            {
                x = x - 1; // make x always be left part of the box
            }

            // (x, checkY) is the where the box should move
            var checkY = y + dy;

            var destLeft = GetSquare(x, checkY);
            var destRight = GetSquare(x + 1, checkY);

            bool canMove;

            if (destLeft == '#' || destRight == '#')
            {
                canMove = false;
            }
            else if (destLeft == '.' && destRight == '.')
            {
                canMove = true;
            }
            else if (destLeft == '[')
            {
                canMove = MoveBox(x, checkY, dy, onlyCheck);
            }
            else
            {
                canMove = true;
                if (destLeft == ']')
                {
                    canMove = canMove && MoveBox(x, checkY, dy, onlyCheck);
                }
                if (destRight == '[')
                {
                    canMove = canMove && MoveBox(x + 1, checkY, dy, onlyCheck);
                }
            }

            if (canMove && !onlyCheck)
            {
                SetSquare(x, checkY, '[');
                SetSquare(x + 1, checkY, ']');
                SetSquare(x, y, '.');
                SetSquare(x + 1, y, '.');
            }


            return canMove;
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

        private void CalculateResultPart2()
        {
            long sumOfGpsOfAllBoxes = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (GetSquare(x, y) == '[')
                    {
                        var boxGps = x + y * 100;
                        sumOfGpsOfAllBoxes += boxGps;
                    }
                }
            }

            Console.WriteLine($"Part2 - Sum of all boxes' GPS coordinates: {sumOfGpsOfAllBoxes}");
        }

        private char GetSquare(int x, int y)
        {
            return rows[y][x];
        }
    }
}
