


namespace day_14
{
    // input 500 bots, field 101x103, after 100 seconds
    // p=0,4 v=3,-3  v=per second
    // calculate safety factor: multiple robots in each of 4 quadrants
    // quadrant=50x50

    // @1200 start coding
    // @1232 finished coding
    // @1247 p1 example ok = 12
    // @226548000
    //

    internal class Program
    {
        static void Main(string[] args)
        {
            //var filename = "input_example.txt"; // 11x7
            var filename = "input.txt"; // 101X103

            var botCollection = BotCollection.ReadInput(filename);
            botCollection.SolvePart1(101, 103);
        }
    }

    class Bot
    {
        public int x;
        public int y;
        public int dx;
        public int dy;

        public Bot(string definition)
        {
            //p = 0,4 v = 3,-3//bug1 incorrect format
            //p=0,4 v=3,-3
            var parts = definition.Split(' ');
            var p = parts[0].Split('=');
            var v = parts[1].Split('=');
            var positionParts = p[1].Split(',');
            var velocityParts = v[1].Split(',');
            x = int.Parse(positionParts[0]);
            y = int.Parse(positionParts[1]);
            dx = int.Parse(velocityParts[0]);
            dy = int.Parse(velocityParts[1]);
        }

        internal void Move(int width, int height)
        {
            // 11x7 area
            // p=2,4 v=2,-3
            // 1 p=4,1
            // 2 p=6,-2 --> p=6,5   ? -2 % 7 =?= 5 --> no, it is -2 !!
            // 3 p=8,2
            // 4 p=10,-1 -> p=10,6
            // 5 p=12,3 -> p=1,3
            var safeX = x + dx;   
            var safeY = y + dy;  
            while (safeX < 0)
            {
                safeX += width;// + width to account for negative issue
            }
            while (safeY < 0)
            {
                safeY += height;// + height to account for negative issue 
            }
            x = (safeX) % width;
            y = (safeY) % height;
        }
    }

    class BotCollection
    {
        public List<Bot> bots = [];

        public static BotCollection ReadInput(string filename)
        {
            var botCollection = new BotCollection();
            botCollection.bots = File.ReadAllLines(filename).Select(line => new Bot(line)).ToList();
            return botCollection;
        }

        internal void SolvePart1(int width, int height)
        {
            // 100 seconds
            for (int i = 0; i < 100; i++)
            {
                MoveBots(width, height);
            }

            var quadrantWidth = width / 2;
            var quadrantHeight = height / 2;

            //bug 1: quadrant should be leftTop, rightBottom 
            //var safetyFactor =
            //    CountBotsInQuadrant(0, 0, quadrantWidth-1, quadrantHeight-1) *
            //    CountBotsInQuadrant(width-1, 0, width-quadrantWidth, quadrantHeight-1) *
            //    CountBotsInQuadrant(0, height-1, quadrantWidth-1, height-quadrantHeight) *
            //    CountBotsInQuadrant(width-1, height-1, width-quadrantWidth, height-quadrantHeight);

            var safetyFactor =
                CountBotsInQuadrant(0, 0, quadrantWidth, quadrantHeight) *
                CountBotsInQuadrant(width - quadrantWidth, 0, quadrantWidth, quadrantHeight) *
                CountBotsInQuadrant(0, height - quadrantHeight, quadrantWidth, quadrantHeight) *
                CountBotsInQuadrant(width - quadrantWidth, height - quadrantHeight, quadrantWidth, quadrantHeight);

            Console.WriteLine($"Part1: Safety factor = {safetyFactor}");
        }

        private int CountBotsInQuadrant(int x, int y, int qWidth, int qHeight)
        {
            var numberOfBots = 0;
            var x2 = x + qWidth;
            var y2 = y + qHeight;
            foreach (var bot in bots)
            {
                if (bot.x >= x && bot.x < x2 &&
                    bot.y >= y && bot.y < y2)
                {
                    numberOfBots++;
                }
            }
            return numberOfBots;
        }

        private void MoveBots(int width, int height)
        {
            foreach (var bot in bots)
            {
                bot.Move(width, height);
            }
        }
    }
}
