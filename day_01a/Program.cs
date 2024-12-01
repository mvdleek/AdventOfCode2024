namespace day_01a
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            //var input = File.ReadAllLines("input_example.txt");

            var list1 = new List<int>();
            var list2 = new List<int>();

            foreach (var item in input)
            {
                var parts = item.Split(' ');
                list1.Add(int.Parse(parts[0]));
                list2.Add(int.Parse(parts[parts.Length-1]));
            }

            list1.Sort();
            list2.Sort();

            var totalDistance = 0;

            for (int i = 0; i < list1.Count; i++)
            {
                var distance = Math.Abs(list1[i] - list2[i]);
                totalDistance += distance;
            }

            Console.WriteLine($"Total distance = { totalDistance }");
            // 2815556

            IEnumerable<IGrouping<int, int>> list2_grouped = list2.GroupBy(k => k, e => e);
            var list2_counts = new Dictionary<int, int>();
            foreach (var item in list2_grouped)
            {
                list2_counts.Add(item.Key, item.ToList().Count);
            }

            var similarityScore = 0;
            foreach (var item in list1)
            {
                if (list2_counts.ContainsKey(item))
                {
                    similarityScore += item * list2_counts[item];
                }
            }

            Console.WriteLine($"Similarity score = {similarityScore}");
            // 23927637
        }
    }
}
