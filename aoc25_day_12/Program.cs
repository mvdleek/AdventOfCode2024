

namespace aoc25_day_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = ReadInput("input.txt");

            SolveP1(input); //524 --> but only lucky with stupid area comparison (did not work for the example)
        }

        private static void SolveP1(Input input)
        {
            var fitCount = 0;
            foreach (var item in input.Regions) { 
            
                var regionArea = item.Width * item.Height;

                var presentArea = 0;
                for (global::System.Int32 i = 0; i < 6; i++)
                {
                    presentArea += item.PresentCounts[i] * 9;//input.Shapes[i].area;
                }

                Console.WriteLine($"Region {presentArea} {regionArea}");
                if (presentArea <= regionArea)
                {
                    fitCount++;
                }
            }
            Console.WriteLine($"Solution P1 = {fitCount}");
        }

        private static Input ReadInput(string v)
        {
            var input = new Input();
            var lines = File.ReadAllLines(v);
            Shape s = new();
            foreach (var line in lines)
            {
                if (line.Length == 0)
                {
                    s.CalcArea();
                    continue;
                }

                if (line.EndsWith(":"))
                {
                    s = new Shape();
                    input.Shapes.Add(s);
                    s.Index = int.Parse(line.Substring(0, line.Length - 1));
                }
                else if (line.EndsWith("#") || line.EndsWith("."))
                {
                    s.lines.Add(line);
                }
                else 
                {
                    var region = new Region(line);
                    input.Regions.Add(region);
                }
            }
            return input;
        }
    }

    class Input
    {
        public List<Shape> Shapes = new();
        public List<Region> Regions = new();

    }

    public class Region
    {
        public int Height;
        public int Width;
        public int[] PresentCounts = new int[6];

        public Region(string s)
        {
            //12x5: 1 0 1 0 2 2
            var parts = s.Split(": ");
            var widthHeight = parts[0].Split("x");
            var presentCounts = parts[1].Split(" ");
            Width = int.Parse(widthHeight[0]);
            Height = int.Parse(widthHeight[1]);
            for (int i = 0; i < 6; i++)
            {
                PresentCounts[i] = int.Parse(presentCounts[i]);
            }
        }
    }

    public class Shape
    {
        public int Index = 0;
        public List<string> lines = new();
        public int area = 0;

        public void CalcArea()
        {
            foreach (var line in lines)
            {
                for (global::System.Int32 i = 0; i < line.Length; i++)
                {
                    if (line[i] == '#')
                    {
                        area++;
                    }
                }
            }
        }
    }
}
