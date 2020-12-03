using System;
using System.IO;

string[] lines = File.ReadAllLines("/users/andrewvardeman/Desktop/AdventOfCodeInput/03.txt");

long count11 = GetTreeCount(1, 1);
long count31 = GetTreeCount(3, 1);
long count51 = GetTreeCount(5, 1);
long count71 = GetTreeCount(7, 1);
long count12 = GetTreeCount(1, 2);
Console.WriteLine($"{count11},{count31},{count51},{count71},{count12},{count11 * count31 * count51 * count71 * count12}");


int GetTreeCount(int right, int down)
{
    int x = 0;
    int treeCount = 0;
    for (int l = 0; l < lines.Length; l += down, x += right)
    {
        string line = lines[l].Trim();
        if (line[x % line.Length] == '#')
        {
            treeCount++;
            //Console.WriteLine(l + "," + x);
        }
    }
    return treeCount;
}