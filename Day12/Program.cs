// #define IS_SAMPLE

using System.Diagnostics;
using Coordinate = (int x, int y);

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

long startTime = Stopwatch.GetTimestamp();
var maxX = 0;
var maxY = 0;

var garden = InitGarden();
var regions = FindRegions(garden);

int totalPrice = regions.Sum(region => region.GetPrice());
Console.WriteLine($"Total price: {totalPrice}");

return;

Plot[] InitGarden()
{
    string[] initialGarden = File.ReadAllLines(path);
    var garden = new Plot[initialGarden.Length * initialGarden[0].Length];

    maxX = initialGarden[0].Length - 1;
    maxY = initialGarden.Length - 1;

    for (var y = 0; y <= maxY; y++)
    {
        for (var x = 0; x <= maxX; x++)
        {
            garden[GetIndex((x, y))] = new Plot((x, y), initialGarden[y][x]);
        }
    }

    return garden;
}

List<Region> FindRegions(Plot[] inGarden)
{
    var regions = new List<Region>();
    foreach (Plot plot in inGarden)
    {
        // The plot has already been added to a region
        if (plot.belongsToRegion >= 0)
            continue;
        regions.Add(FindRegion(plot, inGarden));
    }

    return regions;
}

Region FindRegion(Plot startPlot, Plot[] inGarden)
{
    var region = new Region(startPlot.plantType);

    var queue = new Queue<Plot>();
    queue.Enqueue(startPlot);

    while (queue.Count > 0)
    {
        Plot plot = queue.Dequeue();

        // // This plot has already been checked
        if (plot.belongsToRegion >= 0)
            continue;

        int i = GetIndex(plot.coordinate);

        int leftI = GetPreviousPlotIndex(plot.coordinate);
        int aboveI = GetAbovePlotIndex(plot.coordinate);
        int rightI = GetNextPlotIndex(plot.coordinate);
        int belowI = GetBelowPlotIndex(plot.coordinate);

        if (leftI >= 0 && inGarden[leftI].plantType == plot.plantType)
        {
            queue.Enqueue(inGarden[leftI]);
            plot.nFencedBorders--;
        }

        if (aboveI >= 0 && inGarden[aboveI].plantType == plot.plantType)
        {
            queue.Enqueue(inGarden[aboveI]);
            plot.nFencedBorders--;
        }

        if (rightI >= 0 && inGarden[rightI].plantType == plot.plantType)
        {
            queue.Enqueue(inGarden[rightI]);
            plot.nFencedBorders--;
        }

        if (belowI >= 0 && inGarden[belowI].plantType == plot.plantType)
        {
            queue.Enqueue(inGarden[belowI]);
            plot.nFencedBorders--;
        }

        region.AddPlot(plot);
    }

    return region;
}

#region HelperFunctions

int GetIndex(Coordinate coordinate)
{
    return (maxX + 1) * coordinate.y + coordinate.x;
}

int GetPreviousPlotIndex(Coordinate coordinate)
{
    if (coordinate.x == 0)
        return -1;
    return (maxX + 1) * coordinate.y + coordinate.x - 1;
}

int GetNextPlotIndex(Coordinate coordinate)
{
    if (coordinate.x == maxX)
        return -1;
    return (maxX + 1) * coordinate.y + coordinate.x + 1;
}

int GetAbovePlotIndex(Coordinate coordinate)
{
    if (coordinate.y == 0)
        return -1;
    return (maxX + 1) * (coordinate.y - 1) + coordinate.x;
}

int GetBelowPlotIndex(Coordinate coordinate)
{
    if (coordinate.y == maxY)
        return -1;
    return (maxX + 1) * (coordinate.y + 1) + coordinate.x;
}

#endregion

internal class Plot
{
    public readonly Coordinate coordinate;
    public readonly char plantType;
    public int belongsToRegion = -1;
    public int nFencedBorders = 4;

    public Plot(Coordinate coordinate, char plantType)
    {
        this.coordinate = coordinate;
        this.plantType = plantType;
    }
}

internal class Region
{
    private static int nextRegionId = 1;
    private readonly List<Plot> plots = [];
    private readonly int regionId;
    private readonly char regionPlant;

    private int area;
    private int perimeter;

    public Region(char regionPlant)
    {
        regionId = nextRegionId++;
        this.regionPlant = regionPlant;
    }

    public void AddPlot(Plot plot)
    {
        plots.Add(plot);
        plot.belongsToRegion = regionId;
        area++;
        perimeter += plot.nFencedBorders;
    }

    public int GetPrice()
    {
        return area * perimeter;
    }
}