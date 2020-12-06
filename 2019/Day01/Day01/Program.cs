using System;
using System.IO;
using System.Linq;

int GetFuel(int mass)
{
    return Math.Max(mass / 3 - 2, 0);
}

int GetTotalFuel(int mass)
{
    int fuel = GetFuel(mass);
    if (fuel > 0)
    {
        fuel += GetTotalFuel(fuel);
    }
    return fuel;
}

string[] lines = File.ReadAllLines("/Users/andrewvardeman/Desktop/AOC2019Input/01.txt");
int[] masses = lines.Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => int.Parse(l)).ToArray();

Console.WriteLine($"Part 1: {masses.Sum(m => GetFuel(m))}");
Console.WriteLine($"Part 2: {masses.Sum(m => GetTotalFuel(m))}");