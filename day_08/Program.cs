using System.Diagnostics;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace day_08
{
    // start @ 1030
    // time finished coding proposed solution (without debugging) .. 1130 (tentatively)
    // p1 example finished @ 1156 (stupid j=0 instead of j=i+1 bug)
    // p1 finished @ 1157
    // p2 finshed  @ 1215
    // total time 1h45m
    internal class Program
    {
        static void Main(string[] args)
        {
            //var filename = "input_example.txt"; // 4 too many !!! --> 14
            //var filename = "input.txt"; // 305 
            //var filename = "input_example.txt"; // p2  34 (only b5 bug, easily found though because I had this mistake before, and this time I would be careful with this :-))
            var filename = "input.txt"; // p2 1150

            var map = new Map(filename);
            var count = map.FindNumberOfUniqueAntinodeLocations();

            Console.WriteLine($"Number Of Unique Antinode Locations = {count}");

            var map2 = new Map(filename);
            var count2 = map2.FindNumberOfUniqueAntinodeLocationsPart2(); //bug: 5 forget rename map to map2
            Console.WriteLine($"Number Of Unique Antinode Locations P2 = {count2}");
        }
    }

    class Map
    {
        int width;
        int height;
        List<char[]> transmitterMap = new();
        Dictionary<char, List<(int x, int y)>> transmittersByFrequency = new();

        List<((int x, int y) t1, (int x, int y) t2)> transmitterPairs = new();

        List<char[]> antinodeMap = new();
        int uniqueAntinodeLocationsCount = 0;

        public Map(string filename)
        {
            var lines = File.ReadAllLines(filename);
            width = lines[0].Length;
            height = lines.Length;

            foreach (var line in lines)
            {
                var row = line.ToArray();
                transmitterMap.Add(row);

                var antinodeRow = new char[width];
                Array.Fill(antinodeRow, '.');
                antinodeMap.Add(antinodeRow);
            }
        }

        public int FindNumberOfUniqueAntinodeLocations()
        {
            FindTransmitters();
            FindTransmitterPairs();
            FindAntinodes();
            //PrintAntinodeMap();

            return uniqueAntinodeLocationsCount;
        }

        public int FindNumberOfUniqueAntinodeLocationsPart2()
        {
            FindTransmitters();
            FindTransmitterPairs();
            FindAntinodesPart2();

            return uniqueAntinodeLocationsCount;
        }

        private void FindAntinodesPart2()
        {
            /* an antinode occurs at any grid position exactly in line with at least two antennas of the same frequency, regardless of distance. 
             * This means that some of the new antinodes will occur at the position of each antenna(unless that antenna is the only one of its frequency). 
             */

            foreach (var tp in transmitterPairs)
            {
                var dx = tp.t2.x - tp.t1.x;
                var dy = tp.t2.y - tp.t1.y;

                // we can just start marking antinodes from 1 transmitter in 2 directions

                // direction 1
                var x = tp.t1.x;
                var y = tp.t1.y;

                while (IsOnGrid(x, y))
                {
                    PutAntinodeMark(x, y);
                    x -= dx;
                    y -= dy;
                }

                // direction 2
                x = tp.t1.x;
                y = tp.t1.y;

                while (IsOnGrid(x, y))
                {
                    PutAntinodeMark(x, y);
                    x += dx;
                    y += dy;
                }
            }
        }

        private bool IsOnGrid(int x, int y)
        {
            return (x >= 0 && x < width && y >= 0 && y < width);
        }

        private void PrintAntinodeMap()
        {
            foreach (var item in antinodeMap)
            {
                Console.WriteLine(new string(item));
            }
        }

        private void FindAntinodes()
        {
            /* In particular, an antinode occurs at any point that is perfectly in line with two antennas of the same frequency 
             * -but only when one of the antennas is twice as far away as the other.This means that for any pair of antennas with 
             * the same frequency, there are two antinodes, one on either side of them */

            // hm it says two times, but should'nt it be possible 4 times??
            // ..X.......A..X..X..A.......X..
            //

            foreach (var tp in transmitterPairs)
            {
                var dx = tp.t2.x - tp.t1.x;
                var dy = tp.t2.y - tp.t1.y;

                var an1x = tp.t1.x - dx;
                var an1y = tp.t1.y - dy;

                var an2x = tp.t2.x + dx;
                var an2y = tp.t2.y + dy;

                PutAntinodeMark(an1x, an1y);
                PutAntinodeMark(an2x, an2y);
            }
        }

        private void PutAntinodeMark(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < width) //bug 2: forgot check if on map
            {
                if (antinodeMap[y][x] != '#') // b3 : used 'X'
                {
                    antinodeMap[y][x] = '#';
                    uniqueAntinodeLocationsCount++;
                }
            }
        }

        private void FindTransmitterPairs()
        {
            foreach (var freq in transmittersByFrequency.Keys)
            {
                var transmitters = transmittersByFrequency[freq];

                // determine all unique transmitter pair combinations 
                // (t1,t2) is same is (t2,t1) do not add twice!

                for (var i = 0; i < transmitters.Count - 1; i++)
                {
                    var t1 = transmitters[i];
                    for (var j = i+1; j < transmitters.Count; j++) //bug 4: j=0 had to be j=i+1, this was the big debug time sink (30m)
                    {
                        var t2 = transmitters[j];
                        transmitterPairs.Add((t1, t2));
                    }
                }
            }
        }

        private void FindTransmitters()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var transmitter = GetTransmitterAt(x, y);
                    if (transmitter != '.')
                    {
                        if (!transmittersByFrequency.ContainsKey(transmitter)) //bug 1: forgot !
                        {
                            transmittersByFrequency.Add(transmitter, []);
                        }

                        transmittersByFrequency[transmitter].Add((x, y));
                    }
                }
            }
        }

        private char GetTransmitterAt(int x, int y)
        {
            return transmitterMap[y][x];
        }
    }
}
