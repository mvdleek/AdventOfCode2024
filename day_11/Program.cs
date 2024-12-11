

namespace day_11
{
    // @0630 start 
    // @0720 p1 coding finished
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input_example.txt")[0].Split(' ').Select(x => long.Parse(x)).ToArray();
            var stoneList = new List<long>(1_000_000); //optimization 1: allocate memory 1 time for the list, no expensive memory realloctions needed
            stoneList.AddRange(input);

            for (var i = 0; i < 25; i++)
            {
                Blink(stoneList);
                Console.WriteLine($"Length after blink { i+1 } = { stoneList.Count }");
            }

            Console.WriteLine($"Part 1: Number of stones: { stoneList.Count }");
        }

        private static void Blink(List<long> stoneList)
        {
            // optimization ideas: 
            // 1) walk 1 time counting number of splits that will happen
            // 2) then start at the end of the list walking back
            // 3) determine number of digits math Log 10 math operation
            // 4) determine new values by mod and div operations by Pow 10^(num_digits/2) -> no toString, subString and int.Parse ops

            for (int i = stoneList.Count - 1; i >= 0; i--)
            {
                ApplyRule(stoneList, i);
            }
        }

        private static int ApplyRule(List<long> stoneList, int stoneIndex)
        {
            var stoneValue = stoneList[stoneIndex];

            //rule 1: If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
            if (stoneValue == 0)
            {
                stoneList[stoneIndex] = 1;
                return 1;
            }

            //rule 2: If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
            var digitCount = GetNumberOfDigitsIn(stoneValue);

            if (int.IsEvenInteger(digitCount))
            {
                var modValue = (long)Math.Pow(10, digitCount / 2);
                var leftPart = stoneValue / modValue;
                var rightPart = stoneValue % modValue;

                stoneList[stoneIndex] = leftPart;
                stoneList.Insert(stoneIndex + 1, rightPart);
                return 2;
            }

            //rule 3: If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.        }
            stoneList[stoneIndex] = stoneValue * 1024;
            return 1;
        }

        private static int GetNumberOfDigitsIn(long stoneValue)
        {
            //
            //return stoneValue.ToString().Length;

            //optimization 2: do not use .ToString()
            //1100 --> 4  if < 1000 = 3
            // 150 --> 3  if <  100 = 2
            //  12 --> 2  if <   10 = 1

            long compareValue = 10;
            int digitcount = 1;
            while (true) 
            {  
                if (stoneValue < compareValue)
                {
                    return digitcount;
                }
                compareValue *= 10;
                digitcount++;
            }
        }
    }
}
