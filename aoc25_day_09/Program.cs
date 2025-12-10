

namespace aoc25_day_09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = ReadInput("input.txt");

            Solve(input);

            var bm = new Bitmap(input);

            //bm.FloodFill(8, 2); // example
            //bm.FloodFill(50_000, 50_000); // real, maar een guess
            bm.FloodFill(0, 0); // dit zou dan een mask moeten worden om de echte fill te maken
            bm.DrawLines(input, true);
            Console.WriteLine($"0,0 ffilled = {bm.IsFilled(0, 0).ToString()}");



            SolveP2(input, bm);  // 164017360 too low

        }

        private static void SolveP2(List<Tile> tiles, Bitmap bm)
        {
            Console.WriteLine("solving p2");
            // just try all combinations
            long maxArea = 0;
            int count = 0;
            int checkedz = 0;
            for (int i = 0; i < tiles.Count - 1; i++)
            {
                for (int j = i + 1; j < tiles.Count; j++)
                {
                    
                    count++;
                    var t1 = tiles[i];
                    var t2 = tiles[j];
                    if (t1.x == t2.x || t1.y == t2.y)
                        continue; // can never be biggest

                    long area = (long)(1 + Math.Abs(t1.y - t2.y)) * (1 + Math.Abs(t1.x - t2.x)); // pfffff int*int = problem
                    if (area > maxArea)
                    {
                        checkedz++;
                        Console.WriteLine($"Checking rect {count}; checknr {checkedz}");
                        if (bm.IsCompletlyFilled(t1, t2))
                        {
                            maxArea = area;
                        }
                    }
                }

            }
            Console.WriteLine($"count = {count} (={/*495+494+..+ 1*/ (496 * 494 / 2) + 496 / 2})");
            Console.WriteLine($"maxarea = {maxArea}");

            Console.WriteLine("solved p2");  // after ~5/6 hours: 1534043700 and it was correct!!!!!
        }

        private static void Solve(List<Tile> tiles)
        {
            // just try all combinations
            long maxArea = 0;
            int count = 0;
            for (int i = 0; i < tiles.Count-1; i++)
            {
                for (int j = i+1; j < tiles.Count; j++)
                {
                    count++;
                    var t1 = tiles[i];
                    var t2 = tiles[j];
                    long area = (long)(1 + Math.Abs(t1.y - t2.y)) * (1 + Math.Abs(t1.x - t2.x)); // pfffff int*int = problem
                    if (area > maxArea)
                    {
                        maxArea = area;
                    }
                }

            }
            Console.WriteLine($"count = {count} (={/*495+494+..+ 1*/ (496 * 494/2)+496/2 })");
            Console.WriteLine($"maxarea = {maxArea}");  //2147462366 = fout, too low!

        }

        private static List<Tile> ReadInput(string v)
        {
            var tiles = new List<Tile>();
            var lines = File.ReadAllLines(v);

            foreach (var line in lines)
            {
                var t = new Tile(line);
                tiles.Add(t);
            }

            return tiles;
        }
    }

    class Tile { 

        public int x; 
        public int y;

        public Tile(string line)
        {
            var parts = line.Split(',');
            x = int.Parse(parts[0]);
            y = int.Parse(parts[1]);
        }

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

// 100.000 * 100.000 = 10.000 000 000 10G = 2GB bits
// 122.760 combis
// i can only think of bitmap and a floodfill
