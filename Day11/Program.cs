// #define IS_SAMPLE

using System.Diagnostics;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

long startTime = Stopwatch.GetTimestamp();

var stones = File.ReadAllLines(path)[0].Split().ToList(); // Only one file
var stonesCount1 = new Dictionary<string, long>();
var stonesCount2 = new Dictionary<string, long>();
var useCount1 = true;

// Initiate `stonesCount`
foreach (string stone in stones)
{
    stonesCount1.Add(stone, 1);
}

var blink = 0;
const int maxBlinks = 75;

// Every blink, perform the main algorithm on each stone and add the amount of times each new engraving comes in the next blink
// as a dictionary (Example below)
// We then only have to perform the main algo once for each number, but store add the number in the value
/* 0
 * {
 * 0: 1
 * }
 */
/* 1
 * {
 * 0: 0
 * 1: 1
 * }
 */
/* 2024
 * {
 * 0: 0
 * 1: 0
 * 2024: 1
 * }
 */
/* 20 24
 * {
 * 0: 0
 * 1: 0
 * 20: 1
 * 24: 1
 * 2024: 0
 * }
 */
/* 2 0 2 4
 * {
 * 0: 1
 * 1: 0
 * 2: 2
 * 4: 1
 * 20: 0
 * 24: 0
 * 2024: 0
 * }
 */
/* 4048 1 4048 8096
 * {
 * 0: 0
 * 1: 1
 * 2: 0
 * 4: 0
 * 20: 0
 * 24: 0
 * 2024: 0
 * 4048: 2
 * 8096: 1
 * }
 */
/* 40 48 2024 40 48 80 96
 * {
 * 0: 0
 * 1: 0
 * 2: 0
 * 4: 0
 * 20: 0
 * 24: 0
 * 40: 2
 * 48: 2
 * 80: 1
 * 96: 1
 * 2024: 1
 * 4048: 0
 * 8096: 0
 * }
 */
/* 4 0 4 8 20 24 4 0 4 8 8 0 9 6
 * {
 * 0: 3
 * 1: 0
 * 2: 0
 * 4: 4
 * 6: 3
 * 8: 3
 * 9: 1
 * 20: 1
 * 24: 1
 * 40: 0
 * 48: 0
 * 80: 0
 * 96: 0
 * 2024: 0
 * 4048: 0
 * 8096: 0
 * }
 */

while (blink < maxBlinks)
{
    if (useCount1)
    {
        foreach ((string stone, long amount) in stonesCount1)
        {
            CheckStone(stone, amount, stonesCount2);
        }

        stonesCount1.Clear();
    }
    else
    {
        foreach ((string stone, long amount) in stonesCount2)
        {
            CheckStone(stone, amount, stonesCount1);
        }

        stonesCount2.Clear();
    }

    blink++;
    useCount1 = !useCount1;
}

long nStones = useCount1 ? stonesCount1.Sum(stone => stone.Value) : stonesCount2.Sum(stone => stone.Value);

Console.WriteLine($"Part 1: {nStones}");
Console.WriteLine($"Elapsed time: {Stopwatch.GetElapsedTime(startTime)}");
// Part 1: 186175
// Part 2: 220566831337810

void CheckStone(string stone, long multiplier, Dictionary<string, long> stonesCount)
{
    if (stone == "0")
    {
        AddOrIncrement("1", multiplier, stonesCount);
    }
    else if (stone.Length % 2 == 0)
    {
        string firstHalf = stone[..(stone.Length / 2)];
        string secondHalf = stone[(stone.Length / 2)..].TrimStart('0'); // Also remove the leading zeros
        if (secondHalf == "")
            secondHalf = "0";

        AddOrIncrement(firstHalf, multiplier, stonesCount);
        AddOrIncrement(secondHalf, multiplier, stonesCount);
    }
    else
    {
        AddOrIncrement((long.Parse(stone) * 2024).ToString(), multiplier, stonesCount);
    }
}

void AddOrIncrement(string stone, long multiplier, Dictionary<string, long> stonesCount)
{
    if (!stonesCount.TryAdd(stone, multiplier))
        stonesCount[stone] += multiplier;
}