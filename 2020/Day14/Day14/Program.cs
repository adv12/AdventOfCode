using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/14.txt";
string[] lines = File.ReadAllLines(path).Select(l => l.Trim())
    .Where(l => l.Length > 0).ToArray();

Console.WriteLine($"Part 1: {new Computer1().RunProgram(lines)}");
Console.WriteLine($"Part 2: {new Computer2().RunProgram(lines)}");

abstract class Computer
{
    static readonly Regex _maskRegex = new Regex(@"^mask = (.*)$");
    static readonly Regex _memRegex = new Regex(@"^mem\[(\d+)\] = (\d+)$");

    protected Dictionary<long, long> Memory { get; set; } = new Dictionary<long, long>();

    protected string _mask = null;
    protected long _and { get; set; } = long.MaxValue;
    protected long _or = 0;

    public long RunProgram(string[] instructions)
    {
        foreach (string instruction in instructions)
        {
            ProcessInstruction(instruction);
        }
        return Memory.Sum(kvp => kvp.Value);
    }

    protected abstract void SetMemory(long index, long val);

    void ProcessInstruction(string instruction)
    {
        Match memMatch = _memRegex.Match(instruction);
        if (memMatch.Success)
        {
            long index = long.Parse(memMatch.Groups[1].Value);
            long val = long.Parse(memMatch.Groups[2].Value);
            SetMemory(index, val);
        }
        else
        {
            Match maskMatch = _maskRegex.Match(instruction);
            if (maskMatch.Success)
            {
                SetMask(maskMatch.Groups[1].Value);
            }
        }
    }

    void SetMask(string mask)
    {
        _mask = mask;
        _and = long.MaxValue;
        _or = 0;
        for (int i = 0; i < mask.Length; i++)
        {
            char c = mask[mask.Length - 1 - i];

            if (c == '0')
            {
                _and ^= (long)1 << i;
            }
            else if (c == '1')
            {
                _or ^= (long)1 << i;
            }
        }
    }
}

class Computer1 : Computer
{
    protected override void SetMemory(long index, long val)
    {
        ApplyMask(ref val);
        Memory[index] = val;
    }

    void ApplyMask(ref long val)
    {
        val = val & _and | _or;
    }
}

class Computer2 : Computer
{
    protected override void SetMemory(long index, long val)
    {
        index |= _or;
        List<long> newAddresses = new List<long>();
        List<long> addresses = new List<long>();
        addresses.Add(0);
        for (int i = 0; i < 36; i++)
        {
            long bit = (long)1 << i;
            long indexAndBit = index & bit;
            foreach (long address in addresses)
            {
                bool floating = _mask[_mask.Length - 1 - i] == 'X';
                if ((indexAndBit) > 0)
                {
                    newAddresses.Add(address | bit);
                    if (floating)
                    {
                        newAddresses.Add(address);
                    }
                }
                else
                {
                    newAddresses.Add(address);
                    if (floating)
                    {
                        newAddresses.Add(address | bit);
                    }
                }
            }
            var tmp = addresses;
            addresses = newAddresses;
            newAddresses = tmp;
            newAddresses.Clear();
        }
        foreach (long address in addresses)
        {
            Memory[address] = val;
        }
    }
}