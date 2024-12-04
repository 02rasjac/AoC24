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

#region Part 1

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

#endregion

nMatches = 0;
for (var i = 1; i < nLines - 1; i++)
for (var j = 1; j < lineLength - 1; j++)
{
    // X
    // string right = string.Concat(data[i][j], data[i + 1][j + 1], data[i + 2][j + 2]);
    // string left = string.Concat(data[i][j + 2], data[i + 1][j + 1], data[i + 2][j]);
    // if (right is "MAS" or "SAM" && left is "MAS" or "SAM") nMatches++;

    // If there is an A or X in the corners or not an A in the center, it's invalid
    // A.A
    // .B.
    // X.A
    if (data[i][j] != 'A' || data[i - 1][j - 1] is 'A' or 'X' || data[i + 1][j - 1] is 'A' or 'X' ||
        data[i + 1][j + 1] is 'A' or 'X' ||
        data[i - 1][j + 1] is 'A' or 'X')
        continue;

    // If the diagonal corners are the same, it's not equal
    // M.S
    // .A.
    // S.M
    if (data[i - 1][j - 1] == data[i + 1][j + 1] || data[i - 1][j + 1] == data[i + 1][j - 1])
        continue;

    // Now, it should only be a version of
    // M.S
    // .A.
    // M.S
    nMatches++;


    // // +
    // string vertical = string.Concat(data[i][j + 1], data[i + 1][j + 1], data[i + 2][j + 1]);
    // string horizontal = string.Concat(data[i + 1][j], data[i + 1][j + 1], data[i + 1][j + 2]);
    // if (vertical is "MAS" or "SAM" && horizontal is "MAS" or "SAM")
    //     nMatches++;
}

Console.WriteLine(nMatches);
// 1833 low
// 1850 Ans