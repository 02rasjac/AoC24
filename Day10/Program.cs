// #define IS_SAMPLE

using Coordinate = (int x, int y);

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] map = File.ReadAllLines(path);
int maxY = map.Length;
int maxX = map[0].Length;

List<Node> nodes = new();

// Create a node for every position and add it to `nodes`, where they are kept track of with their index
// Node is the trailhead and the value is the set of tails
var trailHeadsUniqueTails = new Dictionary<Node, HashSet<Node>>();
for (var y = 0; y < maxY; y++)
{
    for (var x = 0; x < maxX; x++)
    {
        // trailHeads.Add((x, y), []);
        int height = map[y][x] - '0';
        var node = new Node((x, y), height);
        if (x > 0)
            node.Neighbors.Add(Direction.Left, CoordToIndex((x - 1, y)));
        if (x < maxX - 1)
            node.Neighbors.Add(Direction.Right, CoordToIndex((x + 1, y)));
        if (y > 0)
            node.Neighbors.Add(Direction.Up, CoordToIndex((x, y - 1)));
        if (y < maxY - 1)
            node.Neighbors.Add(Direction.Down, CoordToIndex((x, y + 1)));

        nodes.Add(node);
        if (node.IsHead)
            trailHeadsUniqueTails.Add(node, []);
    }
}

// Part 2 (rating)
// Perform BFS on each trailHead to find unique ends
var uniqueTrails = 0;
foreach ((Node head, var tails) in trailHeadsUniqueTails)
{
    Bfs(head, tails);
    uniqueTrails += Dfs(head);
}

// Part 1 summation (score)
var sum = 0;
foreach (var node in trailHeadsUniqueTails.Values)
{
    sum += node.Count;
}

Console.WriteLine($"Part 1: {sum}");
Console.WriteLine($"Part 2: {uniqueTrails}");

return;

int CoordToIndex(Coordinate coord)
{
    return coord.y * maxX + coord.x;
}

void Bfs(Node head, HashSet<Node> tails)
{
    int headIndex = CoordToIndex(head.Coordinate);
    var queue = new Queue<int>();
    var visitedByIndex = new Dictionary<int, bool>();
    visitedByIndex.Add(headIndex, true);
    queue.Enqueue(headIndex);
    while (queue.Count > 0)
    {
        int currIndex = queue.Dequeue();
        Node currNode = nodes[currIndex];

        foreach ((Direction _, int neighborIndex) in currNode.Neighbors)
        {
            // If it exists in visited, skip the content, otherwise work with it
            if (visitedByIndex.TryGetValue(neighborIndex, out _)) continue;

            Node neighborNode = nodes[neighborIndex];

            // Only continue when the height increases by exactly one
            if (neighborNode.Height != currNode.Height + 1) continue;

            // End of trail
            if (neighborNode.Height == 9)
                tails.Add(neighborNode);

            queue.Enqueue(neighborIndex);
            visitedByIndex.Add(neighborIndex, true);
        }
    }
}

int Dfs(Node head)
{
    int headIndex = CoordToIndex(head.Coordinate);

    return DfsRec(headIndex, 0);

    int DfsRec(int i, int count)
    {
        Node currNode = nodes[i];

        if (currNode.Height == 9)
        {
            count++;
            return count;
        }

        foreach ((Direction _, int neighborIndex) in currNode.Neighbors)
        {
            Node neighborNode = nodes[neighborIndex];

            // Only continue when the height increases by exactly one
            if (neighborNode.Height != currNode.Height + 1) continue;

            count = DfsRec(neighborIndex, count);
        }

        return count;
    }
}

public class Node(Coordinate coordinate, int height)
{
    public readonly Coordinate Coordinate = coordinate;
    public readonly int Height = height;
    public readonly bool IsHead = height == 0;

    // Up to four directions, the `int` is the index of its neighbour in `nodes`
    public readonly Dictionary<Direction, int> Neighbors = new();
}

public enum Direction
{
    Left = 0,
    Up = 1,
    Right = 2,
    Down = 3
}