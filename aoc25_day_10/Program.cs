namespace aoc25_day_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = ReadInput("input.txt");

            //SolveP1(input); //411
            SolveP2(input); //
            //[..#.##] (1,2) (0,2,3) (0,2,4) (0,5) (2,3) {22,13,33,18,2,16}
            // a*(0, 1, 1, 0, 0, 0) + b*(1, 0, 1, 1, 0, 0) + c*(1, 0, 1, 0, 1, 0) + d*(1, 0, 0, 0, 0, 1) + e*(0, 0, 1, 1, 0, 0) = {22,13,33,18,2,16}
            //          a     b        c       d     e
            //   22=b+c+d, 13=a, 33=a+b+c+e, 18=b+e, 1=c, 16=d
            //

            //[.####..#] (0,1,7) (0,2,4,5,6,7) (2,3) (1,2,6) (1,2,5,7) (3,4,6,7) (2,7) (2,3,4,6,7) {10,187,228,38,28,192,33,218}
            //            10      10            38    33      187       28       218    28
            //            a       b             c     d       e         f        g      h
            //    10=a+b,187=a+d+e, 228=b+c+d+e+g+h, 38=c+f+h, 28=b+f+h, 192=b+e, 33=b+d+f+h, 218=a+b+e+f+g+h
            //
            //   b -> a,e,d     10 
            //    f -> h,g,c    28

        }

        private static void SolveP1(List<Line> input)
        {
            var totalPresses = 0;
            foreach (var item in input)
            {
                item.SolveP1();
                totalPresses += item.buttonPressesNeededToSolve;
            }
            Console.WriteLine($"Minimal presses needed P1 = {totalPresses}");
        }
        private static void SolveP2(List<Line> input)
        {
            var totalPresses = 0;
            var i = 0;
            foreach (var item in input)
            {
                Console.WriteLine($"i={i}");
                item.SolveP2();
                totalPresses += item.buttonPressesNeededToSolve;
                i++;
            }
            Console.WriteLine($"Minimal presses needed P2 = {totalPresses}");
        }

        private static List<Line> ReadInput(string v)
        {
            var lines = File.ReadAllLines(v);

            var result = new List<Line>();
            foreach (var line in lines)
            {
                // parse line into Line object
                var lineObj = new Line(line);    //<---- AI PUT V instead of line here LOL
                result.Add(lineObj);
            }
            return result;
        }
    }

    internal class Line
    {
        public State target; 
        public List<Button> buttons { get; set; } = new List<Button>();

        public int buttonPressesNeededToSolve = 0;

        public Line(string v)
        {
            var parts = v.Split(" ");

            target = new State(parts[0], parts[parts.Length - 1]);

            for (int i = 1; i < parts.Length-1; i++)
            {
                buttons.Add(new Button(parts[i]));
            }
        }

        public void SolveP1()
        {
            // perform a breadth-first search to find the minimum button presses needed to light all lights

            var states = new Queue<State>();

            var initialState = new State(target.lightCount);
            if (initialState.IsGoalStateP1(target))
            {
                return;
            }

            states.Enqueue(initialState);

            while (true)
            {
                var state = states.Dequeue();

                foreach (var button in buttons)
                {
                    var newState = state.ApplyButtonPress(button);
                    if (newState.IsGoalStateP1(target))
                    {
                        buttonPressesNeededToSolve = newState.buttonPresses;
                        states.Clear();
                        return;
                    }
                    states.Enqueue(newState);
                }
            }
        }

        public void SolveP2()
        {
            // perform a breadth-first search to find the minimum button presses needed to reach target joltages

            var states = new Queue<State>();

            var initialState = new State(target.lightCount);
            if (initialState.IsGoalStateP2(target) == 1)
            {
                return;
            }

            states.Enqueue(initialState);

            while (true)
            {
                var state = states.Dequeue();

                foreach (var button in buttons)
                {
                    var newState = state.ApplyButtonPress(button);
                    var r = newState.IsGoalStateP2(target);
                    if (r == 1)
                    {
                        buttonPressesNeededToSolve = newState.buttonPresses;
                        states.Clear();
                        return;
                    }
                    if (r == -1) { 
                        continue;
                    }
                    states.Enqueue(newState);
                }
            }
        }
    }

    internal class State
    {
        public int lightCount = 0;
        public bool[] currentLights;
        public int[] currentJoltages;
        public int buttonPresses = 0;

        public State(string lights, string joltages)
        {
            lightCount = lights.Length-2;
            currentLights = new bool[lightCount];
            for (int i = 1; i < lights.Length - 1; i++)
            {
                currentLights[i - 1] = lights[i] == '#';
            }
            currentJoltages = new int[lightCount];
            var joltageParts = joltages.Substring(1, joltages.Length - 2).Split(","); //AI
            for (int i = 0; i < lightCount; i++)
            {
                currentJoltages[i] = int.Parse(joltageParts[i]);
            }
        }
        public State(int lightCount)
        {
            this.lightCount = lightCount;
            currentLights = new bool[lightCount];
            currentJoltages = new int[lightCount];
        }

        public State ApplyButtonPress(Button button)
        {
            var newState = new State(lightCount);
            newState.currentLights = (bool[])currentLights.Clone();
            newState.currentJoltages = (int[])currentJoltages.Clone();
            newState.buttonPresses = buttonPresses + 1;

            foreach (var toggle in button.toggles)
            {
                newState.currentLights[toggle] = !newState.currentLights[toggle];
                newState.currentJoltages[toggle] += 1;
            }   

            return newState;
        }

        internal bool IsGoalStateP1(State targetLights)
        {
            for (int i = 0; i < lightCount; i++)
            {
                if (currentLights[i] != targetLights.currentLights[i])
                    return false;
            }
            return true;
        }
        internal int IsGoalStateP2(State targetLights)
        {
            var allEqual = true;
            var anyAbove = false;
            for (int i = 0; i < lightCount; i++)
            {
                if (currentJoltages[i] > targetLights.currentJoltages[i])//\\
                {
                    anyAbove = true;
                    break;
                }
                if (currentJoltages[i] != targetLights.currentJoltages[i])//\\
                {
                    allEqual = false;
                    break;
                }
            }
            if (anyAbove)
            {
                return -1;
            }
            if (allEqual) { 
                return 1;
            }
            //if (anyAbove)
            //{
            //    return -1;
            //}
            for (int i = 0; i < lightCount; i++)
            {
                if (currentJoltages[i] > targetLights.currentJoltages[i])//\\
                {
                    anyAbove = true;
                    break;
                }
            }

            if (anyAbove)
            {
                return -1;
            }

            return 0;
        }
    }

    public class Button
    {
        public List<int> toggles { get; set; } = new List<int>();

        public Button(string v)
        {
            var parts = v.Substring(1, v.Length - 2).Split(",");

            foreach (var part in parts)
            {
                toggles.Add(int.Parse(part));
            }
        }
    }
}
