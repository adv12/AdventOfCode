using System;
using System.Collections.Generic;
using System.IO;

string[] lines = File.ReadAllLines(args[0]);
var vals = new List<int>(lines.Length);
bool twoValues = false;
bool threeValues = false;
foreach (string line in lines)
{
    vals.Add(int.Parse(line.Trim()));
}
for (int i = 0; i < vals.Count; i++)
{
    for (int j = 0; j < vals.Count; j++)
    {
        if (i != j && vals[i] + vals[j] == 2020)
        {
            Console.WriteLine("Two values: " + (vals[i] * vals[j]));
            twoValues = true;
            if (threeValues)
            {
                return;
            }
        }
        if (!threeValues)
        {
            for (int k = 0; k < vals.Count; k++)
            {
                if (i != j && i != k && j != k && vals[i] + vals[j] + vals[k] == 2020)
                {
                    Console.WriteLine("Three values: " + vals[i] * vals[j] * vals[k]);
                    threeValues = true;
                    if (twoValues)
                    {
                        return;
                    }
                }
            }
        }
    }
}

