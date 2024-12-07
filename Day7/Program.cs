// #define IS_SAMPLE

using System.Text.RegularExpressions;

#if IS_SAMPLE
const string path = "sample.txt";
#else
const string path = "data.txt";
#endif

string[] data = File.ReadAllLines(path);

long sumValid = 0;

foreach (string line in data)
{
    // "3267: 81 40 27" => [3267, 81, 40, 27]
    long[] numbers = Regex.Matches(line, @"\d+").Select(match => long.Parse(match.Value)).ToArray();

    sumValid += GetValidResult(numbers);
}

Console.WriteLine($"Part 1: {sumValid}"); // 850435817339


return;

// Return 0 if invalid, otherwise the result
long GetValidResult(long[] numbers)
{
    long expectedRes = numbers[0];

    // Initialize operators
    var operators = new char[numbers.Length - 2];
    for (var i = 0; i < operators.Length; i++)
        operators[i] = '+';

    // Check the result for every combination of operators
    do
    {
        long testRes = numbers[1];

        // Do the math left to right
        for (var i = 0; i < operators.Length; i++)
        {
            switch (operators[i])
            {
                case '+':
                    testRes += numbers[i + 2];
                    break;
                case '*':
                    testRes *= numbers[i + 2];
                    break;
            }
        }

        if (testRes == expectedRes)
            return testRes;
    } while (!IncrementOperators(operators));

    return 0;
}

// Return true if `input` is only multiply. Otherwise false
bool IncrementOperators(char[] input)
{
    // It doesn't contain any addition => last operations
    if (!input.Contains('+'))
        return true;

    for (int i = input.Length - 1; i >= 0; i--)
    {
        if (input[i] == '+')
        {
            input[i] = '*';
            break;
        }

        input[i] = '+';
    }

    return false;
}