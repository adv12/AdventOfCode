using System;
using System.IO;

int GetRow(string spec)
{
    int min = 0;
    int max = 127;
    for (int i = 0; i < 7; i++)
    {
        int diff = 64 / (1 << i);
        if (spec[i] == 'F')
        {
            max -= diff;
        }
        else
        {
            min += diff;
        }
    }
    return min;
}

int GetColumn(string spec)
{
    int min = 0;
    int max = 7;
    for (int i = 0; i < 3; i++)
    {
        int diff = 4 / (1 << i);
        if (spec[i + 7] == 'L')
        {
            max -= diff;
        }
        else
        {
            min += diff;
        }
    }
    return min;
}

int GetSeatId(string spec)
{
    return GetRow(spec) * 8 + GetColumn(spec);
}

(int, bool[]) GetLargestAndSeatMap(string[] lines)
{
    int largest = 0;
    bool[] seats = new bool[128 * 8];
    foreach (string line in lines)
    {
        string spec = line.Trim();
        if (spec.Length == 10)
        {
            int seatId = GetSeatId(spec);
            seats[seatId] = true;
            largest = Math.Max(largest, seatId);
        }
    }
    return (largest, seats);
}

string[] lines = File.ReadAllLines("/Users/andrewvardeman/Desktop/AdventOfCodeInput/05.txt");


(int largest, bool[] seatMap) = GetLargestAndSeatMap(lines);

Console.WriteLine($"Largest: {largest}");

int i;
for (i = 1; i < seatMap.Length - 1; i++)
{
    if (!seatMap[i] && seatMap[i - 1] && seatMap[i + 1])
    {
        break;
    }
}

Console.WriteLine($"My Seat: {i}");