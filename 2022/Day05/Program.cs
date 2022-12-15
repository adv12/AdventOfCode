Stack<char>[] GetStacks(List<List<char>> columns)
{
    var stacks = new Stack<char>[columns.Count];
    for (var i = 0; i < columns.Count; i++)
    {
        stacks[i] = new Stack<char>();
        for (var j = columns[i].Count - 1; j >= 0; j--)
        {
            stacks[i].Push(columns[i][j]);
        }
    }
    return stacks;
}

void MovePart1(Stack<char>[] stacks, int count, int fromcolidx, int tocolidx)
{
    for (var i = 0; i < count; i++)
    {
        stacks[tocolidx].Push(stacks[fromcolidx].Pop());
    }
}

void MovePart2(Stack<char>[] stacks, int count, int fromcolidx, int tocolidx, Stack<char> buffer)
{
    for (var i = 0; i < count; i++)
    {
        buffer.Push(stacks[fromcolidx].Pop());
    }
    for (var i = 0; i < count; i++)
    {
        stacks[tocolidx].Push(buffer.Pop());
    }
}


var lines = File.ReadAllLines(args[0]);
var columns = new List<List<char>>();
var skipCount = 0;
foreach (var line in lines)
{
    if (line.Length > 0 && line[0] == 'm')
    {
        break;
    }
    for (var i = 0; i < line.Length; i += 4)
    {
        var colidx = i / 4;
        if (line[i] == '[')
        {
            while (columns.Count < colidx + 1)
            {
                columns.Add(new List<char>());
            }
            columns[colidx].Add(line[i + 1]);
        }
    }
    skipCount++;
}

var stacks1 = GetStacks(columns);
var stacks2 = GetStacks(columns);
var buffer = new Stack<char>();

foreach (var line in lines.Skip(skipCount))
{
    var match = System.Text.RegularExpressions.Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
    int count = int.Parse(match.Groups[1].Value);
    int fromcolidx = int.Parse(match.Groups[2].Value) - 1;
    int tocolidx = int.Parse(match.Groups[3].Value) - 1;
    MovePart1(stacks1, count, fromcolidx, tocolidx);
    MovePart2(stacks2, count, fromcolidx, tocolidx, buffer);
}

foreach (var stack in stacks1)
{
    Console.Write(stack.Peek());
}
Console.WriteLine();

foreach (var stack in stacks2)
{
    Console.Write(stack.Peek());
}
Console.WriteLine();