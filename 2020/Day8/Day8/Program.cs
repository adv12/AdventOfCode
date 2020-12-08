using System;
using System.IO;
using System.Linq;

long RunProgram(Instruction[] program, out bool infiniteLoop)
{
    long accumulator = 0;
    infiniteLoop = false;
    bool[] visited = new bool[program.Length];
    for (int i = 0; i < program.Length;)
    {
        if (visited[i])
        {
            infiniteLoop = true;
            return accumulator;
        }
        Instruction instruction = program[i];
        visited[i] = true;
        switch (program[i].Operation[0])
        {
            case 'n': // nop
                i++;
                break;
            case 'a': // jmp
                accumulator += instruction.Argument;
                i++;
                break;
            case 'j':
                i += instruction.Argument;
                break;
        }
    }
    return accumulator;
}

void Swap(Instruction instruction)
{
    if (instruction.Operation == "nop")
    {
        instruction.Operation = "jmp";
    }
    else if (instruction.Operation == "jmp")
    {
        instruction.Operation = "nop";
    }
}

long Part2(Instruction[] program)
{
    for (int i = 0; i < program.Length; i++)
    {
        Swap(program[i]);
        long accumulator = RunProgram(program, out bool infiniteLoop);
        Swap(program[i]);
        if (!infiniteLoop)
        {
            return accumulator;
        }
    }
    return long.MinValue;
}

string[] lines = File.ReadAllLines("/Users/andrewvardeman/Desktop/AdventOfCodeInput/08.txt");

Instruction[] program = lines.Where(l => !string.IsNullOrWhiteSpace(l.Trim()))
    .Select(l => new Instruction(l)).ToArray();

Console.WriteLine($"Part 1: {RunProgram(program, out _)}");
Console.WriteLine($"Part 2: {Part2(program)}");

class Instruction
{
    public string Operation;
    public int Argument;

    public Instruction(string spec)
    {
        Operation = spec.Substring(0, 3);
        Argument = int.Parse(spec.Substring(5));
        if (spec[4] == '-')
        {
            Argument = -Argument;
        }
    }
}