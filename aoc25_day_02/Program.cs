
namespace aoc25_day_02
{
    internal class Program
    {

        class IDRange
        {
            public long first;
            public long last;

            public IDRange(string x)
            {
                var parts = x.Split('-');
                first = long.Parse(parts[0]);
                last = long.Parse(parts[1]);
            }
        }
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0].Split(',').Select(x => new IDRange(x)).ToArray();  // p1=200446, p2: 238317474993392

            long sum = 0;
            foreach (var range in input)
            {
                //Console.WriteLine($"{range.first}-{range.last}");
                for (long i = range.first; i <= range.last; i++)
                {
                    if (isInvalidId_2(i))
                    {
                        //Console.WriteLine($"  {i}");
                        sum += i;
                    }
                }
            }

            Console.WriteLine($"sum={sum}");
        }

        private static bool isInvalidId(long i)
        {
            var s = i.ToString();
            if (s.Length % 2 != 0)
            {
                return false;
            }

            var half = s.Length / 2;
            for (int pos = 0; pos < half; pos++)
            {
                if (s[pos] != s[half+pos])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool isInvalidId_2(long i)
        {
            var s = i.ToString();
            var half = s.Length / 2;

            for (int len = 1; len <= half; len++)
            {
                if (hasRepeatsOfLength(s, len))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool hasRepeatsOfLength(string s, int len)
        {
            if (len > 1 && s.Length % len != 0)
            {
                //Console.WriteLine($"slen={s.Length} len={len}  repeats not possible");
                return false;
            }

            var partCount = s.Length / len;

            for (int part = 1; part < partCount; part++)
            {
                for (int pos = 0; pos < len; pos++)
                {
                    if (s[pos] != s[part * len + pos])
                    {
                        return false;
                    }
                }   
            }
            //Console.WriteLine($"s={s} len={len} , hasRepeats");
            return true;
        }
    }
}
