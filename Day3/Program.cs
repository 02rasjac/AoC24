// #define IS_SAMPLE

using System.Text.RegularExpressions;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

var data = File.ReadAllText(path);

var res = 0;

var match = Regex.Match(data, @"mul\(\d{1,3},\d{1,3}\)");
while (match.Success)
{
    var numbers = Regex.Matches(match.Value, @"(\d{1,3})");
    res += int.Parse(numbers[0].Value) * int.Parse(numbers[1].Value);
    match = match.NextMatch();
}

Console.WriteLine(res);