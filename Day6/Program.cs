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
var defaultMap = File.ReadLines(path).Select(line => line.ToCharArray()).ToList();

// (x, y)
Int2 position, startingPos = (0, 0);
Int2 direction = (0, -1);
var nDistinctPositions = 0;
var nLoops = 0;

// Find the starting position
for (var y = 0; y < defaultMap.Count; y++)
{
    int x = Array.IndexOf(defaultMap[y], '^');
    if (x > 0)
    {
        position = (x, y);
        startingPos = (x, y);
        defaultMap[y][x] = 'X';
        nDistinctPositions++;
        break;
    }
}

// Part 2
// For every position on the map, place an obstacle `#`
// Perform the walk
//  Place a direction character at every position
//  If the guard enters a position with a direction character in the same direction it's traveling
//  AND it's in front of an obstacle
//      It's in a loop
// Else if it crashes, then it reached the end of map and the obstacle failed.

for (var obsY = 0; obsY < defaultMap.Count; obsY++)
{
    for (var obsX = 0; obsX < defaultMap[obsY].Length; obsX++)
    {
        position = startingPos;
        direction = up;

        // Placing an obstacle on an existing obstacle will not cause a loop
        if (defaultMap[obsY][obsX] == '#')
            continue;

        // Not allowed to put directly on the guard
        if (obsX == startingPos.x && obsY == startingPos.y)
            continue;

        var modifiedMap = CopyMap(defaultMap);
        modifiedMap[obsY][obsX] = '#';

        if (IsLooping(modifiedMap))
            nLoops++;
    }
}

Console.WriteLine($"Part 1: {nDistinctPositions}");
// Part 1: 5404
Console.WriteLine($"Part 2: {nLoops}");
// Part 2: 1949 low
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

char GetDirectionChar(Int2 t)
{
    if (t.Equals(up))
        return 'U';
    if (t.Equals(down))
        return 'D';
    if (t.Equals(left))
        return 'L';
    return 'R';
}

List<char[]> CopyMap(List<char[]> map)
{
    var newMap = new List<char[]>();
    foreach (char[] line in map)
    {
        newMap.Add(line.Clone() as char[]);
    }

    return newMap;
}

bool IsLooping(List<char[]> map)
{
    while (true)
    {
        Int2 checkPos = GetNextPos(position, direction);

        // If it crashes, it reached the end of the map
        bool isObstacleInFront;
        try
        {
            isObstacleInFront = map[checkPos.y][checkPos.x] == '#';
        }
        catch
        {
            return false;
        }

        // Check if looping
        if (isObstacleInFront && map[position.y][position.x] == GetDirectionChar(direction))
            return true;

        map[position.y][position.x] = GetDirectionChar(direction);

        if (isObstacleInFront)
        {
            direction = RotateRight(direction);
        }

        position = GetNextPos(position, direction);
    }
}