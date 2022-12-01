using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

(Dictionary<int, Rule>, List<string>) Parse(string[] lines)
{
    bool parsingRules = true;
    Dictionary<int, Rule> rules = new Dictionary<int, Rule>();
    List<string> messages = new List<string>();

    foreach (string line in lines)
    {
        if (parsingRules)
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2)
            {
                int rulenum = int.Parse(parts[0].Trim());
                new Rule(rulenum, parts[1], rules);
            }
            else
            {
                parsingRules = false;
            }
        }
        if (!parsingRules)
        {
            messages.Add(line);
        }
    }
    return (rules, messages);
}

bool MatchesRule0 (string message, Dictionary<int, Rule> rules)
{
    Rule r = rules[0];
    HashSet<int> starts = new HashSet<int>();
    starts.Add(0);
    bool isMatch = r.IsMatch(message, rules, starts, out Dictionary<int, HashSet<int>> startsToLengths);
    return isMatch && startsToLengths.First().Value.Contains(message.Length);
}

string path1 = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/19.txt";
string path2 = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/19_2.txt";

string[] lines1 = File.ReadAllLines(path1).Select(l => l.Trim())
    .Where(l => l.Length > 0).ToArray();

string[] lines2 = File.ReadAllLines(path2).Select(l => l.Trim())
    .Where(l => l.Length > 0).ToArray();

(Dictionary<int, Rule> rules1, List<string> messages) = Parse(lines1);
(Dictionary<int, Rule> rules2, _) = Parse(lines1);

int count1 = messages.Count(m => MatchesRule0(m, rules1));
int count2 = messages.Count(m => MatchesRule0(m, rules2));

Console.WriteLine($"Part 1: {count1}");
Console.WriteLine($"Part 2: {count2}");

class Rule
{
    Dictionary<int, Rule> _rules;
    RuleList[] _ruleLists;
    char? _char;

    public Rule(int rulenum, string spec, Dictionary<int, Rule> rules)
    {
        _rules = rules;
        _rules[rulenum] = this;
        spec = spec.Trim();
        if (spec[0] == '"')
        {
            _char = spec[1];
        }
        else
        {
            string[] parts = spec.Split("|");
            _ruleLists = new RuleList[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                _ruleLists[i] = new RuleList(parts[i].Trim().Split(' ')
                    .Select(s => int.Parse(s)).ToArray(), _rules);
            }
        }
    }

    public GetRuleLists(int length, HashSet<RuleList> ruleLists)
    {

    }

    public bool IsMatch(string message, Dictionary<int, Rule> rules,
        HashSet<int> starts, out Dictionary<int, HashSet<int>> startsToLengths)
    {
        startsToLengths = new Dictionary<int, HashSet<int>>();
        
        foreach (int start in starts)
        {
            HashSet<int> lengths = new HashSet<int>();
            startsToLengths[start] = lengths;
            if (start >= message.Length)
            {
                continue;
            }
            if (_char.HasValue)
            {
                if (_char == message[start])
                {
                    lengths.Add(1);
                }
            }
            else
            {
                foreach (int[] ruleList in _ruleLists)
                {
                    bool success = true;
                    HashSet<int> tmpStarts = new HashSet<int>();
                    tmpStarts.Add(start);
                    foreach (int i in ruleList)
                    {
                        HashSet<int> tmpStarts2 = new HashSet<int>();
                        Rule r = rules[i];
                        if (r.IsMatch(message, rules, tmpStarts, out Dictionary<int, HashSet<int>> stl))
                        {
                            foreach (KeyValuePair<int, HashSet<int>> kvp in stl)
                            {
                                foreach (int val in kvp.Value)
                                {
                                    int newStart = kvp.Key + val;
                                    if (newStart <= message.Length)
                                    tmpStarts2.Add(newStart);
                                }
                            }
                            tmpStarts = tmpStarts2;
                        }
                        else
                        {
                            success = false;
                            break;
                        }
                    }
                    if (success)
                    {
                        lengths.UnionWith(tmpStarts.Select(s => s - start).ToHashSet());
                    }
                }
            }
        }
        return startsToLengths.Any(kvp => kvp.Value.Any());
    }
}

class RuleList
{
    int[] _ruleIds;
    Dictionary<int, Rule> _rules;

    public RuleList(int[] ruleIds, Dictionary<int, Rule> rules)
    {
        _ruleIds = ruleIds;
        _rules = rules;
    }

    public override bool Equals(object obj)
    {
        if (obj is RuleList that)
        {
            return StructuralComparisons.StructuralEqualityComparer
                .Equals(this._ruleIds, that._ruleIds);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return StructuralComparisons.StructuralEqualityComparer
            .GetHashCode(_ruleIds);
    }
}