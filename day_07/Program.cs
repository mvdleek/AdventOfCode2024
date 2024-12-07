
namespace day_07
{
    // start @1030
    // p1 finished @ 1115, only real 1 mistake before running (only found in part 2, + long/int issue)
    // p2 finished @ 1126 - first time right
    // total time 55 minutes

    internal class Program
    {
        static void Main(string[] args)
        {
            //var inputs = ReadInput("input_example.txt"); //3749, p2=11387
            var inputs = ReadInput("input.txt"); // 1st try 5540634308465  2nd try 5540634308362 ;; p2=472290821152397

            long totalCalibrationResult = 0;

            foreach (var equation in inputs)
            {
                if (equation.CanBeMadeTrue())
                {
                    totalCalibrationResult += equation.TestValue;
                }

            }

            Console.WriteLine($"Total calibration result: {totalCalibrationResult}");
        }

        private static Equation[] ReadInput(string filename)
        {
            return File.ReadAllLines(filename).Select(line => new Equation(line)).ToArray() ;
        }
    }

    class Equation
    {
        bool part2 = true;
        public long TestValue;

        int[] Numbers;

        public Equation(string line)
        {
            var splitresult = line.Split(':');
            TestValue = long.Parse(splitresult[0]);
            Numbers = (splitresult[1].Trim().Split(' ')).Select(p => int.Parse(p)).ToArray();
        }

        public bool CanBeMadeTrue()
        {
            // solve by trying all combinations of operators
            // recursive with backtrack, can cutoff as soon as intermediate total > TestValue

            //return CanBeMadeTrueWith(0, 0); // first mistake 
            return CanBeMadeTrueWith(1, Numbers[0]); 
        }

        bool CanBeMadeTrueWith(int nextNumberIndex, long valueUpTillNow)
        {
            if (nextNumberIndex == Numbers.Length)
            {
                return valueUpTillNow == TestValue;
            }

            if (valueUpTillNow > TestValue)
            {
                return false;
            }

            return CanBeMadeTrueWith(nextNumberIndex+1, valueUpTillNow + Numbers[nextNumberIndex]) || 
                   CanBeMadeTrueWith(nextNumberIndex+1, valueUpTillNow * Numbers[nextNumberIndex]) ||
                   (part2 && CanBeMadeTrueWith(nextNumberIndex + 1, valueUpTillNow * (int)(Math.Pow(10, Numbers[nextNumberIndex].ToString().Length)) + Numbers[nextNumberIndex]))
                   ;
        }
    }
}
