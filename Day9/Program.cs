// #define IS_SAMPLE

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

// 2333133121414131402
string map = File.ReadAllLines(path)[0]; // Only one line

var id = 0;
var isFile = true;
var blocks = new List<int>();

// 00...111...2...333.44.5555.6666.777.888899
for (var i = 0; i < map.Length; i++)
{
    int nBlocks = map[i] - '0';
    for (var j = 0; j < nBlocks; j++)
    {
        blocks.Add(isFile ? id : -1);
    }

    if (isFile)
        id++;

    isFile = !isFile;
}

// 0099811188827773336446555566..............
var frontI = 0;
for (int backI = blocks.Count - 1; backI >= frontI + 1; backI--)
{
    // It's empty => ignore
    if (blocks[backI] < 0)
        continue;

    // Look for the closes empty blcok
    while (blocks[frontI] >= 0)
        frontI++;

    if (backI < frontI)
        break;

    blocks[frontI] = blocks[backI];
    blocks[backI] = -1;
}

long checkSum = 0;
for (var i = 0; i < blocks.Count; i++)
{
    if (blocks[i] < 0)
        break;
    checkSum += blocks[i] * i;
}

Console.WriteLine(checkSum);
// Part 1: 6430446922192