// #define IS_SAMPLE

using System.Text.RegularExpressions;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

// string[] data = File.ReadAllLines(path);
var order = new Dictionary<int, List<int>>();
var validUpdates = new List<List<int>>();

var readingOrder = true;

foreach (string line in File.ReadLines(path))
{
    if (line.Length == 0)
    {
        readingOrder = false;
        continue;
    }

    if (readingOrder)
        ReadOrder(line);
    else
        AddIfValid(line);
}

int sum = validUpdates.Sum(update => update[update.Count / 2]);
Console.WriteLine(sum);
return;

// Part 1: 4766

void ReadOrder(string line)
{
    MatchCollection pages = Regex.Matches(line, @"\d+");
    int firstPage = int.Parse(pages[0].Value);
    int secondPage = int.Parse(pages[1].Value);

    // Add second page to the list of pages after the first page
    if (order.TryGetValue(firstPage, out var pagesAfter))
        pagesAfter.Add(secondPage);
    else
        order.Add(firstPage, [secondPage]);
}

void AddIfValid(string line)
{
    var pages = line.Split(',').Select(int.Parse).ToList();

    foreach (int page in pages)
        if (order.TryGetValue(page, out var pagesAfter))
            foreach (int pageAgain in pages)
            {
                if (pagesAfter.Contains(pageAgain))
                    return; // Invalid
                if (pageAgain == page)
                    break;
            }

    validUpdates.Add(pages);
}

Console.WriteLine(validUpdates.Count);