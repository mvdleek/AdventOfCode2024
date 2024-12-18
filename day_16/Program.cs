


namespace day_16
{
    // https://adventofcode.com/2024/day/16
    // maze is 141x141
    // find shortest path from S to E 141x141=19881 possibe locations maar ! many cycles, recursive descent not possible
    // cheapest path calls for dijkstra
    //
    // @630 start readuing
    // @642 start coding p1
    // "716 finish coding p1
    // 741 p1 must use dijkstra 
    // break
    // 1215 1300 dijkstra
    // 1630 dijkstra 
    // p1 minimum cost to get from S to E 83444 
    // p2 540 = TOO HIGH   (p1=64)
    // p2 506 = TOO HIGH ?!?
    // p2 445 not correct :-(

    internal class Program
    {
        static void Main(string[] args)
        {
            //var filename = "input_example.txt";
            var filename = "input.txt";

            var gridPart1 = new GridPart1(filename);

            gridPart1.SolvePart1_Dijkstra();
            //gridPart1.PrintProcessed();

            //gridPart1.SolvePart2();
            gridPart1.SolvePart2SolveBrute(83444);

            //gridPart1.Print();
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
        public int[] oppositeDir = [2, 3, 0, 1];

        public List<char[]> rows = [];
        public Node[,] nodes;

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

            nodes = new Node[width, height];
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

            Console.WriteLine($"Part 1: {lowestCost}");
        }

        internal void SolvePart1_Dijkstra()
        {
            var allNodes = new List<Node>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var sq = GetSquare(x, y);

                    if (sq == '#') { continue; }

                    var nn = new Node(x, y, sq);
                    allNodes.Add(nn);
                    SetSquareNode(x, y, nn);
                    if (sq == 'S')
                    {
                        nn.distance = 0;
                        nn.direction = 1; //E
                    }
                }
            }

            var myPrioQueue = new PrioQueue(allNodes);

            var n = myPrioQueue.GetAndRemoveNodeWithLowestDistance();
            while (n.sq != 'E')
            {
                if (n.x == 5 && n.y == 13)
                {

                }
                if (n.x == 15 && n.y == 8)
                {

                }
                for (int i = 0; i < 4; i++)
                {
                    if (i == oppositeDir[n.direction])
                    {
                        continue;
                    }

                    var dx = GetMoveDirectionX(i);
                    var dy = GetMoveDirectionY(i);

                    var nx = n.x + dx;
                    var ny = n.y + dy;

                    if (GetSquare(nx, ny) == '#')
                    {
                        continue;
                    }

                    var neighbour = GetSquareNode(nx, ny);

                    var distThroughN = n.distance + 1;
                    if (i != n.direction)
                    {
                        distThroughN += 1000;
                    }

                    if (neighbour.processed) 
                    {
                        if (distThroughN == neighbour.distance)
                        {
                            neighbour.inDirections.Add(i);
                            n.outDirections.Add(i);
                        }
                        else if (neighbour.distance < distThroughN) //if (i == n.direction && (neighbour.inDirections.Count > 2 || (neighbour.outDirections.Count > 0 && neighbour.inDirections[0] != neighbour.outDirections[0])))
                        {
                            if (neighbour.outDirections.Contains(n.direction))
                            {
                                var nextnextX = nx + dx;
                                var nextnextY = ny + dy;

                                var nextnextNode = GetSquareNode(nextnextX, nextnextY);
                                if (nextnextNode.distance == distThroughN + 1)
                                {
                                    neighbour.inDirections.Add(i);
                                    neighbour.forcedInDirections.Add(i);
                                    n.outDirections.Add(i);
                                }
                            }
                        }

                        continue; 
                    }


                    if (distThroughN < neighbour.distance)
                    {
                        neighbour.inDirections.Clear();
                        n.outDirections.Add(i);
                        var oldDistance = neighbour.distance;
                        neighbour.distance = distThroughN;
                        neighbour.direction = i;
                        neighbour.inDirections.Add(i);
                        myPrioQueue.Resort(neighbour, oldDistance);
                    }
                    else if (distThroughN == neighbour.distance)
                    {
                        n.outDirections.Add(i);
                        neighbour.inDirections.Add(i);
                    }
                }
                n.processed = true;
                n = myPrioQueue.GetAndRemoveNodeWithLowestDistance();
            }

            Console.WriteLine($"Part 1: {n.distance}");
        }

        private Node GetSquareNode(int x, int y)
        {
            return nodes[x, y];
        }

        private void SetSquareNode(int x, int y, Node n)
        {
            nodes[x,y] = n;
        }

        private void FindCheapestPathToE(int x, int y, long currentCost, int moveDir)
        {
            var sq = GetSquare(x, y);
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

                if (newSq == '.' || newSq == 'E') // untried square            // bug 3: forgot 'E'
                {
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

        internal void SolvePart2()
        {
            // Find all paths from E to S
            // E holds all directions that were from shortest paths in directions
            // walk this paths in reverse order from E to S, marking visited nodes

            WalkAllPathsBackwardsFromXY(endX, endY);

            var oTileCount = 1; // S square is never marked 'O'
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (GetSquare(x, y) == 'O')
                    {
                        oTileCount++;
                    }
                }
            }

            Console.WriteLine($"Part 2 : { oTileCount }");
        }

        private void WalkAllPathsBackwardsFromXY(int x, int y)
        {
            SetSquare(x, y, 'O');

            var node = GetSquareNode(x, y);

            foreach (var dir in node.inDirections)
            {
                if (dir == 1)
                {

                }
                var backDir = oppositeDir[dir];

                var dx = GetMoveDirectionX(backDir);
                var dy = GetMoveDirectionY(backDir);

                var newX = x + dx;
                var newY = y + dy;

                var sq = GetSquare(newX, newY);
                var bn = GetSquareNode(newX, newY);
                if (bn.distance < node.distance /*|| node.forcedInDirections.Contains(dir)*/)
                {
                    if (sq == 'S')
                    {
                        return;
                    }

                    if (sq != 'O')
                    {
                        WalkAllPathsBackwardsFromXY(newX, newY);
                    }
                }
            }
        }

        internal void Print()
        {
            for (int y = 0; y < height; y++)
            {
                var row = new char[width];
                for (int x = 0; x < width; x++)
                {
                    row[x] = GetSquare(x, y);
                }
                var s = new string(row);
                Console.WriteLine(y.ToString().PadRight(3) + s);
            }
            Console.WriteLine("   012345678901234567890");
        }

        internal void PrintProcessed()
        {
            for (int y = 0; y < height; y++)
            {
                var row = new char[width];
                for (int x = 0; x < width; x++)
                {
                    var n = GetSquareNode(x, y);
                    row[x] = GetSquare(x, y);
                    if (row[x] == '.' && n.processed)
                    {
                        row[x] = '*';
                    }
                }
                var s = new string(row);
                Console.WriteLine(y.ToString().PadRight(3) + s);
            }
            Console.WriteLine("   012345678901234567890");
        }

        internal void SolvePart2SolveBrute(int cutoff)
        {
            WalkTheGrid(startX, startY, 0, 1);
        }
    }

    internal class PrioQueue
    {
        private SortedList<long, List<Node>> allNodes;

        public PrioQueue(List<Node> allNodes)
        {
            this.allNodes = new SortedList<long, List<Node>>
            {
                { 0, [] },
                { long.MaxValue, [] }
            };

            var zeroList = this.allNodes[0];
            var maxList = this.allNodes[long.MaxValue];

            foreach (var item in allNodes)
            {
                if (item.distance == 0)
                {
                    zeroList.Add(item);
                }
                else if (item.distance == long.MaxValue)
                {
                    maxList.Add(item);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        internal Node GetAndRemoveNodeWithLowestDistance()
        {
            var n = allNodes.First();

            var node = n.Value[0];
            n.Value.RemoveAt(0);

            if (n.Value.Count == 0)
            {
                allNodes.RemoveAt(0);
            }

            return node;
        }

        internal void Resort(Node neighbour, long oldDistance)
        {
            var z = allNodes[oldDistance];
            var k = allNodes.IndexOfKey(oldDistance);

            z.Remove(neighbour);
            if (z.Count == 0)
            {
                allNodes.Remove(oldDistance);
            }

            if (!allNodes.ContainsKey(neighbour.distance))
            {
                allNodes.Add(neighbour.distance, []);
            }
            allNodes[neighbour.distance].Add(neighbour);
        }
    }

    internal class Node
    {
        public int x;
        public int y;
        public char sq;
        public long distance = long.MaxValue;
        public int direction;
        public List<int> inDirections = []; // all previous neighbours with same distance to start
        public List<int> forcedInDirections = []; // all previous that must be followed when walking back
        public List<int> outDirections = []; // all previous neighbours with same distance to start
        public Node?[] neighbours = [null, null, null, null]; //N E S W neighbours
        public bool processed = false;

        public Node(int x, int y, char sq)
        {
            this.x = x;
            this.y = y;
            this.sq = sq;
        }
    }
}
