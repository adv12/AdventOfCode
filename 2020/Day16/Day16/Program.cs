using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

int[] ReadTicket(string line)
{
    return line.Split(',').Select(s => int.Parse(s)).ToArray();
}

(bool, int) IsInvalidTicket(int[] ticket, List<Rule> rules)
{
    foreach (int val in ticket)
    {
        bool anyRule = false;
        foreach (Rule rule in rules)
        {
            if (rule.IsInRange(val))
            {
                anyRule = true;
                break;
            }
        }
        if (!anyRule)
        {
            return (true, val);
        }
    }
    return (false, 0);
}

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/16.txt";
string[] lines = File.ReadAllLines(path).Select(l => l.Trim())
    .Where(l => l.Length > 0).ToArray();

bool readingTickets = false;

int[] myTicket = { };
List<int[]> nearbyTickets = new List<int[]>();
List<Rule> rules = new List<Rule>();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];
    if (line == "your ticket:")
    {
        readingTickets = true;
        myTicket = ReadTicket(lines[i + 1]);
        i += 2;
    }
    else
    {
        if (readingTickets)
        {
            nearbyTickets.Add(ReadTicket(line));
        }
        else
        {
            rules.Add(new Rule(line));
        }
    }
}

int answer1 = nearbyTickets.Sum(t => IsInvalidTicket(t, rules).Item2);

Console.WriteLine($"Part 1: {answer1}");

List<int[]> allValidTickets = nearbyTickets
    .Where(t => !IsInvalidTicket(t, rules).Item1).Append(myTicket).ToList();

foreach (Rule rule in rules)
{
    rule.InitCandidates(myTicket.Length);
}

foreach (int[] ticket in allValidTickets)
{
    for (int i = 0; i < ticket.Length; i++)
    {
        foreach (Rule rule in rules)
        {
            rule.Test(ticket[i], i);
        }
    }
}

rules = rules.OrderBy(r => r.CandidateCount).ToList();

for (int i = 0; i < rules.Count; i++)
{
    for (int j = i + 1; j < rules.Count; j++)
    {
        rules[j].Candidates[rules[i].Index] = false;
    }
}

long answer2 = rules.Where(r => r.Name.StartsWith("departure"))
    .Select(r => (long)myTicket[r.Index]).Aggregate((x, y) => x * y);

Console.WriteLine($"Part 2: {answer2}");



class Rule
{
    static readonly Regex _regex =
        new Regex(@"(?<Name>.*?): (?<Min1>\d+)-(?<Max1>\d+) or (?<Min2>\d+)-(?<Max2>\d+)");

    public string Name { get; set; }
    int Min1 { get; set; }
    int Max1 { get; set; }
    int Min2 { get; set; }
    int Max2 { get; set; }

    public bool[] Candidates { get; set; }

    public int CandidateCount => Candidates.Count(x => x);

    public int Index => Array.IndexOf(Candidates, true);

    public Rule(string line)
    {
        Match match = _regex.Match(line);
        if (!match.Success)
        {
            throw new ArgumentException();
        }
        Name = match.Groups["Name"].Value;
        Min1 = int.Parse(match.Groups["Min1"].Value);
        Max1 = int.Parse(match.Groups["Max1"].Value);
        Min2 = int.Parse(match.Groups["Min2"].Value);
        Max2 = int.Parse(match.Groups["Max2"].Value);
    }

    public void InitCandidates(int size)
    {
        Candidates = new bool[size];
        Array.Fill(Candidates, true);
    }

    public bool IsInRange1(int val)
    {
        return val >= Min1 && val <= Max1;
    }

    public bool IsInRange2(int val)
    {
        return val >= Min2 && val <= Max2;
    }

    public bool IsInRange(int val)
    {
        return IsInRange1(val) || IsInRange2(val);
    }

    public void Test(int val, int i)
    {
        if (!IsInRange(val))
        {
            Candidates[i] = false;
        }
    }
}