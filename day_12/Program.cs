namespace day_12
{
    // https://adventofcode.com/2024/day/12
    // input = 140x140, A to Z plant types

    // @710 read & think & design algo on paper 
    // @728 p1 start coding (copy grid reader)
    // @730 stop to eat and work

    // @1845 start coding
    // @1915 p1 1304764 running first time correct!
    // @2003 p2 811148 


    internal class Program
    {
        static void Main(string[] args)
        {
            var filename = "input.txt";

            var grid = Grid.ReadInput(filename);

            grid.Solve();
        }
    }

    class Grid
    {
        List<char[]> rows = new();

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

        public static Grid ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);

            return new Grid(lines);
        }

        internal void Solve()
        {
            var totalPrice1 = 0;
            var totalPrice2 = 0;
            for (int x = 0; x < rows[0].Length; x++)
            {
                for (int y = 0; y < rows.Count; y++)
                {
                    var plantType = rows[y][x];
                    if (plantType != '@' && char.IsUpper(plantType))
                    {
                        // unvisited square found
                        FenceCollection fenceCollection = new();

                        (var area, var fence) = CalculatePriceForRegion(plantType, x, y, fenceCollection);

                        var sides = fenceCollection.CalculateNumberOfSides();

                        totalPrice1 += area * fence;
                        totalPrice2 += area * sides;
                    }
                }
            }

            Console.WriteLine($"Total price part 1 = {totalPrice1}");
            Console.WriteLine($"Total price part 2 = {totalPrice2}");
        }

        private (int area, int fence) CalculatePriceForRegion(char plantType, int x, int y, FenceCollection fenceCollection)
        {
            var area = 1;
            var fence = 0;

            if (!SqHasPlantType(plantType, x, y - 1)) { fence++; fenceCollection.AddFence(0, x, y); }
            if (!SqHasPlantType(plantType, x + 1, y)) { fence++; fenceCollection.AddFence(1, x, y); }
            if (!SqHasPlantType(plantType, x, y + 1)) { fence++; fenceCollection.AddFence(2, x, y); }
            if (!SqHasPlantType(plantType, x - 1, y)) { fence++; fenceCollection.AddFence(3, x, y); }

            rows[y][x] = char.ToLower(plantType);

            if (SqHasPlantTypeAndIsUnvisited(plantType, x, y - 1)) { (var a, var f) = CalculatePriceForRegion(plantType, x, y - 1, fenceCollection); area += a; fence += f; }
            if (SqHasPlantTypeAndIsUnvisited(plantType, x + 1, y)) { (var a, var f) = CalculatePriceForRegion(plantType, x + 1, y, fenceCollection); area += a; fence += f; }
            if (SqHasPlantTypeAndIsUnvisited(plantType, x, y + 1)) { (var a, var f) = CalculatePriceForRegion(plantType, x, y + 1, fenceCollection); area += a; fence += f; }
            if (SqHasPlantTypeAndIsUnvisited(plantType, x - 1, y)) { (var a, var f) = CalculatePriceForRegion(plantType, x - 1, y, fenceCollection); area += a; fence += f; }

            return (area, fence);
        }

        private bool SqHasPlantType(char plantType, int x, int y)
        {
            return char.ToUpper(rows[y][x]) == plantType;
        }

        private bool SqHasPlantTypeAndIsUnvisited(char plantType, int x, int y)
        {
            return rows[y][x] == plantType;
        }
    }

    class FenceCollection
    {
        List<Fence>[] fences = new List<Fence>[4];

        public FenceCollection()
        {
            for (int i = 0; i < 4; i++) //bug 1 ,3 instead of 4
            {
                fences[i] = new List<Fence>();
            }
        }

        public void AddFence(int sideType, int x, int y)
        {
            fences[sideType].Add(new Fence { sideType = sideType, x = x, y = y });
        }

        public int CalculateNumberOfSides()
        {
            return CalculateNumberOfSidesInHFenceList(fences[0])
                + CalculateNumberOfSidesInVFenceList(fences[1])
                + CalculateNumberOfSidesInHFenceList(fences[2])
                + CalculateNumberOfSidesInVFenceList(fences[3]);
        }

        private int CalculateNumberOfSidesInVFenceList(List<Fence> fences)
        {
            if (fences.Count == 1) { return 1; }

            var sideCount = fences.Count;

            var xPositions = fences.GroupBy(f => f.x);

            foreach (var item in xPositions)
            {
                var fencesAtXsorted = item.OrderBy(f => f.y).ToList();

                // bepaal aantal aansluitende 
                for (int i = 1; i < fencesAtXsorted.Count; i++)
                {
                    if (fencesAtXsorted[i-1].y + 1 == fencesAtXsorted[i].y) //bug 2 -1 etc
                    {
                        sideCount--;
                    }
                }
            }
            return sideCount;
        }
        private int CalculateNumberOfSidesInHFenceList(List<Fence> fences)
        {
            if (fences.Count == 1) { return 1; }

            var sideCount = fences.Count;

            var yPositions = fences.GroupBy(f => f.y);

            foreach (var item in yPositions)
            {
                var fencesAtYsorted = item.OrderBy(f => f.x).ToList();

                // bepaal aantal aansluitende 
                for (int i = 1; i < fencesAtYsorted.Count; i++)
                {
                    if (fencesAtYsorted[i-1].x + 1 == fencesAtYsorted[i].x)
                    {
                        sideCount--;
                    }
                }
            }
            return sideCount;
        }
    }

    class Fence
    {
        public int sideType; // N=0,E=1,S=2,W=3
        public int x;
        public int y;
    }

    class VSide
    {
        public int yTop;
        public int yBottom;
        public int x;
    }

    class HSide
    {
        public int xLeft;
        public int xRight;
        public int y;
    }
}
