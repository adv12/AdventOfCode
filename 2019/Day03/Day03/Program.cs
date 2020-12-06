using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

(HashSet<Point>, Dictionary<Point, int>) GetCoveredPointsAndStepCounts(string wire)
{
    HashSet<Point> points = new HashSet<Point>();
    Dictionary<Point, int> stepCounts = new Dictionary<Point, int>();
    string[] instructions = wire.Trim().Split(',');
    int x = 0, y = 0;
    int steps = 0;
    foreach (string instruction in instructions)
    {
        int diff = int.Parse(instruction.Substring(1));
        char dir = instruction[0];
        int increment = 1;
        if (dir == 'D' || dir == 'L')
        {
            increment = -1;
        }
        for (int i = 0; i < diff; i++)
        {
            if (dir == 'L' || dir == 'R')
            {
                x += increment;
            }
            else
            {
                y += increment;
            }
            Point p = new Point(x, y);
            points.Add(p);
            steps++;
            if (!stepCounts.ContainsKey(p))
            {
                stepCounts[p] = steps;
            }
        }
    }
    return (points, stepCounts);
}


string[] wires = File.ReadAllLines("/Users/andrewvardeman/Desktop/AOC2019Input/03.txt");
(var intersections, var stepCounts1) = GetCoveredPointsAndStepCounts(wires[0]);
(var points2, var stepCounts2) = GetCoveredPointsAndStepCounts(wires[1]);
intersections.IntersectWith(points2);
Console.WriteLine($"Part 1: {intersections.Select(p => p.ManhattanDistance).OrderBy(d => d).First()}");
Console.WriteLine($"Part 2: {intersections.Select(p => stepCounts1[p] + stepCounts2[p]).OrderBy(c => c).First()}");

struct Point
{
    public int X;
    public int Y;
    public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

