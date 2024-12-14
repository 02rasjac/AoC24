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

foreach (Region region in regions)
{
    int nSides = FindHorizontalFencesAbove(region);
    nSides += FindHorizontalFencesBelow(region);
    nSides += FindHorizontalFencesLeft(region);
    nSides += FindHorizontalFencesRight(region);
    region.SetSides(nSides);
    Console.WriteLine($"{nSides} sides up for region {region}");
}

int totalStandardPrice = regions.Sum(region => region.GetPrice());
int totalBulkPrice = regions.Sum(region => region.GetBulkPrice());
Console.WriteLine($"Total price: {totalStandardPrice}");
Console.WriteLine($"Total bulk price: {totalBulkPrice}");
// Part 1: 1465968
// Part 2: 897702

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
            plot.RemoveFence(FenceDirection.Left);
        }

        if (aboveI >= 0 && inGarden[aboveI].plantType == plot.plantType)
        {
            queue.Enqueue(inGarden[aboveI]);
            plot.RemoveFence(FenceDirection.Above);
        }

        if (rightI >= 0 && inGarden[rightI].plantType == plot.plantType)
        {
            queue.Enqueue(inGarden[rightI]);
            plot.RemoveFence(FenceDirection.Right);
        }

        if (belowI >= 0 && inGarden[belowI].plantType == plot.plantType)
        {
            queue.Enqueue(inGarden[belowI]);
            plot.RemoveFence(FenceDirection.Below);
        }

        region.AddPlot(plot);
    }

    return region;
}

int FindHorizontalFencesAbove(Region region)
{
    var nSides = 0;

    for (int y = region.boundingBox[0].y; y <= region.boundingBox[1].y; y++)
    {
        var isSideStarted = false;

        for (int x = region.boundingBox[0].x; x <= region.boundingBox[1].x; x++)
        {
            Plot plot = garden[GetIndex((x, y))];
            if (plot.belongsToRegion != region.regionId)
            {
                isSideStarted = false;
                continue;
            }

            if (plot.IsFencedBorder(FenceDirection.Above))
            {
                if (!isSideStarted)
                {
                    nSides++;
                    isSideStarted = true;
                }

                continue;
            }

            isSideStarted = false;
        }
    }

    return nSides;
}

int FindHorizontalFencesBelow(Region region)
{
    var nSides = 0;

    for (int y = region.boundingBox[0].y; y <= region.boundingBox[1].y; y++)
    {
        var isSideStarted = false;

        for (int x = region.boundingBox[0].x; x <= region.boundingBox[1].x; x++)
        {
            Plot plot = garden[GetIndex((x, y))];
            if (plot.belongsToRegion != region.regionId)
            {
                isSideStarted = false;
                continue;
            }

            if (plot.IsFencedBorder(FenceDirection.Below))
            {
                if (!isSideStarted)
                {
                    nSides++;
                    isSideStarted = true;
                }

                continue;
            }

            isSideStarted = false;
        }
    }

    return nSides;
}

int FindHorizontalFencesLeft(Region region)
{
    var nSides = 0;

    for (int x = region.boundingBox[0].x; x <= region.boundingBox[1].x; x++)
    {
        var isSideStarted = false;
        for (int y = region.boundingBox[0].y; y <= region.boundingBox[1].y; y++)
        {
            Plot plot = garden[GetIndex((x, y))];
            if (plot.belongsToRegion != region.regionId)
            {
                isSideStarted = false;
                continue;
            }

            if (plot.IsFencedBorder(FenceDirection.Left))
            {
                if (!isSideStarted)
                {
                    nSides++;
                    isSideStarted = true;
                }

                continue;
            }

            isSideStarted = false;
        }
    }

    return nSides;
}

int FindHorizontalFencesRight(Region region)
{
    var nSides = 0;

    for (int x = region.boundingBox[0].x; x <= region.boundingBox[1].x; x++)
    {
        var isSideStarted = false;
        for (int y = region.boundingBox[0].y; y <= region.boundingBox[1].y; y++)
        {
            Plot plot = garden[GetIndex((x, y))];
            if (plot.belongsToRegion != region.regionId)
            {
                isSideStarted = false;
                continue;
            }

            if (plot.IsFencedBorder(FenceDirection.Right))
            {
                if (!isSideStarted)
                {
                    nSides++;
                    isSideStarted = true;
                }

                continue;
            }

            isSideStarted = false;
        }
    }

    return nSides;
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

    // [left, top, right, bottom]. True => has fence
    public bool[] fencedBorders = [true, true, true, true];
    public bool isBorder = true;
    public int nFencedBorders = 4;

    public Plot(Coordinate coordinate, char plantType)
    {
        this.coordinate = coordinate;
        this.plantType = plantType;
    }

    public void RemoveFence(FenceDirection direction)
    {
        nFencedBorders--;

        switch (direction)
        {
            case FenceDirection.Left:
                fencedBorders[0] = false;
                break;
            case FenceDirection.Above:
                fencedBorders[1] = false;
                break;
            case FenceDirection.Right:
                fencedBorders[2] = false;
                break;
            case FenceDirection.Below:
                fencedBorders[3] = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        if (nFencedBorders == 0)
            isBorder = false;
    }

    public bool IsFencedBorder(FenceDirection direction)
    {
        return fencedBorders[(int)direction];
    }
}

internal class Region
{
    private static int nextRegionId = 1;
    public readonly Coordinate[] boundingBox = [(-1, -1), (-1, -1)]; // [(left, top), (right, bottom)]
    private readonly List<Plot> plots = [];
    public readonly int regionId;
    private readonly char regionPlant;

    private int area;
    private int perimeter;
    private int sides;

    public Region(char regionPlant)
    {
        regionId = nextRegionId++;
        this.regionPlant = regionPlant;
    }

    public override string ToString()
    {
        return $"{regionId} ({regionPlant})";
    }

    public void AddPlot(Plot plot)
    {
        plots.Add(plot);
        plot.belongsToRegion = regionId;
        area++;
        perimeter += plot.nFencedBorders;

        Coordinate plotCoords = plot.coordinate;
        if (boundingBox[0].x < 0)
        {
            boundingBox[0] = plotCoords;
            boundingBox[1] = plotCoords;
            return;
        }

        // Modify the bounding box accordingly
        if (plotCoords.x < boundingBox[0].x)
            boundingBox[0].x = plotCoords.x;
        if (plotCoords.x > boundingBox[1].x)
            boundingBox[1].x = plotCoords.x;
        if (plotCoords.y < boundingBox[0].y)
            boundingBox[0].y = plotCoords.y;
        if (plotCoords.y > boundingBox[1].y)
            boundingBox[1].y = plotCoords.y;
    }

    public void SetSides(int sides)
    {
        this.sides = sides;
    }

    public int GetPrice()
    {
        return area * perimeter;
    }

    public int GetBulkPrice()
    {
        return area * sides;
    }
}

public enum FenceDirection
{
    Left = 0,
    Above = 1,
    Right = 2,
    Below = 3
}

// When adding a plot to the region, store in which direction its fences goes.
// In the region, store bounding box's top-left coordinate and bottom-right coordinate
// For every region, perform 4 loops from top to bottom, left to right of the bounding box
// First, count the number of connected sides of fences above the plot
// Then, count the number of connected sides of fences below the plot
// Then, count the number of connected sides of fences to the left of the plot
// Lastly, count the number of connected sides of fences to the right of the plot.