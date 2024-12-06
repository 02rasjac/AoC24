// #define IS_SAMPLE

using Int2 = (int x, int y);

Int2 up = (0, -1);
Int2 down = (0, 1);
Int2 left = (-1, 0);
Int2 right = (1, 0);

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

// map[y][x], with (0, 0) in first line, first character
var map = File.ReadLines(path).Select(line => line.ToCharArray()).ToList();

// (x, y)
Int2 position = (0, 0);
Int2 direction = (0, -1);
var nDistinctPositions = 0;

// Find the starting position
for (var y = 0; y < map.Count; y++)
{
    int x = Array.IndexOf(map[y], '^');
    if (x > 0)
    {
        position = (x, y);
        map[y][x] = 'X';
        nDistinctPositions++;
        break;
    }
}

var isOnMap = true;
while (isOnMap)
{
    // New distinct position
    if (map[position.y][position.x] != 'X')
    {
        nDistinctPositions++;
        map[position.y][position.x] = 'X';
    }

    Int2 checkPos = GetNextPos(position, direction);

    // If it crashes, it reached the end of the map
    try
    {
        if (map[checkPos.y][checkPos.x] == '#')
        {
            direction = RotateRight(direction);
        }
    }
    catch
    {
        isOnMap = false;
    }

    position = GetNextPos(position, direction);
}

Console.WriteLine(nDistinctPositions);
return;

Int2 GetNextPos(Int2 t1, Int2 t2)
{
    return (t1.x + t2.x, t1.y + t2.y);
}

Int2 RotateRight(Int2 t)
{
    if (t.Equals(up))
        return right;
    if (t.Equals(down))
        return left;
    if (t.Equals(left))
        return up;
    return down;
}