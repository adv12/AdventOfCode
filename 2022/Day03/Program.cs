public static class Program {

    static readonly ulong AllChars;
    static Program(){
        ulong allChars = 0;
        for (int i = 1; i <= 52; i++)
        {
            SetBitOn(ref allChars, i);
        }
        AllChars = allChars;
    }

    static void SetBitOn(ref ulong mask, int bit)
    {
        mask = mask | ((ulong)1 << bit);
    }

    static int ToPriority(this char c)
    {
        int idx = (int)c;
        if (idx >= 65 && idx <= 90)
        {
            return idx - 65 + 27;
        }
        return idx - 97 + 1;
    }

    static int ToPriority(this ulong mask)
    {
        for (int i = 1; i <= 52; i++)
        {
            if ((ulong)1 << i == mask)
            {
                return i;
            }
        }
        return 0;
    }

    static ulong ToMask(this string val)
    {
        ulong mask = 0;
        foreach (char c in val)
        {
            SetBitOn(ref mask, c.ToPriority());
        }
        return mask;
    }

    static string GetHalf(this string line, bool second)
    {
        int halfLength = line.Length / 2;
        return line.Substring(second ? halfLength : 0, halfLength);
    }

    static int FindCommonPriority(params string[] strings)
    {
        return strings.Select(s => s.ToMask()).Aggregate(AllChars, (set, nextSet) => set & nextSet).ToPriority();
    }

    static int GetPart1Priority(this string line)
    {
        return FindCommonPriority(line.GetHalf(false), line.GetHalf(true));
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines(args[0]).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        Console.WriteLine(lines.Select(l => l.GetPart1Priority()).Sum());
        
        int sum = 0;
        for (int i = 0; i < lines.Length; i += 3)
        {
            sum += FindCommonPriority(lines[i], lines[i + 1], lines[i+2]);
        }
        Console.WriteLine(sum);
    }

}