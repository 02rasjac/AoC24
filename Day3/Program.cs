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

var match = Regex.Match(data, @"mul\(\d{1,3},\d{1,3}\)|don't\(\)|do\(\)");
while (match.Success)
{
    var matchValue = match.Value;
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

    var numbers = Regex.Matches(matchValue, @"(\d{1,3})");
    res += int.Parse(numbers[0].Value) * int.Parse(numbers[1].Value);
}

Console.WriteLine(res);