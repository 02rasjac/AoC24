﻿// #define IS_SAMPLE

using Coordinate = (int x, int y);

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

// 2333133121414131402
string[] map = File.ReadAllLines(path);
int maxY = map.Length;
int maxX = map[0].Length;

List<Node> nodes = new();

// Create a node for every position and add it to `nodes`, where they are kept track of with their index
// Node is the trailhead and the value is the set of tails
var trailHeads = new Dictionary<Node, HashSet<Node>>();
for (var y = 0; y < maxY; y++)
{
    for (var x = 0; x < maxX; x++)
    {
        // trailHeads.Add((x, y), []);
        int height = map[y][x] - '0';
        var node = new Node((x, y), height);
        if (x > 0)
            node.neighbors.Add(Direction.Left, CoordToIndex((x - 1, y)));
        if (x < maxX - 1)
            node.neighbors.Add(Direction.Right, CoordToIndex((x + 1, y)));
        if (y > 0)
            node.neighbors.Add(Direction.Up, CoordToIndex((x, y - 1)));
        if (y < maxY - 1)
            node.neighbors.Add(Direction.Down, CoordToIndex((x, y + 1)));

        nodes.Add(node);
        if (node.isHead)
            trailHeads.Add(node, []);
    }
}

// Perform BFS on each trailHead to find unique ends
foreach ((Node head, var tails) in trailHeads)
{
    int headIndex = CoordToIndex(head.coordinate);
    var queue = new Queue<int>();
    var visitedByIndex = new Dictionary<int, bool>();
    visitedByIndex.Add(headIndex, true);
    queue.Enqueue(headIndex);
    while (queue.Count > 0)
    {
        int currIndex = queue.Dequeue();
        Node currNode = nodes[currIndex];

        foreach ((Direction direction, int neighborIndex) in currNode.neighbors)
        {
            // If it exists in visited, skip the content, otherwise work with it
            if (!visitedByIndex.TryGetValue(neighborIndex, out _))
            {
                Node neighborNode = nodes[neighborIndex];

                // Only continue when the height increases by exactly one
                if (neighborNode.height == currNode.height + 1)
                {
                    // End of trail
                    if (neighborNode.height == 9)
                        tails.Add(neighborNode);

                    queue.Enqueue(neighborIndex);
                    visitedByIndex.Add(neighborIndex, true);
                }
            }
        }
    }
}

var sum = 0;
foreach (var node in trailHeads.Values)
{
    sum += node.Count;
}

Console.WriteLine($"Part 1: {sum}");

return;

int CoordToIndex(Coordinate coord)
{
    return coord.y * maxX + coord.x;
}

// var nodes = new Dictionary<(int x, int y), Node>();
// // Node record:
// // x and y coordinates
// // its height value
// // References to its adjacent nodes, with `null` if it's out of bounds
// // boolean isHead
// // List of references to nodes for unique tails
public class Node
{
    public readonly Coordinate coordinate;
    public readonly int height;
    public readonly bool isHead;

    // Up to four directions, the `int` is the index of its neighbour in `nodes`
    public Dictionary<Direction, int> neighbors = new();

    public Node(Coordinate coordinate, int height)
    {
        this.coordinate = coordinate;
        this.height = height;
        isHead = height == 0;
    }
}

public enum Direction
{
    Left = 0,
    Up = 1,
    Right = 2,
    Down = 3
}