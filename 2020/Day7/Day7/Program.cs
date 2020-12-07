using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

void GetContainers(string bag, Dictionary<string, HashSet<string>> bagToContainer,
    HashSet<string> knownContainers)
{
    if (!bagToContainer.ContainsKey(bag))
    {
        return;
    }
    foreach (string container in bagToContainer[bag])
    {
        if (!knownContainers.Contains(container))
        {
            knownContainers.Add(container);
            GetContainers(container, bagToContainer, knownContainers);
        }
    }
}

int CountContainedBags(string container,
    Dictionary<string, List<BagAndCount>> containerToBags)
{
    if (!containerToBags.ContainsKey(container))
    {
        return 0;
    }
    int count = 0;
    foreach (BagAndCount bagAndCount in containerToBags[container])
    {
        count += bagAndCount.Count * (1 + CountContainedBags(bagAndCount.Bag, containerToBags));
    }
    return count;
}

string[] lines = File.ReadAllLines("/Users/andrewvardeman/Desktop/AdventOfCodeInput/07.txt");

lines = lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

var containerToBags = new Dictionary<string, List<BagAndCount>>();
var bagToContainer = new Dictionary<string, HashSet<string>>();

foreach (string line in lines)
{
    string simplified = Regex.Replace(line, @"bags?|\.", "");
    string[] parts = Regex.Split(simplified, " contain ").Select(s => s.Trim()).ToArray();
    string container = parts[0];
    string bagListString = parts[1];
    if (bagListString != "no other")
    {
        string[] countAndBagStrings = bagListString.Split(',').Select(s => s.Trim()).ToArray();
        List<BagAndCount> bagAndCountList = new List<BagAndCount>();
        containerToBags[container] = bagAndCountList;
        foreach (string countAndBagString in countAndBagStrings)
        {
            int idx = countAndBagString.IndexOf(" ");
            int count = int.Parse(countAndBagString.Substring(0, idx).Trim());
            string bag = countAndBagString.Substring(idx + 1).Trim();
            if (!bagToContainer.ContainsKey(bag))
            {
                bagToContainer[bag] = new HashSet<string>();
            }
            bagToContainer[bag].Add(container);
            bagAndCountList.Add(new BagAndCount(bag, count));
        }
    }
}

HashSet<string> knownContainers = new HashSet<string>();

GetContainers("shiny gold", bagToContainer, knownContainers);

long containedBagCount = CountContainedBags("shiny gold", containerToBags);

Console.WriteLine($"Part 1: {knownContainers.Count}");
Console.WriteLine($"Part 2: {containedBagCount}");

class BagAndCount
{
    public string Bag { get; set; }
    public int Count { get; set; }

    public BagAndCount(string bag, int count)
    {
        Bag = bag;
        Count = count;
    }
}