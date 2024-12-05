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
var inValidUpdates = new List<List<int>>();

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
        AddInLists(line);
}

// Part 1
int sum1 = validUpdates.Sum(update => update[update.Count / 2]);
Console.WriteLine($"Part 1: {sum1}");

// Part 2
var sum2 = 0;
foreach (var update in inValidUpdates)
{
    // Start with the first page
    // If it's behind one it needs to be infront of, switch their places
    for (var i = 0; i < update.Count; i++)
    {
        int currPage = update[i];
        order.TryGetValue(currPage, out var pagesRequiredAfter);
        if (pagesRequiredAfter == null)
            continue;

        foreach (int page in pagesRequiredAfter)
        {
            int pageIndex = update.IndexOf(page);
            // Doesn't exist or The page is after the current page, which it should
            if (pageIndex < 0 || pageIndex > i)
                continue;

            // Switch places
            update[i] = page;
            update[pageIndex] = currPage;

            // Restart
            // TODO: figure out how to not restart completely to optimise
            i = 0;
            break;
        }
    }

    sum2 += update[update.Count / 2];
}

Console.WriteLine($"Part 2: {sum2}");


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

void AddInLists(string line)
{
    var pages = line.Split(',').Select(int.Parse).ToList();

    foreach (int page in pages)
        if (order.TryGetValue(page, out var pagesAfter))
            foreach (int pageAgain in pages)
            {
                if (pagesAfter.Contains(pageAgain))
                {
                    inValidUpdates.Add(pages);
                    return;
                }

                if (pageAgain == page)
                    break;
            }

    validUpdates.Add(pages);
}