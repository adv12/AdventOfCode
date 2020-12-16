using System;
using System.Collections.Generic;

long Part1(long[] seed)
{
    return FindNth(seed, 2020);
}

long Part2(long[] seed)
{
    return FindNth(seed, 30000000);
}

long FindNth(long[] seed, long n)
{
    Dictionary<long, long> lastIndices = new Dictionary<long, long>();
    long i;
    long lastValue = 0;
    for (i = 0; i < seed.Length; i++)
    {
        if (i > 0)
        {
            lastIndices[lastValue] = i - 1;
        }
        lastValue = seed[i];
    }
    for (i = seed.Length; i < n; i++)
    {
        long currentValue;
        if (!lastIndices.ContainsKey(lastValue))
        {
            currentValue = 0;
        }
        else
        {
            currentValue = i - 1 - lastIndices[lastValue];
        }
        lastIndices[lastValue] = i - 1;
        lastValue = currentValue;
    }
    return lastValue;
}

Console.WriteLine($"Part 1 for 1,3,2: {Part1(new long[] { 1, 3, 2 })}");
Console.WriteLine($"Part 1 for 2,1,3: {Part1(new long[] { 2, 1, 3 })}");
Console.WriteLine($"Part 1 for 1,2,3: {Part1(new long[] { 1, 2, 3 })}");
Console.WriteLine($"Part 1 for 2,3,1: {Part1(new long[] { 2, 3, 1 })}");
Console.WriteLine($"Part 1 for 3,2,1: {Part1(new long[] { 3, 2, 1 })}");
Console.WriteLine($"Part 1 for 3,1,2: {Part1(new long[] { 3, 1, 2 })}");
Console.WriteLine($"Part 1 for 14,1,17,0,3,20: {Part1(new long[] { 14, 1, 17, 0, 3, 20 })}");

Console.WriteLine($"Part2 for 0,3,6: {Part2(new long[] { 0, 3, 6 })}");
Console.WriteLine($"Part2 for 1,3,2: {Part2(new long[] { 1, 3, 2 })}");
Console.WriteLine($"Part2 for 2,1,3: {Part2(new long[] { 2, 1, 3 })}");
Console.WriteLine($"Part2 for 1,2,3: {Part2(new long[] { 1, 2, 3 })}");
Console.WriteLine($"Part2 for 2,3,1: {Part2(new long[] { 2, 3, 1 })}");
Console.WriteLine($"Part2 for 3,2,1: {Part2(new long[] { 3, 2, 1 })}");
Console.WriteLine($"Part2 for 3,1,2: {Part2(new long[] { 3, 1, 2 })}");
Console.WriteLine($"Part2 for 14,1,17,0,3,20: {Part2(new long[] { 14, 1, 17, 0, 3, 20 })}");