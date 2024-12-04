// #define IS_SAMPLE

using System.Text.RegularExpressions;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] data = File.ReadAllLines(path);
int lineLength = data[0].Length;
int nLines = data.Length;

var nMatches = 0;

// Regex for horizontal (forwards and backwards)
nMatches = data.Sum(line => Regex.Matches(line, @"XMAS").Count + Regex.Matches(line, "SAMX").Count); // 5 for sample

// Vertical (forwards and backwards)
// For each line i and letter j, look forward to i+3 (4 letters). Check the word
for (var i = 0; i < nLines - 3; i++)
for (var j = 0; j < lineLength; j++)
{
    string s = string.Concat(data[i][j], data[i + 1][j], data[i + 2][j], data[i + 3][j]);
    if (s is "XMAS" or "SAMX")
        nMatches++;
} //nMatches = 8 for sample

// Diagonal right (forwards and backwards)
// For each line i, letter j, look forward to i+3 and j+3
for (var i = 0; i < nLines - 3; i++)
for (var j = 0; j < lineLength - 3; j++)
{
    string s = string.Concat(data[i][j], data[i + 1][j + 1], data[i + 2][j + 2], data[i + 3][j + 3]);
    if (s is "XMAS" or "SAMX")
        nMatches++;
}

// Diagonal left (forwards and backwards)
// Same as right diagonal
for (var i = 0; i < nLines - 3; i++)
for (var j = 3; j < lineLength; j++)
{
    string s = string.Concat(data[i][j], data[i + 1][j - 1], data[i + 2][j - 2], data[i + 3][j - 3]);
    if (s is "XMAS" or "SAMX")
        nMatches++;
}

Console.WriteLine(nMatches);