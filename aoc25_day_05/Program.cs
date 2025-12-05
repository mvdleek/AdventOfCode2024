


namespace aoc25_day_05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new Database("input.txt");

            Solve(db);
            Solve2(db);
        }

        private static void Solve2(Database db)
        {
            db.FreshIdRanges.Sort((a, b) => CompareIdRanges(a, b));

            var i = 0;
            while (true)
            {
                if (i == db.FreshIdRanges.Count - 1)
                { break; }

                var current = db.FreshIdRanges[i];
                var next = db.FreshIdRanges[i + 1];

                // door ai gegenereerd: - had ik zelf ook zo willen doen

                if (current.Last + 1 >= next.First)
                {
                    // merge
                    current.Last = Math.Max(current.Last, next.Last);
                    db.FreshIdRanges.RemoveAt(i + 1);
                }
                else
                {
                    i++;
                }
            }

            long idCount = 0;
            // ook door ai gegenereerd: - had ik zelf ook zo willen doen
            foreach (var range in db.FreshIdRanges)
            {
                idCount += (range.Last - range.First + 1);
            }

            Console.WriteLine($"Number of frsh ingredients from ranges: {idCount}");
        }

        private static int CompareIdRanges(IdRange a, IdRange b)
        {
            if (a.First != b.First)
            {
                return a.First.CompareTo(b.First);
            }
            return a.Last.CompareTo(b.Last);
        }

        private static void Solve(Database db)
        {
            var freshCount = 0;
            foreach (var ingredientId in db.IngredientIds)
            {
                foreach (var range in db.FreshIdRanges)
                {
                    if (ingredientId >= range.First && ingredientId <= range.Last)
                    {
                        freshCount++;
                        break;
                    }
                }
            }
            Console.WriteLine($"Fresh ingredients = {freshCount}");
        }
    }

    class Database
    {
        public List<IdRange> FreshIdRanges = new List<IdRange>();
        public List<long> IngredientIds = new List<long>();

        public Database(string filename)
        {
            ReadInput(filename);
        }

        public void ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);

            var inIdRangeSection = true;

            // deze foreach is door ai gegenereerd! (ik tikte alleen foreach)
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    inIdRangeSection = false;
                    continue;
                }
                if (inIdRangeSection)
                {
                    var parts = line.Split('-');
                    var range = new IdRange
                    {
                        First = long.Parse(parts[0]),
                        Last = long.Parse(parts[1])
                    };
                    FreshIdRanges.Add(range);
                }
                else
                {
                    var id = long.Parse(line);
                    IngredientIds.Add(id);
                }
            }

        }
    }

    class IdRange
    {
        public long First;
        public long Last;
    }
}
