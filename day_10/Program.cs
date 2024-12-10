namespace day_10
{
    // https://adventofcode.com/2024/day/10
    // 07:28
    // 07:42 start coding
    // 08:11 finisch coding p1
    // p1 example correct at 8:14
    // p1 finished at 8:15 no more bugs
    // break for work & shower
    // p2 start 8:45 (read it before -> thought about solution in the shower, it was very simple)
    // p2 finished at 8:48

    internal class Program
    {
        static bool part2 = true;

        static void Main(string[] args)
        {

            var input = File.ReadAllLines("input.txt").Select(x => x.Select(c => c - '0').ToArray()).ToArray();

            var trailheads = FindTrailHeadsTotalScore(input);
            Console.WriteLine($"Trailheads {trailheads}");
        }

        private static int FindTrailHeadsTotalScore(int[][] input)
        {
            var totalScore = 0;
            for (int x = 0; x < input[0].Length; x++)
            {
                for (int y = 0; y < input.Length; y++)
                {
                    if (input[y][x] == 0) //bug 1 '0' instead of 0
                    {
                        var trailScore = GetNumberOf9pointsInTrailStartingAt(input, x, y);
                        totalScore += trailScore;
                    }
                }
            }
            return totalScore;
        }

        private static int GetNumberOf9pointsInTrailStartingAt(int[][] input, int startx, int starty)
        {
            var numberOf9pointsReachable = GetNumberOf9pointsInTrailStartingAtWithPrevHeight(input, startx, starty, -1);

            if (!part2)
            {
                // reset marked 9 points
                for (int x = 0; x < input[0].Length; x++)
                {
                    for (int y = 0; y < input.Length; y++)
                    {
                        if (input[y][x] == -1)
                        {
                            input[y][x] = 9;
                        }
                    }
                }
            }
            return numberOf9pointsReachable;
        }

        private static int GetNumberOf9pointsInTrailStartingAtWithPrevHeight(int[][] input, int x, int y, int prevHeight)
        {
            if (x < 0 || y < 0 || x >= input[0].Length || y >= input.Length) { return 0; }

            var height = input[y][x];
            if (height != prevHeight + 1) { return 0; };
            if (height == 9) // bug 2: '9' instead of 9
            {
                if (!part2) { input[y][x] = -1; }// mark it to count it only one time 
                return 1; 
            }


            var numberOf9points = 0;

            numberOf9points += GetNumberOf9pointsInTrailStartingAtWithPrevHeight(input, x+1, y, height);
            numberOf9points += GetNumberOf9pointsInTrailStartingAtWithPrevHeight(input, x-1, y, height);
            numberOf9points += GetNumberOf9pointsInTrailStartingAtWithPrevHeight(input, x, y+1, height);
            numberOf9points += GetNumberOf9pointsInTrailStartingAtWithPrevHeight(input, x, y-1, height);


            return numberOf9points;
        }
    }
}
