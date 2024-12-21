// #define IS_SAMPLE

using Day15;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] data = File.ReadAllLines(path);
List<List<char>> map = [];
var isBoxPartsMovable = new Dictionary<(int x, int y), (bool canMove, char c)>();

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
        switch (c)
        {
            case '@':
                robotPosition = new Vector2I(x * 2, y);
                row.AddRange('@', '.');
                break;
            case 'O':
                row.AddRange('[', ']');
                break;
            default:
                row.AddRange(c, c);
                break;
        }
    }

    map.Add(row);
}

// PrintMap();

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

        isBoxPartsMovable.Clear();
    }
}

PrintMap();

// Calculate the total GPS coordinate
var totalCoordinate = 0;
for (var y = 0; y < map.Count; y++)
{
    for (var x = 0; x < map[y].Count; x++)
    {
        if (map[y][x] != '[')
            continue;
        totalCoordinate += 100 * y + x;
    }
}

Console.WriteLine($"Sum of the boxes GPS coordinates: {totalCoordinate}");
// Part 1: 1426855
// Part 2: 1404917
return;

// Do checks for moving vertically only from the left part of the box
// If both nodes next to it is empty => move 
// If any of them is a wall => don't move
// If any of them belong to a box, repeat the check from the left part of that box
//* Store the boxes left index in a dictionary as the key and a boolean if it can move.
//* Check all boxes that's connected to the first one.
//* If all boxes can move, do it, otherwise don't do anything

bool AttemptToMove(Vector2I position, Vector2I direction, char og = '@')
{
    Vector2I nextPosition = position + direction;
    char nextNode = map[nextPosition.Y][nextPosition.X];
    if (nextNode == '#' || direction is { X: 0, Y: 0 })
        return false;

    if (og == '@' && nextNode == '.')
    {
        map[nextPosition.Y][nextPosition.X] = og;
        map[position.Y][position.X] = '.';
        return true;
    }

    var canMove = false;
    // Do the checks vertically
    if (direction.Y != 0)
    {
        if (nextNode == ']')
            nextPosition.X -= 1;

        switch (map[nextPosition.Y][nextPosition.X])
        {
            case '.':
                isBoxPartsMovable[(position.X, position.Y)] = (true, og);
                return true;
            case '#':
                isBoxPartsMovable[(position.X, position.Y)] = (false, og);
                return false;
        }

        if (map[nextPosition.Y][nextPosition.X] is '[' or ']')
            canMove = AttemptToMove(nextPosition, direction, '[');
        if (map[nextPosition.Y][nextPosition.X + 1] is '[' or ']')
            canMove = canMove && AttemptToMove(nextPosition + new Vector2I(1, 0), direction, ']');

        if (og != '@')
        {
            if (!canMove)
            {
                isBoxPartsMovable[(position.X, position.Y)] = (false, og);
                return false;
            }

            isBoxPartsMovable[(position.X, position.Y)] = (true, og);
            return true;
        }

        // Only the original call to `AttemptToMove` with `og == '@'` should come here.
        if (!canMove)
            return false;

        // Move all the boxes and robot 
        foreach (((int x, int y), (bool _, char c)) in isBoxPartsMovable)
        {
            map[y + direction.Y][x] = c;
            map[y][x] = '.';
        }

        map[position.Y + direction.Y][position.X] = '@';
        map[position.Y][position.X] = '.';
        return true;
    }

    // Do the checks horizontally
    if (nextNode is '[' or ']')
    {
        canMove = AttemptToMove(nextPosition, direction, nextNode);
    }

    if (nextNode != '.' && !canMove) return false;

    map[nextPosition.Y][nextPosition.X] = og;
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