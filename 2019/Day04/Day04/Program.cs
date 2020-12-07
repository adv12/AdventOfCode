using System;

int GetDigit(int val, int index)
{
    if (index < 0 || index > 5)
    {
        return -1;
    }
    int pow = 6 - index;
    return val % (int)Math.Pow(10, pow) / (int)Math.Pow(10, pow - 1);
}

(bool, bool) Test(int val)
{
    bool repeat = false;
    bool onlyRepeat = false;
    for (int i = 0; i < 6; i++)
    {
        int digit = GetDigit(val, i);
        int prevDigit = GetDigit(val, i - 1);
        if (prevDigit == digit)
        {
            repeat = true;
            if (!onlyRepeat && GetDigit(val, i - 2) != digit && GetDigit(val, i + 1) != digit)
            {
                onlyRepeat = true;
            }
        }
        if (digit < prevDigit)
        {
            return (false, false);
        }
    }
    return (repeat, onlyRepeat);
}

(int, int) Run(int min, int max)
{
    int count1 = 0;
    int count2 = 0;
    for (int i = min; i <= max; i++)
    {
        (bool valid1, bool valid2) = Test(i);
        if (valid1)
        {
            count1++;
        }
        if (valid2)
        {
            count2++;
        }
    }
    return (count1, count2);
}

(int count1, int count2) = Run(153517, 630395);

Console.WriteLine($"Part 1: {count1}; Part 2: {count2}");