
namespace aoc25_day_03
{
    internal class Program
    {

        class Bank
        {
            public string batteryBank;

            public Bank(string x)
            {
                batteryBank = x;
            }
        }
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(x => new Bank(x)).ToArray();  // p1=200446, p2: 238317474993392

            long totalJoltage = 0;
            foreach (var bank in input)
            {
                totalJoltage += getMaxJoltage( bank.batteryBank, 12);

            }
            Console.WriteLine($"max joltage={totalJoltage}");
        }

        private static long getMaxJoltage(string batteryBank)
        {
            var j1 = 0; var j2 = 0; // j1 <= j2
            var i1 = 0; 
            for (int i = 0; i < batteryBank.Length-1; i++)
            {
                var joltage =  batteryBank[i] - 48;

                if (joltage > j1) 
                { 
                    j1 = joltage;
                    i1 = i;
                }
            }
            // we now have largest leftmost digit, now find the biggist following
            for (int i = i1+1; i < batteryBank.Length; i++)
            {
                var joltage = batteryBank[i] - 48;

                if (joltage > j2)
                {
                    j2 = joltage;
                }
            }

            return j1 * 10 + j2;
        }

        private static long getMaxJoltage(string batteryBank, int len)
        {
            int[] j1_ar = new int[len];
            int[] i1_ar = new int[len];

            for (int k = 0; k < len; k++)
            {
                j1_ar[k] = 0; i1_ar[k] = 0;
            }

            long sum = 0;

            for (int digit = 0; digit < len; digit++)
            {
                for (int i = (digit == 0 ? 0 : i1_ar[digit-1] + 1); i < batteryBank.Length - (len - (1+digit)); i++)
                {
                    var joltage = batteryBank[i] - 48;

                    if (joltage > j1_ar[digit])
                    {
                        j1_ar[digit] = joltage;
                        i1_ar[digit] = i;
                    }
                }
                sum *= 10;
                sum += j1_ar[digit];
            }
            return sum;

        }
    }
}
