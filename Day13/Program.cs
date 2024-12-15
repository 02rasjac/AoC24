#define IS_SAMPLE

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
    private const long MaxButtonPresses = 10000000000000;

    public long FindCheapestMoveToPrize()
    {
        var cheapest = long.MaxValue;
        for (long aPresses = 0; aPresses <= MaxButtonPresses; aPresses++)
        {
            for (long bPresses = MaxButtonPresses; bPresses >= 0; bPresses--)
            {
                Coordinate location = GetClawLocation(aPresses, bPresses);
                if (location.x < prizeLocation.x || location.y < prizeLocation.y)
                    break;
                if (location.x > prizeLocation.x || location.y > prizeLocation.y)
                    continue;

                // The price was found
                long cost = ACost * aPresses + BCost * bPresses;
                if (cost < cheapest)
                    cheapest = cost;
            }
        }

        if (cheapest == int.MaxValue)
            cheapest = 0;

        return cheapest;
    }

    private Coordinate GetClawLocation(long aPresses, long bPresses)
    {
        return (aDeltas.x * aPresses + bDeltas.x * bPresses, aDeltas.y * aPresses + bDeltas.y * bPresses);
    }
}