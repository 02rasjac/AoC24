// #define IS_SAMPLE

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

var data = File.ReadLines(path).ToList();

if (data.Count == 0) Console.WriteLine("No data found");

var nSafeReports = data.Count(report => IsSafeReportDampened(report.Split(' ').Select(int.Parse).ToList()));

Console.WriteLine($"N-safe reports: {nSafeReports}");
return;

// Returns `-1` if safe, otherwise the index where the error is
int IsSafeReport(List<int> report)
{
    var isIncreasing = report[0] < report[1];

    if (report[1] < report[2] && !isIncreasing) return 0; // The first index may be removable

    for (var i = 0; i < report.Count - 1; i++)
    {
        var diff = Math.Abs(report[i] - report[i + 1]);
        if (diff is > 3 or 0) return i; // Too big of a difference or the same
        if (report[i] < report[i + 1] && !isIncreasing) return i; // Increases, but supposed to decrease
        if (report[i] > report[i + 1] && isIncreasing) return i; // Decreases, but supposed to increase
    }

    return -1;
}

bool IsSafeReportDampened(List<int> report)
{
    var i = IsSafeReport(report);
    if (i < 0) return true;

    var removedFirst = new List<int>(report);
    removedFirst.RemoveAt(i);
    if (IsSafeReport(removedFirst) < 0) return true;

    var removedSecond = new List<int>(report);
    removedSecond.RemoveAt(i + 1);
    if (IsSafeReport(removedSecond) < 0) return true;

    Console.WriteLine($"Unsafe: {string.Join(" ", report)}");

    return false;
}