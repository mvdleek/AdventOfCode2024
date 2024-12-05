
namespace day_05
{
    // 6:30 - 7:50
    internal class Program
    {
        static void Main(string[] args)
        {
            //var (ruleBook, updates) = ReadInput("input_example.txt"); // 143  1st time right   part2: 123 1st time right
            var (ruleBook, updates) = ReadInput("input.txt"); // 4281 1st time right :-) @7:15 part2 5466 @07:50  1st time right

            var sumOfValidMiddlePages = 0;
            var sumOfCorrectedMiddlePages = 0;
            foreach (var update in updates)
            {
                if (update.IsValid(ruleBook))
                {
                    sumOfValidMiddlePages += update.MiddlePage;
                }
                else
                {
                    update.MakeValid(ruleBook);

                    sumOfCorrectedMiddlePages += update.MiddlePage;
                }
            }

            Console.WriteLine($"sumOfValidMiddlePages={sumOfValidMiddlePages}");
            Console.WriteLine($"sumOfCorrectedMiddlePages={sumOfCorrectedMiddlePages}");
        }

        private static (RuleBook rules, List<Update> updates) ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);
            List<Rule> rules = new();
            List<Update> updates = new();

            var inRules = true;
            for (var i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    inRules = false;
                }
                else if (inRules)
                {
                    rules.Add(new Rule(lines[i]));
                }
                else
                {
                    updates.Add(new Update(lines[i]));
                }
            }

            return (new RuleBook(rules), updates);
        }
    }

    class Rule
    {
        public int pageThatMustBeEarlier;
        public int pageThatMustBeLater;

        public Rule(string v)
        {
            var parts = v.Split('|');
            pageThatMustBeEarlier = int.Parse(parts[0]);
            pageThatMustBeLater = int.Parse(parts[1]);
        }
    }

    class RuleBook
    {
        public List<Rule> rules;

        public RuleBook(List<Rule> rules)
        {
            this.rules = rules;
        }

        internal bool IsValidPageOrder(int earlierPage, int laterPage)
        {
            return !rules.Any(r => r.pageThatMustBeEarlier == laterPage && r.pageThatMustBeLater == earlierPage);
        }
    }

    class Update
    {
        public int[] pages;

        public Update(string v)
        {
            var parts = v.Split(',');
            pages = parts.Select(x => int.Parse(x)).ToArray();
        }

        public int MiddlePage
        {
            get
            {
                return pages[pages.Length/2];
            }
        }

        public bool IsValid(RuleBook ruleBook)
        {
            return IsValid(ruleBook, out _);
        }

        public bool IsValid(RuleBook ruleBook, out (int index1, int index2) invalidPageIndexes)
        {
            for (var i = 1; i < pages.Length; i++)
            {
                var p2 = pages[i];
                for (var j = 0; j < i; j++)
                {
                    var p1 = pages[j];

                    if (!ruleBook.IsValidPageOrder(p1, p2))
                    {
                        invalidPageIndexes = (p1, p2);
                        return false;
                    }
                }

            }

            invalidPageIndexes = (-1, -1);
            return true;
        }


        public void MakeValid(RuleBook ruleBook)
        {
            for (var i = 0; i < pages.Length-1; i++)
            {
                for (var j = i+1; j < pages.Length; j++)
                {
                    if (!ruleBook.IsValidPageOrder(pages[i], pages[j]))
                    {
                        SwapPagesAtIndexes(i, j);
                        i = -1;
                        break;
                    }
                }
            }
        }

        private void SwapPagesAtIndexes(int i1, int i2)
        {
            var t = pages[i1];
            pages[i1] = pages[i2];
            pages[i2] = t;
        }
    }
}
