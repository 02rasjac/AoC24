// #define IS_SAMPLE

using System.Diagnostics;
using System.Text.RegularExpressions;
using Coordinate = (long x, long y);

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

long startTime = Stopwatch.GetTimestamp();

string[] data = File.ReadAllLines(path);
long totalCost = 0;

for (var i = 0; i < data.Length; i++)
{
    Coordinate deltasA = GetCoordinates(data[i++]);
    Coordinate deltasB = GetCoordinates(data[i++]);
    Coordinate prizeCoords = GetCoordinates(data[i++], true);
    prizeCoords.x += 10000000000000;
    prizeCoords.y += 10000000000000;

    var newMachine = new Machine(deltasA, deltasB, prizeCoords);
    totalCost += newMachine.FindCheapestMoveToPrize();
    Console.WriteLine($"Elapsed time from start: {Stopwatch.GetElapsedTime(startTime)}");
}

Console.WriteLine($"Part 1: {totalCost}");
// Part 1: 28262
// Part 2: 101406661266314

return;

Coordinate GetCoordinates(string line, bool isPrize = false)
{
    string regex = isPrize ? @"=(\d+)" : @"\+(\d+)";

    MatchCollection coords = Regex.Matches(line, regex);
    long x = int.Parse(coords[0].Groups[1].Value);
    long y = int.Parse(coords[1].Groups[1].Value);
    return (x, y);
}

internal class Machine(Coordinate aDeltas, Coordinate bDeltas, Coordinate prizeLocation)
{
    private const int ACost = 3;
    private const int BCost = 1;

    public long FindCheapestMoveToPrize()
    {
        // See "math.md"
        long bPresses;
        long numerator = prizeLocation.x * aDeltas.y - aDeltas.x * prizeLocation.y;
        long denominator = bDeltas.x * aDeltas.y - aDeltas.x * bDeltas.y;
        if (numerator % denominator == 0)
            bPresses = numerator / denominator;
        else
            return 0;

        long aPresses;
        numerator = prizeLocation.y - bPresses * bDeltas.y;
        denominator = aDeltas.y;
        if (numerator % denominator == 0)
            aPresses = numerator / denominator;
        else
            return 0;

        return aPresses * ACost + bPresses * BCost;
    }
}