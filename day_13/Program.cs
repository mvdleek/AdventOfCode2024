using System.Reflection.PortableExecutable;

namespace day_13
{
    // input heeft 320 machines
    // https://adventofcode.com/2024/day/13
    //
    // @700 - 715 read and think about solution (choice of dijkstra or brute force)
    // @1915 start coding
    // @2000 finished coding
    // @2006 p1 example ok
    // @2007 p1 31589
    //       start on p2
    // @2015 little break
    // @2130 start part 2 (thinking before, it lead me 2 believe it is just intersection of 2 lines)
    // @2230 part 2 finished coding
    // @2315 part 2 ok 875318608908 (only stupid bugs, algo idea was good, and )
    // @2317 part really ok: forgot to use input.txt 98080815200063
    internal class Program
    {
        public static bool part2 = true;

        static void Main(string[] args)
        {
            var filename = "input.txt";
            
            var allMachines = ClawMachine.ReadInput(filename);

            long totalTokens = 0;

            foreach (var machine in allMachines)
            {
                long minTokensForMachine = long.MaxValue;

                if (part2)
                {
                    minTokensForMachine = machine.SolveWithIntersectingLines();
                }
                else
                {
                    int solcount = 0;
                    var minTokensForMachineP2 = machine.SolveWithIntersectingLines();

                    for (int a = 0; a < 100; a++)
                    {
                        for (int b = 0; b < 100; b++)
                        {
                            if (a * machine.buttonA.dx + b * machine.buttonB.dx == machine.prize.x &&
                                a * machine.buttonA.dy + b * machine.buttonB.dy == machine.prize.y)
                            {
                                var tokensNeeded = a * 3 + b * 1;

                                if (tokensNeeded < minTokensForMachine)
                                {
                                    machine.p1_Acount = a;
                                    machine.p1_Bcount = b;

                                    minTokensForMachine = tokensNeeded;
                                    solcount++;
                                }
                            }
                        }
                    }

                    if (minTokensForMachineP2 != minTokensForMachine)
                    {
                        //_ = machine.SolveWithIntersectingLines();

                        machine.WriteDef();
                        Console.WriteLine($"Puzzle SolCount P1={minTokensForMachine}");
                        Console.WriteLine($"Puzzle SolCount P2={minTokensForMachineP2}");
                    }
                }

                if (minTokensForMachine != long.MaxValue)
                {
                    totalTokens += minTokensForMachine;

                    //if (solcount == 1)
                    //{
                    //    machine.WriteDef();
                    //    Console.WriteLine($"Puzzle SolCount={solcount}");
                    //}
                }
            }

            Console.WriteLine($"Fewest tokens to win all possible prizes: {totalTokens}");
        }
    }

    class ClawMachine
    {
        public int index;
        public List<string> defLines = new();
        public Button buttonA;
        public Button buttonB;

        public Prize prize;
        internal int p1_Acount;
        internal int p1_Bcount;

        public static ClawMachine[] ReadInput(string filename)
        {
            var clawMachines = new List<ClawMachine>();

            var lines = File.ReadAllLines(filename);

            var machine = new ClawMachine();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(' ');
                if (parts[0] == "Button")
                {
                    machine.defLines.Add(line);
                    var btn = new Button();
                    btn.dx = parts[2].ExtractNumber();
                    btn.dy = parts[3].ExtractNumber();

                    if (parts[1] == "A:")
                    {
                        machine.buttonA = btn;
                    }
                    else
                    {
                        machine.buttonB = btn;
                    }
                }

                if (parts[0] == "Prize:")
                {
                    machine.defLines.Add(line);

                    var prize = new Prize();
                    prize.x = parts[1].ExtractNumber();
                    prize.y = parts[2].ExtractNumber();

                    if (Program.part2)
                    {
                        prize.x += 10_000_000_000_000;
                        prize.y += 10_000_000_000_000;
                    }

                    machine.prize = prize; //bug 1 forgot this

                    machine.index = clawMachines.Count; 
                    clawMachines.Add(machine);

                    machine = new ClawMachine();
                }
            }

            return clawMachines.ToArray();
        }

        public long SolveWithIntersectingLines()
        {
            long tokensNeeded = long.MaxValue;
            // intersection of 2 lines
            // 1 starting from (0,0) with A slope moving up to the right
            // 1 starting from prize with B slope moving down to the left
            // if intersection @ whole integer coords : solution found
            //
            // line a :    y = (a.dy/a.dx) * x
            // line b :    y = (b.dy/b.dx) * (x - p.x) + p.y 
            //
            // intersection:
            //  (a.dy/a.dx) * x = (b.dy/b.dx) * (p.x - x) + p.y //bug 2, x - p.x moet andersom, had ik eerst ook
            //  (a.dy/a.dx) * x = (b.dy/b.dx) * (x - p.x) + p.y
            //  (a.dy/a.dx) * x - (b.dy/b.dx) * (x - p.x) = p.y
            //  (a.dy/a.dx) * x - (b.dy/b.dx) * x + (b.dy/b.dx) * p.x = p.y
            //  (a.dy/a.dx) * x - (b.dy/b.dx) * x  = p.y + (b.dy/b.dx) * p.x //bug 3 + ipv -
            //  (a.dy/a.dx) * x - (b.dy/b.dx) * x  = p.y - (b.dy/b.dx) * p.x
            //  ((a.dy/a.dx) -  (b.dy/b.dx)) * x  = p.y - (b.dy/b.dx) * p.x
            //  x = (p.y - (b.dy/b.dx) * p.x) / ((a.dy/a.dx) -  (b.dy/b.dx))
            //  y = (a.dy/a.dx) * x
            //

            var p_x = (double)prize.x;
            var p_y = (double)prize.y;
            var a_dx = (double)buttonA.dx;
            var a_dy = (double)buttonA.dy;
            var b_dx = (double)buttonB.dx;
            var b_dy = (double)buttonB.dy;

            //var zz_a_x = p1_Acount * buttonA.dx;
            //var zz_a_y = p1_Acount * buttonA.dy;

            //var zz_b_x = p1_Bcount * buttonB.dx;
            //var zz_b_y = p1_Bcount * buttonB.dy;

            //var zz_px = zz_a_x + zz_b_x;
            //var zz_py = zz_a_y + zz_b_y;

            double x = (p_y - (b_dy / b_dx) * p_x) / ((a_dy / a_dx) - (b_dy / b_dx));
            double y = (a_dy / a_dx) * x;

            var i_X = (long)Math.Round(x);
            var i_Y = (long)Math.Round(y);

            //(2,1) A
            //(1,2) B
            //P @ (3,3)
            //intersect = (2,1)
            // #A = i_X / a.dx = 2 / 2 = 1
            // #B = (p.X - i_X) / b.dx = (3 - 2) / 1 = 1

            var countA = (i_X / buttonA.dx);
            var countB = (prize.x - i_X) / buttonB.dx;

            var result_x = countA * buttonA.dx + countB * buttonB.dx; //bug 5  countB * buttonA.dx
            var result_y = countA * buttonA.dy + countB * buttonB.dy; //       countB * buttonA.dy

            if (result_x == prize.x && result_y == prize.y)
            {
                tokensNeeded = countA * 3 + countB * 1;
            }

            return tokensNeeded;
        }

        internal void WriteDef()
        {
            Console.WriteLine($"Machine: {index}");

            foreach (var line in defLines)
            {
                Console.WriteLine(line);
            }
        }
    }

    class Button
    {
        public long dx;
        public long dy;
    }

    class Prize
    {
        public long x;
        public long y;
    }

    static class StringExt
    {
        public static long ExtractNumber(this string s)
        {
            //X+29,  -> 29
            //Y+2292 -> 12
            //x=839, -> 839
            //y=393  -> 393

            var len = s.Length;
            if (s[len - 1] == ',') { len--; }

            return long.Parse(s.Substring(2, len - 2));
        }
    }
}
