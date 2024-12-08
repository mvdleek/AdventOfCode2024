

using System.Diagnostics;

namespace day_06
{
    internal class Program
    {
        // start@17:40
        // p1   @18:55
        // p2   thinking about solution until 19:45
        // dinner !! 20:10 continue coding
        // p2 finished @2130
        //    p2 again: currentY = startX;
        //    1 array init lost/forgotten
        //    use p1.makestep ipv p2.makestep
        //    used ^ instead of &
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            //var filename = "input_example.txt"; // 41  only 1 bug 'OldY=CurrentX' p2=6
            var filename = "input.txt"; // 4778 2nd time right   only 1 bug 'OldY=CurrentX'  p2=1618

            var gridP1 = ReadInput(filename);

            while (gridP1.IsGuardOnMap)
            {
                gridP1.MakeStep();
            }

            Console.WriteLine($"Distinct positions visited: {gridP1.distinctPositionsVisited}");

            var visitedPositionsInPart1 = gridP1.GetListOfVisitedPositions();
            var gridP2 = ReadInputP2(filename);

            var obstructionPositionCount = 0;
            foreach (var visitedPosition in visitedPositionsInPart1)
            {
                gridP2.Reset();
                gridP2.PutObstacle(visitedPosition.x, visitedPosition.y);

                while (gridP2.IsGuardOnMap && !gridP2.IsLoopDetected)
                {
                    gridP2.MakeStep();
                }

                if (gridP2.IsGuardOnMap && gridP2.IsLoopDetected)
                {
                    obstructionPositionCount++;
                }
            }
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
            Console.WriteLine($"Different obstruction positions: {obstructionPositionCount}");
        }

        private static GridP1 ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);

            return new GridP1(lines);
        }
        private static GridP2 ReadInputP2(string filename)
        {
            var lines = File.ReadAllLines(filename);

            return new GridP2(lines);
        }
    }

    // p2 must get cycles!
    // 130 * 130 = 16900 --->  only visited positions on the 1st path need to be considerd for placement of blocks !!! so 4778 +/- 1

    // how to detect if you are on a loop?
    //   step onto a square with | or - mark matching current direction (| = N/S - = W/E), + matches all NOOOOO
    //
    //   mark each sqaure with travel direction flags. When you step on a sqaure that has been travelled before with same
    //  direction, then your are guaranteed to be on a loop.
    //
    //
    //  funny config: bouncy forever between A and B
    //       #
    //       A#
    //       | 
    //       | 
    //      #B----<
    //       #
    //  
    //  this shows the need to see what direction was travelled on that square before
    //       #
    //       A#
    //       | 
    //       | 
    //      #B----<
    //       
    //   in part 2 if start is like this:
    //
    //   #^#
    //    #
    //
    //   will it be ok in part 2 if you get like this
    //
    //    O
    //   #^#
    //    #

    class GridP1
    {
        int currentX = -1;
        int currentY = -1;
        Direction currentD;

        List<char[]> rows = new();

        public int distinctPositionsVisited;

        public bool IsGuardOnMap => GetSquare(currentX, currentY) != '@';

        public GridP1(string[] lines)
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

        public List<(int x, int y)> GetListOfVisitedPositions()
        {
            var visitedPositions = new List<(int, int)>();
            for (var x = 0; x < rows[0].Length; x++)
            {
                for (var y = 0; y < rows.Count; y++)
                {
                    if (rows[y][x] == 'X')
                    {
                        visitedPositions.Add((x, y));
                    }
                }

            }
            return visitedPositions;
        }
    }


    class GridP2
    {
        int currentX = -1;
        int currentY = -1;
        Direction currentD;

        List<char[]> rows = new();

        private int[] directionMarks; // 
        private bool loopDetected;
        private int width;
        private int startX;
        private int startY;
        private int obstacleX;
        private int obstacleY;

        public bool IsGuardOnMap => GetSquare(currentX, currentY) != '@';
        public bool IsLoopDetected => loopDetected;

        public GridP2(string[] lines)
        {
            width = lines[0].Length + 2;

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
                        startX = 1 + i;
                        startY = rows.Count;
                        c = '.';
                    }

                    row[1 + i] = c;
                }

                rows.Add(row);
            }

            rows.Add(new string('@', width).ToArray());
            directionMarks = new int[width * rows.Count];

            Reset();
        }

        private int GetDirBit(Direction dir)
        {
            switch (dir)
            {
                case Direction.N:
                    return 1;
                case Direction.E:
                    return 2;
                case Direction.S:
                    return 4;
                case Direction.W:
                    return 8;
                default:
                    throw new ArgumentException();
            }
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

                PutDirectionMark(currentX, currentY, currentD);

                MakeStep(); 
            }
            else
            {
                if (sq != '@') 
                {
                    if (HasDirectionMark(currentX, currentY, currentD))
                    {
                        loopDetected = true;
                    }
                    else
                    {
                        PutDirectionMark(currentX, currentY, currentD);
                    }
                }

            }
        }

        private bool HasDirectionMark(int currentX, int currentY, Direction currentD)
        {
            return (directionMarks[currentX + currentY * width] & GetDirBit(currentD)) != 0;
        }

        private void PutDirectionMark(int currentX, int currentY, Direction currentD)
        {
            directionMarks[currentX + currentY * width] |= GetDirBit(currentD);
        }

        internal void Reset()
        {
            currentX = startX;
            currentY = startY;
            currentD = Direction.N;

            Array.Fill(directionMarks, 0);
            PutDirectionMark(currentX, currentY, currentD);

            PutDirectionMark(currentX, currentY, currentD);

            loopDetected = false;

            if (obstacleX != 0)
            {
                rows[obstacleY][obstacleX] = '.';
                obstacleX = 0;
                obstacleY = 0;
            }
        }

        internal void PutObstacle(int x, int y)
        {
            rows[y][x] = '#';
            obstacleX = x;
            obstacleY = y;
        }
    }

    enum Direction { N, E, S, W }
}
