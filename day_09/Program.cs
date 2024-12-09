namespace day_09
{
    // start@0715
    // start coding@0728
    // finish coding@0756
    // p1 example OK
    // p1 ok @ 0804
    // p2 finish coding @0835
    // p2 1 bug2
    // 900 -910 bs
    // p2 buigs fixe
    // p2 finisged @ 914
    // duration: 1h50m

    internal class Program
    {
        static void Main(string[] args)
        {
            var part2 = true;
            //var input = File.ReadAllLines("input_example.txt"); // 1928; p2  1958
            var input = File.ReadAllLines("input.txt"); // input length = 19999; 1st 1449552725 too low :-(  2nd  6323641412437  nth 6351801932670

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
            if (!part2)
            {
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
            }
            else
            {
                var listOfFreeBlocks = new List<(int start, int end)>();
                var listOfFileBlocks = new List<(int start, int end)>();

                var inFreeBlock = blockMap[0] == -1;
                var blockStart = 0;
                var fileBlockId = blockMap[0]; //bug 2  forgot file id change to end block
                for (int i = 1; i < blockCount; i++)
                {
                    if (inFreeBlock)
                    {
                        if (blockMap[i] != -1)
                        {
                            inFreeBlock = false;
                            var blockEnd = i - 1;
                            listOfFreeBlocks.Add((blockStart, blockEnd));
                            blockStart = i;
                            fileBlockId = blockMap[i];
                        }
                    }
                    else // inFileBlock
                    {
                        if (blockMap[i] == -1 || blockMap[i] != fileBlockId)
                        {
                            var blockEnd = i - 1;
                            listOfFileBlocks.Add((blockStart, blockEnd));
                            blockStart = i;
                            fileBlockId = blockMap[i];
                            inFreeBlock = fileBlockId == -1;
                        }
                    }
                }

                var blockEnd2 = blockCount-1;
                if (inFreeBlock)
                {
                    listOfFreeBlocks.Add((blockStart, blockEnd2));
                }
                else // inFileBlock
                {
                    listOfFileBlocks.Add((blockStart, blockEnd2));
                }

                for (var i = listOfFileBlocks.Count - 1; i >= 0; i--)
                {
                    var fb = listOfFileBlocks[i];
                    var fileBlockSize = (fb.end - fb.start) + 1;

                    // can we find a free block to put it in?
                    for (int j = 0; j < listOfFreeBlocks.Count; j++)
                    {
                        var freeb = listOfFreeBlocks[j];
                        if (freeb.start >= fb.start) { break; } // cannot move left

                        //var freebsize = (fb.end - fb.start) + 1; //bug 6
                        var freebsize = (freeb.end - freeb.start) + 1;
                        if (freebsize >= fileBlockSize)
                        {
                            // move file
                            for (int k = 0; k < fileBlockSize; k++)
                            {
                                blockMap[freeb.start + k] = blockMap[fb.start + k]; //bug 3 used diskMap instead of blockMap
                                blockMap[fb.start + k] = -1;
                            }


                            // make fb smaller
                            freeb.start += fileBlockSize;
                            listOfFreeBlocks[j] = freeb; //bug 4 forgot to update it in the list

                            break; // bug 5: forgot break;
                        }
                    }
                }
            }

            var z = new string (blockMap.Select(z => z == -1 ? '.' : z.ToString()[0]).ToArray());

            // 4: calculate checksum
            long checksum = 0; //bug 1: int should be long. WHY NO OVERFLOW ERROR ???
            for (int i = 0; i < blockCount; i++)
            {
                if (blockMap[i] != -1)
                {
                    checksum += i * blockMap[i];
                }
            }

            Console.WriteLine($"Checksum: {checksum}");
        }
    }
}
