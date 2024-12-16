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

    // @1830 - @2000 p2
    // break
    // @2200 p2 18156 --> difficult to find 100% sure, just looked for pics with >= 50 in bottom 3 rows
    // @2215 p2 7753 ok
    // 
    // tough to find a way to detect a tree, what will be its shape? Is it solid or an outline?
    // i decided to write the seconds states to files, so i could view them more easily than printed on the console
    // (with preview it is easy to look at many files)
    // 
    // then i thought that the bottom line of the tree would contain many bots,
    // somewhat lucky i said that in 3 consecutive lines there should be > 50 bots
    // this still had many hits, but looking through them I stumbled upon 18156 -> it was not correct second, but it showed the outline
    //
    // then i had better criteria to detect the tree, finding 7753
    //
    // part 1 was easy
    // part 2 not easy, had to manual work to find solution

                                                                                                         
//      *                                   *******************************                            
//                                          *                             *                            
//                                          *                             *                            
//                                    *     *                             *                            
//                                          *                             *                            
//                             *            *              *              *                            
//                                          *             ***             *                            
//*   *                                     *            *****            *                            
//                      *                   *           *******           *                            
//                                          *          *********          *                            
//                   *                      *            *****            *                 *  *       
//                                          *           *******           *                            
//                            *             *          *********          *                  *        *
//                                          *         ***********         *          *                 
//                                          *        *************        *              *             
//                                          *          *********          *                            
//                                          *         ***********         *                            
//          *                               *        *************        *                            
// *                   *      *       *     *       ***************       *                            
//                                          *      *****************      *                            
//                                 *        *        *************        *                            
//       *                                  *       ***************       *              *             
//                        *                 *      *****************      *                          * 
//                                *         *     *******************     *                            
//                     *                    *    *********************    *                            
//                    *                     *             ***             *                            
//                                  *       *             ***             *                            
//                                      *   *             ***             *                            
//                            *             *                             *          *                 
//                                 *        *                             *                           *
//                                        * *                             *            *               
//                                          *                             *            *               
//      *                  * *              *******************************                    *       
                                                                                                     


    internal class Program
    {
        static void Main(string[] args)
        {
            //var filename = "input_example.txt"; // 11x7
            var filename = "input"; // 101X103
            //var filename = "input - Copy.txt"; // 8168

            var botCollection = BotCollection.ReadInput(filename);
            botCollection.SolvePart1(101, 103);

            var botCollection2 = BotCollection.ReadInput(filename);
            botCollection2.SolvePart2(101, 103);
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

        internal void SolvePart2(int width, int height)
        {
            if (!Directory.Exists(".\\output"))
            {
                Directory.CreateDirectory(".\\output");
            }
            for (int i = 0; i < 20_000; i++)
            {
                if (i % 1000 == 0)
                {
                    Console.WriteLine($"At seconds: {i + 1}");
                }

                MoveBots(width, height);

                if (DetectTree(height))
                {
                    Console.WriteLine($"Possible tree detected: {i + 1} seconds");
                    Print(i + 1, width, height);
                }
            }
        }

        private bool DetectTree(int height)
        {
            // detect large number of bots in 3 consective rows >= 50, still many hits many to find by hand 18154 from many files
            // used tree found with 18154 to set better search criterua (>= 95 in 5 rows between row 60 and 100, still many hits but found 7753

            // with 18154 these criteria would be best:
            // in 10 consequetive rows, detect 150 bots with x between 30 and 90  <---- this only finds 7753 and 18156
            //                                          *          *********       9   bots  *                            
            //                                          *         ***********      11  bots *                            
            //          *                               *        *************     13  bots *                            
            // *                   *      *       *     *       ***************    15  bots *                            
            //                                          *      *****************   17  bots *                            
            //                                 *        *        *************     13  bots *                            
            //       *                                  *       ***************    15  bots *              *             
            //                        *                 *      *****************   17  bots *                          * 
            //                                *         *     *******************  19  bots *                            
            //                     *                    *    ********************* 21  bots *                            











            int[] counts = new int[height];

            foreach (var b in bots)
            {
                if (b.y >= 0 && b.y < height)
                {
                    if (b.x >= 0 && b.x <= 102)
                    {
                        counts[b.y] += 1;
                    }
                }
            }
            var conseq = 10;
            var minSumForPossibleTree = 150;

            for (int i = (conseq /2)+1; i <= height - conseq; i++)
            {
                var sum = 0;
                for (int z = 0; z < conseq; z++)
                {
                    sum += counts[i+z];
                }
                if (sum >= minSumForPossibleTree)
                {
                    return true;
                }
            }

            return false;
        }

        private void Print(int seconds, int width, int height)
        {
            var lines = new List<string>();
            lines.Add($"After {seconds} seconds:");

            var row = new char[width];

            var botsPerY = bots.GroupBy(b => b.y).ToList();
            var d = new Dictionary<int, List<Bot>>();
            foreach (var b in botsPerY)
            {
                d.Add(b.Key, b.ToList());
;           }

            for (int i = 0; i < height; i++)
            {
                Array.Fill(row, ' ');
                if (d.ContainsKey(i))
                {
                    foreach (var b in d[i])
                    {
                        row[b.x] = '*';
                    }
                }
                var s = new string(row);
                lines.Add(s);
            }

            // write to file to enable manual inspection
            File.WriteAllLines($".\\output\\After {seconds.ToString().PadLeft(7, '0')}.txt", lines);
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
