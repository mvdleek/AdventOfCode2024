

using System.Diagnostics;
using System.Linq;
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
    // @2115 start again
    // input na 25x = 200446, 478 unieke op level 25 !!
    // @2330  ppffft 238317474993392 -- te lang geneuzeld met brute forcen voordat ik maar eens ging kijken hoeveel unieke waarden langskwamen, dat was zeer beperkt!
    internal class Program
    {
        class Stone
        {
            public long value;
            public long occurence;

            public Stone(long v, long o)
            {
                value = v;
                occurence = o;
            }
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0].Split(' ').Select(x => new Stone(long.Parse(x), 1)).ToArray();  // p1=200446, p2: 238317474993392
            var stoneList = new Stone[10_000_000];
            var stoneListCount = input.Length;
            for (int i = 0; i < input.Length; i++)
            {
                stoneList[i] = input[i];
            }

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < 75; i++)
            {
                Blink(stoneList, ref stoneListCount);

                Compact(stoneList, ref stoneListCount);

                Console.WriteLine($"stones after blink {i + 1} = {GetStoneCount(stoneList, stoneListCount)}");
            }
            sw.Stop();

            Console.WriteLine($"Duration = {sw.Elapsed}");
            Console.WriteLine($"Number of stones: {GetStoneCount(stoneList, stoneListCount)}");
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

        private static long GetStoneCount(Stone[] stoneList, int stoneListCount)
        {
            long count = 0;
            for (int j = 0; j < stoneListCount; j++)
            {
                count += stoneList[j].occurence;
            }
            return count;
        }

        private static void Compact(Stone[] stoneList, ref int stoneListCount)
        {
            var dict = new Dictionary<long, Stone>();
            for (int j = 0; j < stoneListCount; j++)
            {
                var stone = stoneList[j];
                if (!dict.ContainsKey(stone.value))
                {
                    dict.Add(stone.value, stone);
                }
                else
                {
                    dict[stone.value].occurence += stone.occurence;
                }
            }
            Console.WriteLine($"Unique values: {dict.Count}");

            for (int i = 0; i < stoneListCount; i++)
            {
                stoneList[i] = null!;
            }

            stoneListCount = 0;
            foreach (var item in dict)
            {
                stoneList[stoneListCount] = item.Value;
                stoneListCount++;
            }
        }

        private static void Blink(Stone[] stoneList, ref int stoneListCount)
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

        private static int GetNumberOfSplitsThatWillHappen(Stone[] stoneList, int stoneListCount)
        {
            var splitCount = 0;

            for (int stoneIndex = 0; stoneIndex < stoneListCount; stoneIndex++)
            {
                var stoneValue = stoneList[stoneIndex].value;

                if (stoneValue == 0) { continue; }

                var digitCount = (int)Math.Floor(Math.Log10(stoneValue) + 1);

                if (int.IsEvenInteger(digitCount))
                {
                    splitCount++;
                }
            }

            return splitCount;
        }

        private static int ApplyRule(Stone[] stoneList, int stoneIndex, int stoneWriteIndex)
        {
            var stone = stoneList[stoneIndex];

            //rule 1: If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
            if (stone.value == 0)
            {
                stoneList[stoneWriteIndex] = stone;
                stone.value = 1;
                return 1; // stoneswritten
            }

            //rule 2: If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
            var digitCount = (int)Math.Floor(Math.Log10(stone.value) + 1);

            if (int.IsEvenInteger(digitCount))
            {
                var modValue = (long)Math.Pow(10, digitCount / 2);
                var leftPart = stone.value / modValue;
                var rightPart = stone.value % modValue;


                stoneList[stoneWriteIndex - 1] = new Stone(leftPart, stone.occurence);
                stoneList[stoneWriteIndex] = new Stone(rightPart, stone.occurence);
                return 2;
            }

            //rule 3: If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.        }
            var newValue = stone.value * 2024;
            stone.value = newValue; //bug 1: wrote 1024 instead of 2024 :-(
            stoneList[stoneWriteIndex] = stone;

            return 1;
        }
    }
}
