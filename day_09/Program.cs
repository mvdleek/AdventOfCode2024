namespace day_09
{
    // start@0715
    // start coding@0728
    // finish coding@0756
    // p1 example OK
    // p1 ok @ 0804
    // p2

    internal class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("input_example.txt"); // 1928
            var input = File.ReadAllLines("input.txt"); // input length = 19999; 1st 1449552725 too low :-(  2nd  6323641412437

            var diskMap = input[0].Select(c => c - '0').ToArray();

            // 1: loop to determine the number of blocks   
            var blockCount = 0;
            for (int i = 0; i < diskMap.Length; i++)
            {
                blockCount += diskMap[i];
            }
            //blockCount = 94551 blocks for input

            var blockMap = new int[blockCount];
            Array.Fill(blockMap, -1); // -1 = free space

            // 2: loop to write the blockMap
            var writeIndex = 0;
            var lastId = -1;
            for (var i = 0; i < diskMap.Length; i++)
            {
                if (i % 2 == 0) // file block count
                {
                    lastId++;
                    for (var j = 0; j < diskMap[i]; j++)
                    {
                        blockMap[writeIndex] = lastId;
                        writeIndex++;
                    }
                }
                else // free space block count
                {
                    writeIndex += diskMap[i];
                }
            }

            // 3: compact the blockMap
            var freeBlockIndex = 0;
            var lastFileBlockIndex = blockMap.Length - 1;
            while (blockMap[freeBlockIndex] != -1) { freeBlockIndex++; }
            while (blockMap[lastFileBlockIndex] == -1) { lastFileBlockIndex--; }

            // freeBlockIndex is at a free block
            // lastFileBlockIndex is at a file block that should be moved if possible
            while (freeBlockIndex <= lastFileBlockIndex) // freeblock before the last file block
            {
                // move file
                blockMap[freeBlockIndex] = blockMap[lastFileBlockIndex];
                blockMap[lastFileBlockIndex] = -1;

                while (blockMap[freeBlockIndex] != -1) { freeBlockIndex++; }
                while (blockMap[lastFileBlockIndex] == -1) { lastFileBlockIndex--; }
            }

            // 4: calculate checksum
            long checksum = 0; //bug 1: int should be long. WHY NO OVERFLOW ERROR ???
            for (int i = 0; i < blockCount; i++)
            {
                if (blockMap[i] == -1) { break; }

                checksum += i * blockMap[i];
            }

            Console.WriteLine($"Checksum: {checksum}");
        }
    }
}
