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

bool IsSafeReport(List<int> report)
{
    var isIncreasing = report[0] < report[1];

    if (report[1] < report[2] && !isIncreasing) return false; // The first index may be removable

    for (var i = 0; i < report.Count - 1; i++)
    {
        var diff = Math.Abs(report[i] - report[i + 1]);
        if (diff is > 3 or 0) return false; // Too big of a difference or the same
        if (report[i] < report[i + 1] && !isIncreasing) return false; // Increases, but supposed to decrease
        if (report[i] > report[i + 1] && isIncreasing) return false; // Decreases, but supposed to increase
    }

    return true;
}

bool IsSafeReportDampened(List<int> report)
{
    var safe = IsSafeReport(report);
    if (safe) return true;

    for (var i = 0; i < report.Count; i++)
    {
        var cut = new List<int>(report);
        cut.RemoveAt(i);
        if (IsSafeReport(cut)) return true;
    }

    return false;
}