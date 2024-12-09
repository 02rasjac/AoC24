#define IS_SAMPLE

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

// 2333133121414131402
string map = File.ReadAllLines(path)[0]; // Only one line

var id = 0;
var isFile = true;
var blocks = new List<long>();

// 00...111...2...333.44.5555.6666.777.888899
foreach (char n in map)
{
    int nBlocks = n - '0';
    for (var j = 0; j < nBlocks; j++)
    {
        blocks.Add(isFile ? id : -1);
    }

    if (isFile)
        id++;

    isFile = !isFile;
}

// Part1();

Part2();


long checkSum = 0;
for (var i = 0; i < blocks.Count; i++)
{
    if (blocks[i] < 0)
        continue;
    checkSum += blocks[i] * i;
}

Console.WriteLine(checkSum);
// Part 1: 6430446922192
// Part 2: 6460170597310 high
// Part 2: 6459957690257 low
return;

// 0099811188827773336446555566..............
void Part1()
{
    var frontI = 0;
    for (int backI = blocks.Count - 1; backI >= frontI + 1; backI--)
    {
        // It's empty => ignore
        if (blocks[backI] < 0)
            continue;

        // Look for the closes empty block
        while (blocks[frontI] >= 0)
            frontI++;

        if (backI < frontI)
            break;

        blocks[frontI] = blocks[backI];
        blocks[backI] = -1;
    }
}

// 00992111777.44.333....5555.6666.....8888..
void Part2()
{
    for (int fileEnd = blocks.Count - 1; fileEnd >= 0; fileEnd--)
    {
        // It's empty => ignore
        if (blocks[fileEnd] < 0)
            continue;

        int fileStart = fileEnd;

        while (fileStart > 0 && blocks[fileStart - 1] == blocks[fileEnd])
            fileStart--;

        int fileLength = fileEnd - fileStart + 1;

        int emptyStart = -1;
        for (var i = 0; i < fileStart; i++)
        {
            if (emptyStart < 0)
            {
                // Start of an empty space
                if (blocks[i] < 0)
                    emptyStart = i;

                // Haven't found the start of an empty space
                continue;
            }

            // Ignore if the block isn't empty, and break if it goes above the fileStart index
            if (blocks[i] >= 0 || i == fileStart - 1)
            {
                int emptyEnd = i == fileStart - 1 ? i : i - 1;

                int emptyLength = emptyEnd - emptyStart + 1;
                if (emptyLength >= fileLength)
                {
                    MoveFile(fileStart, emptyStart, fileLength);
                    break;
                }

                emptyStart = -1;
            }
        }

        fileEnd = fileStart;
    }
}

void MoveFile(int fileStart, int emptyStart, int length)
{
    long fileId = blocks[fileStart];
    for (int i = fileStart; i < fileStart + length; i++)
    {
        blocks[i] = -1;
    }

    for (int i = emptyStart; i < emptyStart + length; i++)
    {
        blocks[i] = fileId;
    }
}