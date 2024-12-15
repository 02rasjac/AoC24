// #define IS_SAMPLE

using System.Text.RegularExpressions;
using Day14;

#if IS_SAMPLE
const string path = "sample.txt";
const int width = 11;
const int height = 7;
#else
const string path = "data.txt";
const int width = 101;
const int height = 103;
#endif

const int maxSeconds = 100;
const int middleX = width / 2;
const int middleY = height / 2;

string[] data = File.ReadAllLines(path);
var robots = new Robot[data.Length];

// Initiate robots
for (var i = 0; i < data.Length; i++)
{
    // The first two matches are coordinates for position, and the other two are for velocity
    MatchCollection matches = Regex.Matches(data[i], @"-\d+|\d+");
    var newRobot = new Robot(new Vector2I(int.Parse(matches[0].Value), int.Parse(matches[1].Value)),
        new Vector2I(int.Parse(matches[2].Value), int.Parse(matches[3].Value)));
    robots[i] = newRobot;
}

for (var second = 1; second <= maxSeconds; second++)
{
    foreach (Robot robot in robots)
    {
        robot.Move(width, height);
    }
}

int quad1 = Count(0, 0, middleX, middleY);
int quad2 = Count(middleX + 1, 0, width, middleY);
int quad3 = Count(0, middleY + 1, middleX, height);
int quad4 = Count(middleX + 1, middleY + 1, width, height);

int safetyFactor = quad1 * quad2 * quad3 * quad4;
Console.WriteLine($"Safety factor: {safetyFactor}");
// Part 1: 230900224

return;

// Start is inclusive, end is exclusive
int Count(int startX, int startY, int endX, int endY)
{
    var count = 0;
    for (int y = startY; y < endY; y++)
    {
        for (int x = startX; x < endX; x++)
        {
            count += robots.Count(robot => robot.Position.X == x && robot.Position.Y == y);
        }
    }

    return count;
}

// TODO: A class representing a robot with its position and velocity
// TODO: An array with items representing a tile

internal class Robot(Vector2I position, Vector2I velocity)
{
    public Vector2I Position { get; private set; } = position;
    private Vector2I Velocity { get; } = velocity;

    public void Move(int mapWidth, int mapHeight)
    {
        Vector2I newPos = Position + Velocity;
        if (newPos.X < 0)
            newPos.X = mapWidth + newPos.X;
        else if (newPos.X >= mapWidth)
            newPos.X = newPos.X - mapWidth;
        if (newPos.Y < 0)
            newPos.Y = mapHeight + newPos.Y;
        else if (newPos.Y >= mapHeight)
            newPos.Y = newPos.Y - mapHeight;

        Position = newPos;
    }
}