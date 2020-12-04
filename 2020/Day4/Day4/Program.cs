using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("/Users/andrewvardeman/Desktop/AdventOfCodeInput/04.txt");
List<Passport> passports = new List<Passport>();

Passport current = new Passport();
passports.Add(current);
for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i].Trim();
    if (line.Length == 0)
    {
        current = null;
        continue;
    }
    if (current == null)
    {
        current = new Passport();
        passports.Add(current);
    }
    current.ParseLine(line);
}

Console.WriteLine($"lines.Length: {lines.Length}");
Console.WriteLine($"passports.Count: {passports.Count}");
Console.WriteLine($"Valid passport count: {passports.Count(p => p.Valid)}");

class Passport
{
    static readonly Regex hgtRegex = new Regex(@"^(\d+)(cm|in)$");
    static readonly Regex hclRegex = new Regex("^#[0-9a-f]{6}$");
    static readonly Regex eclRegex = new Regex("^amb|blu|brn|gry|grn|hzl|oth$");
    static readonly Regex pidRegex = new Regex(@"^\d{9}$");

    public bool byr; //(Birth Year)
    public bool iyr; //(Issue Year)
    public bool eyr; //(Expiration Year)
    public bool hgt; //(Height)
    public bool hcl; //(Hair Color)
    public bool ecl; //(Eye Color)
    public bool pid; //(Passport ID)
    public bool cid; //(Country ID)

    public bool Valid => byr && iyr && eyr && hgt && hcl && ecl && pid;

    public void ParseLine(string line)
    {
        string[] fields = Regex.Split(line, @"\s");
        foreach (string field in fields)
        {
            int idx = field.IndexOf(":");
            string name = field.Substring(0, idx);
            string value = field.Substring(idx + 1);
            int intval;
            if (!int.TryParse(value, out intval))
            {
                intval = -1;
            }
            switch (name)
            {
                case "byr":
                    byr = value.Length == 4 && intval >= 1920 && intval <= 2002;
                    break;
                case "iyr":
                    iyr = value.Length == 4 && intval >= 2010 && intval <= 2020;
                    break;
                case "eyr":
                    eyr = value.Length == 4 && intval >= 2020 && intval <= 2030;
                    break;
                case "hgt":
                    Match match = hgtRegex.Match(value);
                    if (match.Success)
                    {
                        string units = match.Groups[2].Value;
                        string hstr = match.Groups[1].Value;
                        if (hstr.Length == 0)
                        {
                            hstr = match.Groups[2].Value;
                        }
                        int h = int.Parse(hstr);
                        if (units == "cm")
                        {
                            hgt = h >= 150 && h <= 193;
                        }
                        else
                        {
                            hgt = h >= 59 && h <= 76;
                        }
                    }
                    break;
                case "hcl":
                    hcl = hclRegex.IsMatch(value);
                    break;
                case "ecl":
                    ecl = eclRegex.IsMatch(value);
                    break;
                case "pid":
                    pid = pidRegex.IsMatch(value);
                    break;
                case "cid":
                    cid = true;
                    break;
            }
        }
    }
}