using System;
using System.IO;
using System.Linq;

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/12.txt";
string[] lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l))
    .ToArray();

Ferry ferry1 = new Ferry1();
Ferry ferry2 = new Ferry2();
foreach (string line in lines)
{
    ferry1.ProcessInstruction(line);
    ferry2.ProcessInstruction(line);
}
Console.WriteLine($"Part 1: {ferry1.ManhattanDistance}");
Console.WriteLine($"Part 2: {ferry2.ManhattanDistance}");

public abstract class Ferry
{
    public int X = 0;
    public int Y = 0;
    public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);

    protected abstract void ProcessInstruction(char operation, int arg);

    public void ProcessInstruction(string instruction)
    {
        char operation = instruction[0];
        int arg = int.Parse(instruction.Substring(1));
        ProcessInstruction(operation, arg);
    }
}

public class Ferry1 : Ferry
{
    public int Direction = 0;

    private void Turn(int degrees)
    {
        Direction = (Direction + degrees) % 360;
        if (Direction < 0)
        {
            Direction += 360;
        }
    }

    private void Forward(int units)
    {
        if (Direction == 0)
        {
            X += units;
        }
        else if (Direction == 90)
        {
            Y += units;
        }
        else if (Direction == 180)
        {
            X -= units;
        }
        else if (Direction == 270)
        {
            Y -= units;
        }
        else
        {
            Console.WriteLine("Non-multiple of 90 degrees");
        }
    }

    protected override void ProcessInstruction(char operation, int arg)
    {
        if (operation == 'L')
        {
            Turn(arg);
        }
        else if (operation == 'R')
        {
            Turn(-arg);
        }
        else if (operation == 'F')
        {
            Forward(arg);
        }
        else if (operation == 'N')
        {
            Y += arg;
        }
        else if (operation == 'S')
        {
            Y -= arg;
        }
        else if (operation == 'W')
        {
            X -= arg;
        }
        else if (operation == 'E')
        {
            X += arg;
        }
    }
}

public class Ferry2 : Ferry
{
    public int WaypointX = 10;
    public int WaypointY = 1;

    public void Turn(int degrees)
    {
        degrees = degrees % 360;
        if (degrees < 0)
        {
            degrees += 360;
        }
        if (degrees == 90)
        {
            Turn(1, 0);
        }
        else if (degrees == 180)
        {
            Turn(0, -1);
        }
        else if (degrees == 270)
        {
            Turn(-1, 0);
        }
    }

    public void Turn(int sin, int cos)
    {
        int x = WaypointX;
        int y = WaypointY;
        WaypointX = x * cos - y * sin;
        WaypointY = x * sin + y * cos;
    }

    public void Forward(int units)
    {
        X += WaypointX * units;
        Y += WaypointY * units;
    }

    protected override void ProcessInstruction(char operation, int arg)
    {
        if (operation == 'L')
        {
            Turn(arg);
        }
        else if (operation == 'R')
        {
            Turn(-arg);
        }
        else if (operation == 'F')
        {
            Forward(arg);
        }
        else if (operation == 'N')
        {
            WaypointY += arg;
        }
        else if (operation == 'S')
        {
            WaypointY -= arg;
        }
        else if (operation == 'W')
        {
            WaypointX -= arg;
        }
        else if (operation == 'E')
        {
            WaypointX += arg;
        }
    }
}