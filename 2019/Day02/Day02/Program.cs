using System;
using System.IO;
using System.Linq;

long RunProgram(long[] memory)
{
    long ip = 0;
    while (true)
    {
        switch (memory[ip])
        {
            case 99:
                return memory[0];
            case 1:
                memory[memory[ip + 3]] = memory[memory[ip + 1]] + memory[memory[ip + 2]];
                ip += 4;
                break;
            case 2:
                memory[memory[ip + 3]] = memory[memory[ip + 1]] * memory[memory[ip + 2]];
                ip += 4;
                break;
        }
    }
}

long PartOne(long[] program)
{
    var memory = (long[])program.Clone();

    memory[1] = 12;
    memory[2] = 2;

    return RunProgram(memory);
}

long PartTwo(long[] program)
{
    for (int i = 0; i < 100; i++)
    {
        for (int j = 0; j < 100; j++)
        {
            var memory = (long[])program.Clone();

            memory[1] = i;
            memory[2] = j;

            if (RunProgram(memory) == 19690720)
            {
                return 100 * i + j;
            }
        }
    }
    return -1;
}

string text = File.ReadAllText("/Users/andrewvardeman/Desktop/AOC2019Input/02.txt").Trim();

long[] program = text.Split(',').Select(s => long.Parse(s)).ToArray();

Console.WriteLine(PartOne(program));

Console.WriteLine(PartTwo(program));
