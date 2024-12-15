// #define IS_SAMPLE

using Day15;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] data = File.ReadAllLines(path);
List<List<char>> map = [];
var startOfInstructions = 0;
var robotPosition = new Vector2I(0, 0);

// Initiate map
for (var y = 0; y < data.Length; y++)
{
    if (data[y] == "")
    {
        startOfInstructions = y;
        break;
    }

    List<char> row = [];
    for (var x = 0; x < data[y].Length; x++)
    {
        char c = data[y][x];
        if (c == '@')
            robotPosition = new Vector2I(x, y);
        row.Add(c);
    }

    map.Add(row);
}

// Follow the instructions
for (int y = startOfInstructions; y < data.Length; y++)
{
    for (var x = 0; x < data[y].Length; x++)
    {
        char instruction = data[y][x];
        Vector2I direction = instruction switch
        {
            '^' => new Vector2I(0, -1),
            '>' => new Vector2I(1, 0),
            'v' => new Vector2I(0, 1),
            '<' => new Vector2I(-1, 0),
            _ => new Vector2I(0, 0)
        };

        if (AttemptToMove(robotPosition, direction))
        {
            robotPosition += direction;
        }
    }
}

PrintMap();

// Calculate the total GPS coordinate
var totalCoordinate = 0;
for (var y = 0; y < map.Count; y++)
{
    for (var x = 0; x < map[y].Count; x++)
    {
        if (map[y][x] != 'O')
            continue;
        totalCoordinate += 100 * y + x;
    }
}

Console.WriteLine($"Sum of the boxes GPS coordinates: {totalCoordinate}");
// Part 1: 1426855
return;

bool AttemptToMove(Vector2I position, Vector2I direction)
{
    Vector2I nextPosition = position + direction;
    char nextNode = map[nextPosition.Y][nextPosition.X];
    if (nextNode == '#' || direction is { X: 0, Y: 0 })
        return false;

    var canMove = false;
    if (nextNode == 'O')
    {
        canMove = AttemptToMove(nextPosition, direction);
    }

    if (nextNode != '.' && !canMove) return false;

    map[nextPosition.Y][nextPosition.X] = map[position.Y][position.X];
    map[position.Y][position.X] = '.';


    return true;
}

void PrintMap()
{
    foreach (var row in map)
    {
        foreach (char node in row)
        {
            Console.Write(node);
        }

        Console.WriteLine();
    }
}