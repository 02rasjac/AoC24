// #define IS_SAMPLE

using System.Text.RegularExpressions;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

var data = File.ReadAllText(path);

var res = 0;
var enabled = true;

var match = Regex.Match(data, @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");
while (match.Success)
{
    var matchValue = match.Value;
    var oldMatch = match;
    match = match.NextMatch();
    if (matchValue.Equals("don't()"))
    {
        enabled = false;
        continue;
    }

    if (matchValue.Equals("do()"))
    {
        enabled = true;
        continue;
    }

    if (!enabled) continue;

    res += int.Parse(oldMatch.Groups[1].Value) * int.Parse(oldMatch.Groups[2].Value);
}

// ans: 98729041
Console.WriteLine(res);