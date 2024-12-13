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

    internal class Program
    {
        static void Main(string[] args)
        {
            var filename = "input.txt";
            
            var allMachines = ClawMachine.ReadInput(filename);

            long totalTokens = 0;

            foreach (var machine in allMachines)
            {
                long minTokensForMachine = long.MaxValue;

                for (int a = 0; a < 100; a++)
                {
                    for (int b = 0; b < 100; b++)
                    {
                        if (a*machine.buttonA.dx + b*machine.buttonB.dx == machine.prize.x && 
                            a*machine.buttonA.dy + b*machine.buttonB.dy == machine.prize.y)
                        {
                            var tokensNeeded = a * 3 + b * 1;
                            if (tokensNeeded < minTokensForMachine)
                            {
                                minTokensForMachine = tokensNeeded;
                            }
                        }
                    }
                }

                if (minTokensForMachine != long.MaxValue)
                {
                    totalTokens += minTokensForMachine;
                }
            }

            Console.WriteLine($"Fewest tokens to win all possible prizes: {totalTokens}");
        }
    }

    class ClawMachine
    {
        public Button buttonA;
        public Button buttonB;

        public Prize prize;
        
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
                    var prize = new Prize();
                    prize.x = parts[1].ExtractNumber();
                    prize.y = parts[2].ExtractNumber();

                    machine.prize = prize; //bug 1 forgot this
                    clawMachines.Add(machine);

                    machine = new ClawMachine();
                }
            }

            return clawMachines.ToArray();
        }
    }

    class Button
    {
        public int dx;
        public int dy;
    }

    class Prize
    {
        public long x;
        public long y;
    }

    static class StringExt
    {
        public static int ExtractNumber(this string s)
        {
            //X+29,  -> 29
            //Y+2292 -> 12
            //x=839, -> 839
            //y=393  -> 393

            var len = s.Length;
            if (s[len - 1] == ',') { len--; }

            return int.Parse(s.Substring(2, len - 2));
        }
    }
}
