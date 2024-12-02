
namespace day_02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            //var input = File.ReadAllLines("input_example.txt");

            var safeCount = 0;
            foreach (var item in input)
            {
                if (IsReportSafe(item))
                {
                    safeCount++;
                }
            }

            Console.WriteLine($"Safe reports: {safeCount}");
            // 213

            var safeWithDampeningCount = 0;
            foreach (var item in input)
            {
                if (IsReportSafeWithDampeningBrutal(item))
                {
                    safeWithDampeningCount++;
                }
            }

            Console.WriteLine($"Safe reports with dampening: {safeWithDampeningCount}");
            // 265 -- too low -- the non-brutal way
            // 283 -- too low the non-brutal way
            // 285 -- the brutal way

        }

        private static bool IsReportSafe(string item)
        {
            var parts = item.Split(' ');

            return FindIndexOfReportUnSafeLevel(parts) == -1;
        }

        private static int FindIndexOfReportUnSafeLevel(string[] parts)
        {
            if (parts.Length < 1)
            {
                throw new Exception("Unexpected report with {parts.Length} levels found!");
            }

            var increasing = false;

            for (var i = 0; i < parts.Length - 1; i++)
            {
                var diff = int.Parse(parts[i + 1]) - int.Parse(parts[i]);
                if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                {
                    return i+1;
                }

                if (i == 0)
                {
                    increasing = diff > 0;
                }
                else if (increasing && diff < 0)
                {
                    return i+1;
                }
                else if (!increasing && diff > 0)
                {
                    return i+1;
                }
            }


            return -1;
        }


        private static bool IsReportSafeWithDampening(string item)
        {
            var parts = item.Split(' ');

            var unsafeIndex = FindIndexOfReportUnSafeLevel(parts);

            if (unsafeIndex == -1)
            {
                return true;
            }

            // to make the report safe, we can remove the item on index i OR on i-1 !! (if i>0)
            // (forgot this the 1st time)s
            //if (unsafeIndex > 0)
            //{
            //    if (IsReportSafeWithDampeningWithLevelRemoved(parts, unsafeIndex-1))
            //    {
            //        return true;
            //    }
            //}

            return IsReportSafeWithDampeningWithLevelRemoved(parts, unsafeIndex);
        }

        private static bool IsReportSafeWithDampeningWithLevelRemoved(string[] parts, int levelToRemove)
        {
            var dampendedParts = parts.ToList();
            dampendedParts.RemoveAt(levelToRemove);

            return (FindIndexOfReportUnSafeLevel(dampendedParts.ToArray()) == -1);
        }

        private static bool IsReportSafeWithDampeningBrutal(string item)
        {
            if (IsReportSafe(item))
            {
                return true;

            }


            var parts = item.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                if (IsReportSafeWithDampeningWithLevelRemoved(parts, i))
                {
                    return true;
                }

            }
           

             

            return false;
        }

    }
}
