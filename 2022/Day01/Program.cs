var lines = File.ReadAllLines(args[0]);
var sums = new int[lines.Length];
int i = 0;
foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        i++;
    }
    else
    {
        sums[i] += int.Parse(line);
    }
}
Array.Sort(sums, (l, r) => r - l);
Console.WriteLine(sums[0]);
Console.WriteLine(sums[0] + sums[1] + sums[2]);
