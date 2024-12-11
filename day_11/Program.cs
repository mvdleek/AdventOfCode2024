

using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace day_11
{
    // Advent of code 2024: https://adventofcode.com/2024/day/11
    //
    // @0630 start 
    // @0720 p1 coding finished
    // @0725 p1 example ok  (only bug 1024 instead of 2024)
    // @0727 p1 input ok finsihed
    // @0740 p2 coding finished
    // @0748 p2 from list to array
    //
    internal class Program
    {
        static long maxValue = 0;


        static void Main(string[] args)
        {
            // diepte 50 = 4 minuten 6_900_581_362
            // diepte 55 = 

            var stoneList = File.ReadAllLines("input.txt")[0].Split(' ').Select(x => long.Parse(x)).ToArray();  // 200446
            //var stoneList = new List<long>(10_000_000); //optimization 1: allocate memory 1 time for the list, no expensive memory realloctions needed
            //var stoneList = new long[2_000_000_000];
            //var stoneListCount = input.Length;
            //for (int i = 0; i < input.Length; i++)
            //{
            //    stoneList[i] = input[i];
            //}

            var sw = new Stopwatch();
            sw.Start();
            long stoneCount = 0;

            Parallel.ForEach(
                     stoneList,                         // source collection
                     () => (long)0,                           // thread local initializer
                     (n, loopState, localSum) =>        // body
                     {
                             localSum += GetStoneCountFromValue(n, 55);

                         //localSum += n;
                         Console.WriteLine("Thread={0}, n={1}, localSum={2}", Thread.CurrentThread.ManagedThreadId, n, localSum);
                         return localSum;
                     },
                     (localSum) => Interlocked.Add(ref stoneCount, localSum)                   // thread local aggregator
                 );

            //for (int i = 0; i < stoneList.Length; i++)
            //{
            //    stoneCount += GetStoneCountFromValue(stoneList[i], 25);
            //}
            sw.Stop();

            Console.WriteLine($"Number of stones: {stoneCount}");
            Console.WriteLine($"Duration = {sw.Elapsed}");
        }


        private static long GetStoneCountFromValue(long stoneValue, int depth)
        {
            if (depth == 0) 
            { 
                
                return 1; 
            }

            //rule 1: If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
            if (stoneValue == 0)
            {
                return GetStoneCountFromValue(1, depth-1);
            }

            //rule 2: If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
            var digitCount = (int)Math.Floor(Math.Log10(stoneValue) + 1);

            if (int.IsEvenInteger(digitCount))
            {
                var modValue = (long)Math.Pow(10, digitCount / 2);
                var leftPart = stoneValue / modValue;
                var rightPart = stoneValue % modValue;

                return GetStoneCountFromValue(leftPart, depth - 1) +
                       GetStoneCountFromValue(rightPart, depth - 1)
                    ;
            }

            //rule 3: If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.        
            return GetStoneCountFromValue(stoneValue * 2024, depth - 1);
        }

        static void MainPart1(string[] args)
        {
            

            var input = File.ReadAllLines("input_example.txt")[0].Split(' ').Select(x => long.Parse(x)).ToArray();  // 200446
            //var stoneList = new List<long>(10_000_000); //optimization 1: allocate memory 1 time for the list, no expensive memory realloctions needed
            var stoneList = new long[2_000_000_000];
            var stoneListCount = input.Length;
            for (int i = 0; i < input.Length; i++)
            {
                stoneList[i] = input[i];
            }

            for (var i = 0; i < 25; i++)
            {
                Blink(stoneList, ref stoneListCount);
                Console.WriteLine($"stones after blink { i+1 } = { stoneListCount }");
                Console.WriteLine($"max after blink { i+1 } = { maxValue }");
            }

            Console.WriteLine($"Number of stones: { stoneListCount }");
        }

        private static void Blink(long[] stoneList, ref int stoneListCount)
        {
            // optimization ideas: 
            // 1) walk 1 time counting number of splits that will happen   
            // 2) then start at the end of the list walking back, writing at the end of the future list  : applied
            // 3) determine number of digits math Log 10 math operation : applied from start
            // 4) determine new values by mod and div operations by Pow 10^(num_digits/2) -> no toString, subString and int.Parse ops : applied from start

            var numberOfSplits = GetNumberOfSplitsThatWillHappen(stoneList, stoneListCount);
            var newStoneListCount = stoneListCount + numberOfSplits; //bug 2: forgot this in p2 ; bug 3 use newStoneList
            var stoneWriteIndex = newStoneListCount - 1;

            for (int stoneReadIndex = stoneListCount - 1; stoneReadIndex >= 0; stoneReadIndex--)
            {
                var stonesWritten = ApplyRule(stoneList, stoneReadIndex, stoneWriteIndex);
                stoneWriteIndex -= stonesWritten;
            }

            stoneListCount = newStoneListCount;
        }

        private static int GetNumberOfSplitsThatWillHappen(long[] stoneList, int stoneListCount)
        {
            var splitCount = 0;

            for (int stoneIndex = 0; stoneIndex < stoneListCount; stoneIndex++)
            {
                var stoneValue = stoneList[stoneIndex];

                if (stoneValue == 0) { continue; }

                var digitCount = (int)Math.Floor(Math.Log10(stoneValue) + 1);

                if (int.IsEvenInteger(digitCount))
                {
                    splitCount++;
                }
            }

            return splitCount;
        }

        private static int ApplyRule(long[] stoneList, int stoneIndex, int stoneWriteIndex)
        {
            var stoneValue = stoneList[stoneIndex];

            //rule 1: If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
            if (stoneValue == 0)
            {
                stoneList[stoneWriteIndex] = 1;
                return 1; // stoneswritten
            }

            //rule 2: If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
            var digitCount = (int)Math.Floor(Math.Log10(stoneValue) + 1);

            if (int.IsEvenInteger(digitCount))
            {
                var modValue = (long)Math.Pow(10, digitCount / 2);
                var leftPart = stoneValue / modValue;
                var rightPart = stoneValue % modValue;


                stoneList[stoneWriteIndex - 1] = leftPart;
                stoneList[stoneWriteIndex] = rightPart;
                return 2;
            }

            //rule 3: If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.        }
            var newValue = stoneValue * 2024;
            stoneList[stoneWriteIndex] = newValue; //bug 1: wrote 1024 instead of 2024 :-(

            if (newValue > maxValue)
            {
                maxValue = newValue;
            }
            return 1;
        }
    }
}
