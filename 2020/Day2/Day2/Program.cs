using System;
using System.IO;
using System.Text.RegularExpressions;

Regex regex = new Regex(@"^(\d+)-(\d+) (.): (.*)$");

string[] lines = File.ReadAllLines("/Users/andrewvardeman/Desktop/AdventOfCodeInput/02.txt");

int rule1MatchCount = 0;
int rule2MatchCount = 0;

foreach (string line in lines)
{
    Match match = regex.Match(line);
    int num1 = int.Parse(match.Groups[1].Value);
    int num2 = int.Parse(match.Groups[2].Value);
    char letter = match.Groups[3].Value[0];
    string password = match.Groups[4].Value;

    bool passwordMatchesRule1 = MatchesRule1(num1, num2, letter, password);
    bool passwordMatchesRule2 = MatchesRule2(num1, num2, letter, password);
    //Console.WriteLine($"{num1},{num2},{letter},{password},{passwordMatchesRule1},{passwordMatchesRule2}");
    if (passwordMatchesRule1)
    {
        rule1MatchCount++;
    }
    if (passwordMatchesRule2)
    {
        rule2MatchCount++;
    }
}

Console.WriteLine($"Rule 1 count: {rule1MatchCount}; Rule 2 count: {rule2MatchCount}");

bool MatchesRule1(int min, int max, char letter, string password)
{
    int letterCount = 0;
    foreach (char c in password)
    {
        if (c == letter)
        {
            letterCount++;
        }
    }
    return letterCount >= min && letterCount <= max;
}

bool MatchesRule2(int i1, int i2, char letter, string password)
{
    char c1 = password[i1 - 1];
    char c2 = password[i2 - 1];
    return (c1 == letter && c2 != letter) || (c1 != letter && c2 == letter);
}