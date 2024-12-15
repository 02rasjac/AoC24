// #define IS_SAMPLE

using System.Text.RegularExpressions;
using Coordinate = (int x, int y);

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] data = File.ReadAllLines(path);
List<Machine> machines = [];
long totalCost = 0;

for (var i = 0; i < data.Length; i++)
{
    Coordinate deltasA = GetCoordinates(data[i++]);
    Coordinate deltasB = GetCoordinates(data[i++]);
    Coordinate prizeCoords = GetCoordinates(data[i++], true);

    var newMachine = new Machine(deltasA, deltasB, prizeCoords);
    machines.Add(newMachine);
    totalCost += newMachine.FindCheapestMoveToPrize();
}

Console.WriteLine($"Part 1: {totalCost}");
// Part 1: 28262

return;

Coordinate GetCoordinates(string line, bool isPrize = false)
{
    string regex = isPrize ? @"=(\d+)" : @"\+(\d+)";

    MatchCollection coords = Regex.Matches(line, regex);
    int x = int.Parse(coords[0].Groups[1].Value);
    int y = int.Parse(coords[1].Groups[1].Value);
    return (x, y);
}

internal class Machine(Coordinate aDeltas, Coordinate bDeltas, Coordinate prizeLocation)
{
    private const int ACost = 3;
    private const int BCost = 1;
    private const int MaxButtonPresses = 100;

    public int FindCheapestMoveToPrize()
    {
        var cheapest = int.MaxValue;
        for (var aPresses = 0; aPresses <= MaxButtonPresses; aPresses++)
        {
            for (int bPresses = MaxButtonPresses; bPresses >= 0; bPresses--)
            {
                Coordinate location = GetClawLocation(aPresses, bPresses);
                if (location.x < prizeLocation.x || location.y < prizeLocation.y)
                    break;
                if (location.x > prizeLocation.x || location.y > prizeLocation.y)
                    continue;

                // The price was found
                int cost = ACost * aPresses + BCost * bPresses;
                if (cost < cheapest)
                    cheapest = cost;
            }
        }

        if (cheapest == int.MaxValue)
            cheapest = 0;

        return cheapest;
    }

    private Coordinate GetClawLocation(int aPresses, int bPresses)
    {
        return (aDeltas.x * aPresses + bDeltas.x * bPresses, aDeltas.y * aPresses + bDeltas.y * bPresses);
    }
}