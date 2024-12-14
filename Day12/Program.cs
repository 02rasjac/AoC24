// #define IS_SAMPLE

using Coordinate = (int x, int y);

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

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
    var newGarden = new Plot[initialGarden.Length * initialGarden[0].Length];

    maxX = initialGarden[0].Length - 1;
    maxY = initialGarden.Length - 1;

    for (var y = 0; y <= maxY; y++)
    {
        for (var x = 0; x <= maxX; x++)
        {
            newGarden[GetIndex((x, y))] = new Plot((x, y), initialGarden[y][x]);
        }
    }

    return newGarden;
}

List<Region> FindRegions(Plot[] inGarden)
{
    return (from plot in inGarden where plot.belongsToRegion < 0 select FindRegion(plot, inGarden)).ToList();
}

// Perform floodfill to find all plots that's in a region
Region FindRegion(Plot startPlot, Plot[] inGarden)
{
    var region = new Region(startPlot.plantType);

    var queue = new Queue<Plot>();
    queue.Enqueue(startPlot);

    while (queue.Count > 0)
    {
        Plot plot = queue.Dequeue();

        // This plot has already been checked
        if (plot.belongsToRegion >= 0)
            continue;

        CheckAndEnqueueAdjacentPlot(FenceDirection.Left, GetPreviousPlotIndex(plot.coordinate), plot);
        CheckAndEnqueueAdjacentPlot(FenceDirection.Above, GetAbovePlotIndex(plot.coordinate), plot);
        CheckAndEnqueueAdjacentPlot(FenceDirection.Right, GetNextPlotIndex(plot.coordinate), plot);
        CheckAndEnqueueAdjacentPlot(FenceDirection.Below, GetBelowPlotIndex(plot.coordinate), plot);

        region.AddPlot(plot);
    }

    return region;

    void CheckAndEnqueueAdjacentPlot(FenceDirection direction, int nextIndex, Plot currPlot)
    {
        if (nextIndex < 0 || inGarden[nextIndex].plantType != currPlot.plantType)
            return;

        queue.Enqueue(inGarden[nextIndex]);
        currPlot.RemoveFence(direction);
    }
}

// TODO: Reduce reused code for these functions.
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

internal class Plot(Coordinate coordinate, char plantType)
{
    // [left, top, right, bottom]. True => has fence
    private readonly bool[] _fencedBorders = [true, true, true, true];
    public readonly Coordinate coordinate = coordinate;
    public readonly char plantType = plantType;

    public int belongsToRegion = -1;
    public bool isBorder = true;
    public int nFencedBorders = 4;

    public void RemoveFence(FenceDirection direction)
    {
        nFencedBorders--;

        switch (direction)
        {
            case FenceDirection.Left:
                _fencedBorders[0] = false;
                break;
            case FenceDirection.Above:
                _fencedBorders[1] = false;
                break;
            case FenceDirection.Right:
                _fencedBorders[2] = false;
                break;
            case FenceDirection.Below:
                _fencedBorders[3] = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        if (nFencedBorders == 0)
            isBorder = false;
    }

    public bool IsFencedBorder(FenceDirection direction)
    {
        return _fencedBorders[(int)direction];
    }
}

internal class Region
{
    private static int _nextRegionId = 1;
    private readonly List<Plot> _plots = [];
    private readonly char _regionPlant;
    public readonly Coordinate[] boundingBox = [(-1, -1), (-1, -1)]; // [(left, top), (right, bottom)]
    public readonly int regionId;

    private int _area;
    private int _perimeter;
    private int _sides;

    public Region(char regionPlant)
    {
        regionId = _nextRegionId++;
        _regionPlant = regionPlant;
    }

    public override string ToString()
    {
        return $"{regionId} ({_regionPlant})";
    }

    public void AddPlot(Plot plot)
    {
        _plots.Add(plot);
        plot.belongsToRegion = regionId;
        _area++;
        _perimeter += plot.nFencedBorders;

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
        _sides = sides;
    }

    public int GetPrice()
    {
        return _area * _perimeter;
    }

    public int GetBulkPrice()
    {
        return _area * _sides;
    }
}

public enum FenceDirection
{
    Left = 0,
    Above = 1,
    Right = 2,
    Below = 3
}