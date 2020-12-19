using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/18.txt";
string[] lines = File.ReadAllLines(path).Select(l => l.Trim())
    .Where(l => l.Length > 0).ToArray();

Evaluator p1 = new Evaluator();
Evaluator p2 = new Evaluator(true);

long sum1 = lines.Sum(l => p1.Evaluate(l));
long sum2 = lines.Sum(l => p2.Evaluate(l));

Console.WriteLine($"Part 1: {sum1}");
Console.WriteLine($"Part 2: {sum2}");

enum Operator
{
    None,
    Plus,
    Times
}

class Evaluator
{
    static readonly char[] _nonDigits = new char[] { '(', ')', '+', '*' };
    static readonly Regex _whitespaceRegex = new Regex(@"\s");

    bool _part2;

    public Evaluator(bool part2 = false)
    {
        _part2 = part2;
    }

    public long Evaluate(string line)
    {
        line = _whitespaceRegex.Replace(line, "");
        if (_part2)
        {
            line = InsertParens(line);
        }
        Stack<long> leftArgs = new Stack<long>();
        Stack<Operator> ops = new Stack<Operator>();
        long leftArg = -1;
        Operator op = Operator.None;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '(')
            {
                leftArgs.Push(leftArg);
                ops.Push(op);
                leftArg = -1;
                op = Operator.None;
            }
            else if (c == ')')
            {
                leftArg = Reduce(leftArgs.Pop(), ops.Pop(), leftArg);
            }
            else if (c == '+')
            {
                op = Operator.Plus;
            }
            else if (c == '*')
            {
                op = Operator.Times;
            }
            else
            {
                int idx = line.IndexOfAny(_nonDigits, i);
                if (idx == -1)
                {
                    idx = line.Length;
                }
                long arg = long.Parse(line.Substring(i, idx - i));
                leftArg = Reduce(leftArg, op, arg);
                op = Operator.None;
                i = idx - 1;
            }
        }
        return leftArg;
    }

    long Reduce(long leftArg, Operator op, long rightArg)
    {
        if (op == Operator.None)
        {
            return rightArg;
        }
        else if (op == Operator.Plus)
        {
            return leftArg + rightArg;
        }
        else
        {
            return leftArg * rightArg;
        }
    }

    string InsertParens(string line)
    {
        StringBuilder sb = new StringBuilder(line);
        for (int i = 0; i < sb.Length; i++)
        {
            if (sb[i] == '+')
            {
                int start = FindEndpoint(sb, i, -1);
                int end = FindEndpoint(sb, i, 1);
                if (!(IsChar(sb, start - 1, '(') && IsChar(sb, end + 1, ')')))
                {
                    sb.Insert(end + 1, ')');
                    sb.Insert(start, '(');
                    i++;
                }
            }
        }
        return sb.ToString();
    }

    int FindEndpoint(StringBuilder sb, int index, int increment)
    {
        char openParen = increment == -1 ? ')' : '(';
        char closeParen = increment == -1 ? '(' : ')';
        char c1 = sb[index + increment];
        if (c1 == openParen)
        {
            int depth = 0;
            for (int i = index + 2 * increment; ; i += increment)
            {
                char c2 = sb[i];
                if (c2 == openParen)
                {
                    depth++;
                }
                else if (c2 == closeParen)
                {
                    if (depth == 0)
                    {
                        return i;
                    }
                    depth--;
                }
            }
        }
        else
        {
            int i = index + 2 * increment;
            for (; i != -1 && i != sb.Length; i += increment)
            {
                if (_nonDigits.Contains(sb[i]))
                {
                    return i - increment;
                }
            }
            return i - increment;
        }
    }

    public bool IsChar(StringBuilder sb, int i, char c)
    {
        if (i < 0 || i >= sb.Length)
        {
            return false;
        }
        return sb[i] == c;
    }

}