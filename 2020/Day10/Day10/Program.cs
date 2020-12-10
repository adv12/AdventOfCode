using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/10.txt";

List<int> adapters = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l))
    .Select(l => int.Parse(l)).ToList();

adapters.Sort();

adapters.Append(adapters[adapters.Count - 1] + 3);

int[] prevAdaptersInRange = new int[adapters.Count];

long GetRouteCount(List<int> adapters, List<int>[] curToPrev, long[] routeCounts, int curIndex)
{
    long routeCount = 0;
    foreach (int prevIndex in curToPrev[curIndex])
    {
        if (routeCounts[prevIndex] == -1)
        {
            routeCounts[prevIndex] = GetRouteCount(adapters, curToPrev, routeCounts, prevIndex);
        }
        routeCount += routeCounts[prevIndex];
    }
    if (adapters[curIndex] <= 3)
    {
        routeCount++;
    }
    return routeCount;
}

int prev = 0;
int num1 = 0;
int num3 = 0;
var curToPrev = new List<int>[adapters.Count];
long[] routeCounts = new long[adapters.Count];
Array.Fill(routeCounts, -1);
for (int i = 0; i < adapters.Count; i++)
{
    int cur = adapters[i];
    int diff = cur - prev;
    if (diff == 1)
    {
        num1++;
    }
    else if (diff == 3)
    {
        num3++;
    }

    var prevs = new List<int>();
    for (int j = i - 1; j >= 0 && j > i - 4; j--)
    {
        if (cur - adapters[j] <= 3)
        {
            prevs.Add(j);
        }
    }
    curToPrev[i] = prevs;
    prev = cur;
}

Console.WriteLine($"Part 1: {num1 * num3}");
Console.WriteLine($"Part 2: {GetRouteCount(adapters, curToPrev, routeCounts, adapters.Count - 1)}");

