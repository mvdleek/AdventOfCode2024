
namespace TestProject1
{
    class Turn
    {
        public char direction;
        public int value;

        public Turn(string x)
        {
            direction = x[0];
            value = int.Parse(x.Substring(1));
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var turner = new Turner();
            turner.Turn("L50");
            Assert.Equal(1, turner.zeroHitCount);
        }

        [Fact]
        public void Test2()
        {
            var turner = new Turner();
            turner.Turn("L150");
            Assert.Equal(2, turner.zeroHitCount);
        }

        [Fact]
        public void Test3()
        {
            var turner = new Turner(0);
            turner.Turn("L100");
            Assert.Equal(1, turner.zeroHitCount);
        }

        [Fact]
        public void Day01_P1()
        {
            var input = File.ReadAllLines("input.txt").Select(x => new Turn(x)).ToArray();  // p1=200446, p2: 238317474993392

            var turner = new Turner();
            foreach (var turn in input)
            {
                turner.Turn($"{turn.direction}{turn.value}");
            }
            Console.WriteLine($"P1={turner.zeroHitCount}");
        }
    }

    class Turner
    {
        public int location = 50;
        public int zeroHitCount = 0;

        public Turner()
        {

        }

        public Turner(int start)
        {
            location = start;
        }

        internal void Turn(string v)
        {
            var direction = v[0];
            var degrees = int.Parse(v[1..]);

            if (direction == 'L')
            {
                for (global::System.Int32 i = 0; i < degrees; i++)
                {
                    ClickLeft();
                }
            }
            else
            {
                for (global::System.Int32 i = 0; i < degrees; i++)
                {
                    ClickRight();
                }
            }
        }

        void ClickLeft()
        {
            location--;
            if (location == 0)
            {
                zeroHitCount++;
            }
            if (location == -1)
            {
                location = 99;
            }
        }

        void ClickRight()
        {
            location++;
            if (location == 100)
            {
                location = 0;
                zeroHitCount++;
            }
        }
    }
}
