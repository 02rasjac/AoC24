// #define IS_SAMPLE

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

var stones = File.ReadAllLines(path)[0].Split().ToList(); // Only one file

var blink = 0;
const int maxBlinks = 25;

while (blink < maxBlinks)
{
    for (var i = 0; i < stones.Count; i++)
    {
        string engraving = stones[i];

        if (engraving == "0")
        {
            stones[i] = "1";
        }
        else if (engraving.Length % 2 == 0)
        {
            string firstHalf = engraving[..(engraving.Length / 2)];
            string secondHalf = engraving[(engraving.Length / 2)..].TrimStart('0'); // Also remove the leading zeros
            if (secondHalf == "")
                secondHalf = "0";

            stones[i] = firstHalf;
            stones.Insert(i + 1, secondHalf);
            i++;
        }
        else
        {
            stones[i] = (long.Parse(engraving) * 2024).ToString();
        }
    }

    blink++;
}

Console.WriteLine($"Part 1: {stones.Count}");
// Part 1: 186175