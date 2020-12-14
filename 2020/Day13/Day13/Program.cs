using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics;

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/13.txt";
long earliestTimestamp;
long[] buses;

using (StreamReader reader = new StreamReader(path))
{
    earliestTimestamp = long.Parse(reader.ReadLine());
    string[] busStrings = reader.ReadLine().Split(',');
    buses = new long[busStrings.Length];
    for (int i = 0; i < busStrings.Length; i++)
    {
        string bs = busStrings[i];
        buses[i] = bs == "x" ? -1 : long.Parse(bs);
    }
}

(long bus, long minutes) = buses
    .Where(b => b != -1)
    .Select(b => (b, (b - earliestTimestamp % b) % b))
    .OrderBy(p => p.Item2)
    .First();

Console.WriteLine($"Part 1: {bus * minutes}");

long timestamp = 0;



for (int i = 0; i < buses.Length; i++)
{
    long b = buses[i];
    if (b == -1)
    {
        continue;
    }
    long period = Euclid.LeastCommonMultiple(buses.Take(i).Where(b => b != -1).ToArray());
    while ((timestamp + i) % b != 0)
    {
        timestamp += period;
    }
}

Console.WriteLine($"Part 2: {timestamp}");
