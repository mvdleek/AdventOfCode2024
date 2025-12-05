

using System.Diagnostics;
using System.Linq;

namespace aoc25_day_01
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
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(x => new Turn(x)).ToArray();  // p1=200446, p2: 238317474993392

            var location = 50;
            var password = 0;
            foreach (var turn in input)
            {
                if (false)
                { // p1
                    if (turn.direction == 'L')
                    {
                        location = (location - turn.value) % 100;
                    }
                    else if (turn.direction == 'R')
                    {
                        location = (location + turn.value) % 100;
                    }
                    else
                    {
                        continue;
                    }

                    while (location < 0) location += 100;

                    while (location > 99) location -= 100;

                    if (location == 0)
                    {
                        password++;
                    }
                }
                else // p2
                {
                    var value = turn.value % 100;
                    password = password + turn.value / 100;
                    var locBefore = location;
                    if (value > 0)
                    {
                        if (turn.direction == 'L')
                        {
                            location = (location - value);
                            
                        }
                        else if (turn.direction == 'R')
                        {
                            location = (location + value);
                        }
                        else
                        {
                            continue;
                        }

                        while (location < 0) { location += 100; if (location != 0 && locBefore > 0) { password++; } }

                        while (location > 99) { location -= 100; if (location > 0) { password++; } }

                        if (location == 0)
                        {
                            password++;
                        }
                    } 
                    if (value == 0 && location == 0)
                    {
                        password++;
                    }


                    Console.WriteLine($"l={locBefore} T={turn.direction}{turn.value} -> {location}  pwd={password}");
                }
            }

            Console.WriteLine($"password={password}");
        }


    }
}
