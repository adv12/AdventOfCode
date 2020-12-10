using System;
using System.IO;
using System.Linq;

int GetInvalidIndex(long[] vals)
{
    for (int i = 25; i < vals.Length; i++)
    {
        if (!IsValid(i, vals))
        {
            return i;
        }
    }
    return -1;
}

bool IsValid(int i, long[] vals)
{
    long val = vals[i];
    for (int j = i - 25; j < i; j++)
    {
        for (int k = j + 1; k < i; k++)
        {
            if (vals[j] != vals[k] && vals[j] + vals[k] == val)
            {
                return true;
            }
        }
    }
    return false;
}

long Part2(long val, long[] vals)
{
    for (int i = 0; i < vals.Length; i++)
    {
        long sum = vals[i];
        for (int j = i + 1; j < vals.Length; j++)
        {
            sum += vals[j];
            if (sum == val)
            {
                // sacrificing efficiency for coolness...
                var range = vals.Skip(i).Take(j - i + 1).ToList();
                return range.Max() + range.Min();
            }
        }
    }
    return -1;
}

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/09.txt";

long[] vals = File.ReadAllLines(path).Where(l => l.Trim().Length > 0)
    .Select(l => long.Parse(l)).ToArray();

int invalidIndex = GetInvalidIndex(vals);
long answer1 = vals[invalidIndex];
long answer2 = Part2(answer1, vals);

Console.WriteLine($"Part 1: {answer1}");
Console.WriteLine($"Part 2: {answer2}");