

namespace aoc25_day_09
{
    internal class Bitmap
    {
        const int WIDTH = 100_000;
        const int HEIGHT = 100_000;
        byte[] bits = new byte[((long)WIDTH) * (HEIGHT / 8)]; // 1MB bitmap = 8M bits
        List<Line> lines = new List<Line>();

        public Bitmap(List<Tile> input)
        {
            DrawLines(input);


            //CheckIntersectingLines();
        }

        //public void SaveAsPngFile(string filename)
        //{
        //    using (var bmp = new System.Drawing.Bitmap(WIDTH, HEIGHT))
        //    {
        //        for (int y = 0; y < HEIGHT; y++)
        //        {
        //            for (int x = 0; x < WIDTH; x++)
        //            {
        //                var color = IsFilled(x, y) ? System.Drawing.Color.Black : System.Drawing.Color.White;
        //                bmp.SetPixel(x, y, color);
        //            }
        //        }
        //        bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        //    }
        //}

        public void FloodFill(int startX, int startY)
        {
            Console.WriteLine("Start floodfill");
            // ik typte alleen var en ai zei de rest LOL
            var queue = new Queue<Tile>();
            queue.Enqueue(new Tile(startX, startY));
            long setCount = 0;
            int prevQueueCount = 0;
            while (queue.Count > 0)
            {
                if (Math.Abs(queue.Count - prevQueueCount) > 1000)
                { 
                Console.WriteLine($"Quue.Count = {queue.Count}  SetCount = {setCount}");
                    prevQueueCount = queue.Count;
                }
                var tile = queue.Dequeue();
                // check if in bounds
                if (tile.x < 0 || tile.x >= WIDTH || tile.y < 0 || tile.y >= HEIGHT)
                    continue;
                // check if already set
                var byteIndex = tile.y * (WIDTH / 8) + tile.x / 8;
                var bitIndex = tile.x % 8;
                var b = (byte)(1 << bitIndex);
                if ((bits[byteIndex] & b) != 0)
                    continue; // already set
                // set bit
                bits[byteIndex] |= b;
                setCount++;
                // enqueue neighbors
                queue.Enqueue(new Tile(tile.x + 1, tile.y));
                queue.Enqueue(new Tile(tile.x - 1, tile.y));
                queue.Enqueue(new Tile(tile.x, tile.y + 1));
                queue.Enqueue(new Tile(tile.x, tile.y - 1));
            }

            Console.WriteLine("End floodfill");
        }

       

        internal bool IsCompletlyFilled(Tile t1, Tile t2)
        {
            // niks getiks, AI gaf deze oplossing, die ik ook wilde maken, alleen wilde ik de inner rectangle doen
            var minX = Math.Min(t1.x, t2.x);
            var maxX = Math.Max(t1.x, t2.x);
            var minY = Math.Min(t1.y, t2.y);
            var maxY = Math.Max(t1.y, t2.y);
            for (int y = minY+1; y <= maxY-1; y++)
            {
                for (int x = minX+1; x <= maxX-1; x++)
                {
                    if (IsFilled(x, y)) //hacky
                        return false;
                }
            }
            return true;
        }

        internal bool IsFilled(int midX, int midY)
        {
            //ai
            var byteIndex = midY * (WIDTH / 8) + midX / 8;
            var bitIndex = midX % 8;
            var b = (byte)(1 << bitIndex);
            return (bits[byteIndex] & b) != 0;
        }

        private void CheckIntersectingLines()
        {
            // naai, te traag  <- AI!!! LOL
            for (int i = 0; i < lines.Count-1; i++)
            {
                for (int j = i+1; j < lines.Count; j++)
                {
                    var line1 = lines[i];
                    var line2 = lines[j];
                    // check intersection


                }
            }
        }

        internal void DrawLines(List<Tile> input, bool clear = false)
        {
            for (int i = 0; i < input.Count - 1; i++)
            {
                DrawLine(input[i], input[i + 1]);
                Console.WriteLine($"Line drawn: {i}");
            }
            DrawLine(input[input.Count - 1], input[0]); ;
        }

        private void DrawLine(Tile tile1, Tile tile2, bool clear = false)
        {
            lines.Add(new Line(tile1, tile2));

            if (tile1.x == tile2.x) // after this, AI
            {
                // vertical line
                var x = tile1.x;
                var y1 = Math.Min(tile1.y, tile2.y);
                var y2 = Math.Max(tile1.y, tile2.y);
                for (int y = y1; y <= y2; y++)
                {
                    SetBit(x, y, clear);
                }
            }
            else if (tile1.y == tile2.y)
            {
                // horizontal line
                var y = tile1.y;
                var x1 = Math.Min(tile1.x, tile2.x);
                var x2 = Math.Max(tile1.x, tile2.x);
                for (int x = x1; x <= x2; x++)
                {
                    SetBit(x, y, clear);
                }
            }
            else
            {
                throw new Exception("only hor/ver lines supported");
            }
        }

        private void SetBit(int x, int y, bool clear = false)
        {
            var byteIndex = y * (WIDTH / 8) + x / 8;
            var bitIndex = x % 8;
            var b = (byte)(1 << bitIndex);

            if (clear)
            {
                bits[byteIndex] &= (byte)~b;

            }
            else
            {
                bits[byteIndex] |= b;
            }
        }
    }

    internal class Line
    {
        private Tile tile1;
        private Tile tile2;

        public Line(Tile tile1, Tile tile2)
        {
            this.tile1 = tile1;
            this.tile2 = tile2;
        }
    }
}