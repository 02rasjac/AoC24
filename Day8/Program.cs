// #define IS_SAMPLE

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] data = File.ReadAllLines(path);
int nLines = data.Length;
int lineLength = data[0].Length;

var antiNodes = new Dictionary<(int x, int y), char>();

// PART 1 PSEUDO
/* Go through the data left -> right, top -> bottom.
 ** When it hits a letter or digit => Start a new loop from the position after this letter/digit
 *** Every time it hits the same letter/digit
 **** Calculate the difference in x- and y coordinates (`dx`, `dy`)
 **** In a dictionary, store '#' in coordinates (-dx, -dy) from the first letter/digit and (+dx, +dy) from the second letter/digit, if they are within the range of the map
 * Count how many items the dictionary has to find the number of unique anti nodes
 */

// PART 2 PSEUDO
/* Go through the data left -> right, top -> bottom.
 ** When it hits a letter or digit => Start a new loop from the position after this letter/digit
 *** Every time it hits the same letter/digit
 **** Calculate the difference in x- and y coordinates (`dx`, `dy`)
 **** In a dictionary, store '#' in coordinates (-dx, -dy) from the first letter/digit and (+dx, +dy) from the second letter/digit, if they are within the range of the map
 ***** This needs to be done until (-a*dx, -a*dy) respectively (+a*dx, +a*dy) is outside the map
 * Count how many items the dictionary has to find the number of unique anti nodes
 */

for (var y = 0; y < nLines; y++)
{
    for (var x = 0; x < lineLength; x++)
    {
        char node = data[y][x];
        if (node == '.')
            continue;

        FindAntiNodes(x, y, node);
    }
}

Console.WriteLine(antiNodes.Count);
PrintMapWithAntiNodes();
// Part 1: 409
// Part 2: 1308
return;

void FindAntiNodes(int startX, int startY, char frequency)
{
    // This is needed since `int x = startX + 1` in the for loop would start on startX + 1 every line
    // It is set to `0` in the first iteration of y
    int actualXStart = startX + 1;

    for (int y = startY; y < nLines; y++)
    {
        for (int x = actualXStart; x < lineLength; x++)
        {
            char node = data[y][x];
            if (node != frequency)
                continue;

            int dx = x - startX;
            int dy = y - startY;

            AddNodesIfValid(-dx, -dy, (startX, startY));
            AddNodesIfValid(dx, dy, (x, y));
        }

        actualXStart = 0;
    }
}

void AddNodesIfValid(int dx, int dy, (int x, int y) node)
{
    // If this function is called, that means a pair of antennas has been found and both antennas (which are at `node` coords)
    // needs to be added to `antiNodes`
    antiNodes.TryAdd(node, '#');

    while (true)
    {
        (int x, int y) antiNode = (node.x + dx, node.y + dy);

        if (antiNode.x >= 0 && antiNode.x < lineLength && antiNode.y >= 0 && antiNode.y < nLines)
        {
            antiNodes.TryAdd(antiNode, '#');
            node = antiNode;
        }
        else
        {
            break;
        }
    }
}

void PrintMapWithAntiNodes()
{
    for (var y = 0; y < nLines; y++)
    {
        for (var x = 0; x < lineLength; x++)
        {
            if (antiNodes.ContainsKey((x, y)))
            {
                Console.Write('#');
                continue;
            }

            char node = data[y][x];
            Console.Write(node);
        }

        Console.WriteLine();
    }
}