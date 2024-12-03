namespace day_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
            // 161
            //              mul(123,123)
            //              mul(

            // NOT: mul(252,213#+[*)
            //NOT: mul(139,875 &@<)
            //NOT: mul(369,854''wh)
            //NOT: mul(847,620')+f)
            //NOT: mul(929,569:%>,)

            //NOT: mul(755,460{who)
            //NOT: mul(937,923,how)
            //NOT: mul(130,679why()
            //NOT: mul(714,836((se)
            //NOT: mul(80,911how(2)
            //NOT: mul(22,82>-!- /)
            //NOT: mul(647,320[(;m)
            // NOT: mul(620,80<( 3?)
            //NOT: mul(574,50>~wha)
            //NOT: mul(510,88]how()
            //NOT: mul(132,913!%fr)
            //NOT: mul(884,988when)
            //NOT: mul(88,163^%sel)
            //NOT: mul(600,55s3)fr) <----- THISSSSSS
            // 790 x mul(
            // 39 x do()
            // 31 x don't()
            var sentinel = "$$$$$$$$$$$$$";
            //var input = File.ReadAllText("input_example_p2.txt")! + sentinel;
            //var input = File.ReadAllText("input2.txt")! + sentinel;
            var input = File.ReadAllText("c:\\temp\\input_dl.txt")! + sentinel;
            var input_len = input.Length - sentinel.Length;
            long total = 0;
            var startIndex = 0;
            var doing = true;
            var enablePart2 = true;

            while (startIndex < input_len)
            {
                var i = input.IndexOf("mul(", startIndex);
                if (i == -1) { break; }

                if (enablePart2)
                {
                    var i_do = input.LastIndexOf("do()", i, 1 + (i - startIndex));
                    var i_dont = input.LastIndexOf("don't()", i, 1 + (i - startIndex));

                    if (i_do != -1 || i_dont != -1)
                    {
                        if (i_do > i_dont)
                        {
                            doing = true;
                        }
                        else
                        {
                            doing = false;
                        }
                    }
                }

                var start = i;
                i += 4;
                var x = ReadNumber(input, ref i);
                if (x != null)
                {
                    if (input[i] == ',')
                    {
                        i++;
                        var y = ReadNumber(input, ref i);
                        if (y != null)
                        {
                            if (input[i] == ')')
                            {
                                // uncorrupted instruction found
                                i++;
                                if (doing)
                                {
                                    total += x.Value * y.Value;
                                }
                            }
                        }
                    }
                }

                startIndex = i;
            }

            Console.WriteLine($"Total: {total}");
            // 192435729 via copy & past naar input.txt
            // 192767529 via edge, save as naar input_dl.txt
            // 192767529 via edge, copy paste input2.txt
            // 192767529 via copy & past naar input.txt   --> accidental s inserted because of mac keyboard i pressed fn+s thinking it was ctrl+s

            // part 2:
            // 101636318 -- zucht foute input file gebruikt :-(
            // 101636318 -- grrrrrrr
            // 104083373 -- lastindex count toch 1 + (i - startindex)
        }

        private static int? ReadNumber(string input, ref int i)
        {
            int? number = null;
            if (char.IsAsciiDigit(input[i]))
            {
                number = input[i] - '0'; 
                i++;

                if (char.IsAsciiDigit(input[i]))
                {
                    number = number * 10;
                    number += input[i] - '0';
                    i++;

                    if (char.IsAsciiDigit(input[i]))
                    {
                        number = number * 10;
                        number += input[i] - '0';
                        i++;
                    }

                }

            }

            return number;
        }
    }
}
