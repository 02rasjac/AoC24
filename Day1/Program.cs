// See https://aka.ms/new-console-template for more information

// #define IS_SAMPLE

#if IS_SAMPLE
var path = "sample.txt";
#else
var path = "data.txt";
#endif

var data = File.ReadLines(path).ToList();

if (data.Count == 0)
{
    Console.WriteLine("No data found");
    return;
}

var leftList = new int[data.Count];
var rightList = new int[data.Count];


for (var i = 0; i < data.Count; i++)
{
    var values = data[i].Split("   ");
    leftList[i] = int.Parse(values[0]);
    rightList[i] = int.Parse(values[1]);
}

Array.Sort(leftList);
Array.Sort(rightList);

// Part 1
// var diff = new int[data.Count];
// var sumDiff = 0;
// for (var i = 0; i < leftList.Length; i++)
// {
//     diff[i] = Math.Abs(leftList[i] - rightList[i]);
//     sumDiff += diff[i];
// }
//
// Console.WriteLine(sumDiff);

// Part 2
var index = 0;
var result = 0;
while (index < leftList.Length)
{
    var leftValue = leftList[index];

    // Count number of value in the right list
    var countRight = rightList.Count(value => value == leftValue);

    // Count number of duplicates in left list
    var countLeft = 0;
    while (index < leftList.Length && leftList[index] == leftValue)
    {
        ++countLeft;
        ++index;
    }

    result += leftValue * countLeft * countRight;
}

// Ans 2: 22776016
Console.WriteLine(result);