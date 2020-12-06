using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

string[] lines = File.ReadAllLines("/Users/andrewvardeman/Desktop/AdventOfCodeInput/06.txt");

var part1Groups = new List<HashSet<char>>();
var part2Groups = new List<HashSet<char>>();

HashSet<char> part1Group = null;
HashSet<char> part2Group = null;

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i].Trim();
    if (line.Length == 0)
    {
        part1Group = null;
        part2Group = null;
        continue;
    }
    HashSet<char> person = line.ToHashSet();
    if (part1Group == null)
    {
        part1Group = person;
        part1Groups.Add(part1Group);
        part2Group = line.ToHashSet();
        part2Groups.Add(part2Group);
    }
    part1Group.UnionWith(person);
    part2Group.IntersectWith(person);
}

Console.WriteLine(part1Groups.Sum(g => g.Count));
Console.WriteLine(part2Groups.Sum(g => g.Count));
