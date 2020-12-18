using System;
using System.IO;
using System.Linq;

(int, int, int) GetDimensions3(bool[,,] array)
{
    return (array.GetLength(0), array.GetLength(1), array.GetLength(2));
}

(int, int, int, int) GetDimensions4(bool[,,,] array)
{
    return (array.GetLength(0), array.GetLength(1), array.GetLength(2), array.GetLength(3));
}

bool[,,] Expand3(bool[,,] src)
{
    var numx = src.GetLength(0) + 2;
    var numy = src.GetLength(1) + 2;
    var numz = src.GetLength(2) + 2;
    var dest = new bool[numx, numy, numz];
    for (int x = 1; x < numx - 1; x++)
    {
        for (int y = 1; y < numy - 1; y++)
        {
            for (int z = 1; z < numz - 1; z++)
            {
                dest[x, y, z] = src[x - 1, y - 1, z - 1];
            }
        }
    }
    return dest;
}

bool[,,,] Expand4(bool[,,,] src)
{
    var numx = src.GetLength(0) + 2;
    var numy = src.GetLength(1) + 2;
    var numz = src.GetLength(2) + 2;
    var numw = src.GetLength(3) + 2;
    var dest = new bool[numx, numy, numz, numw];
    for (int x = 1; x < numx - 1; x++)
    {
        for (int y = 1; y < numy - 1; y++)
        {
            for (int z = 1; z < numz - 1; z++)
            {
                for (int w = 1; w < numw - 1; w++)
                {
                    dest[x, y, z, w] = src[x - 1, y - 1, z - 1, w - 1];
                }
            }
        }
    }
    return dest;
}

bool[,,] CloneEmpty3(bool[,,] src)
{
    return new bool[src.GetLength(0), src.GetLength(1), src.GetLength(2)];
}

bool[,,,] CloneEmpty4(bool[,,,] src)
{
    return new bool[src.GetLength(0), src.GetLength(1), src.GetLength(2), src.GetLength(3)];
}

int GetActiveNeighborCount3(bool[,,] src, int ix, int iy, int iz)
{
    (int numx, int numy, int numz) = GetDimensions3(src);
    int count = 0;
    for (int x = Math.Max(ix - 1, 0); x < Math.Min(ix + 2, numx); x++)
    {
        for (int y = Math.Max(iy - 1, 0); y < Math.Min(iy + 2, numx); y++)
        {
            for (int z = Math.Max(iz - 1, 0); z < Math.Min(iz + 2, numz); z++)
            {
                if (!(ix == x && iy == y && iz == z) && src[x, y, z])
                {
                    count++;
                }
            }
        }
    }
    return count;
}

int GetActiveNeighborCount4(bool[,,,] src, int ix, int iy, int iz, int iw)
{
    (int numx, int numy, int numz, int numw) = GetDimensions4(src);
    int count = 0;
    for (int x = Math.Max(ix - 1, 0); x < Math.Min(ix + 2, numx); x++)
    {
        for (int y = Math.Max(iy - 1, 0); y < Math.Min(iy + 2, numx); y++)
        {
            for (int z = Math.Max(iz - 1, 0); z < Math.Min(iz + 2, numz); z++)
            {
                for (int w = Math.Max(iw - 1, 0); w < Math.Min(iw + 2, numw); w++)
                {
                    if (!(ix == x && iy == y && iz == z && iw == w) && src[x, y, z, w])
                    {
                        count++;
                    }
                }
            }
        }
    }
    return count;
}

bool[,,] Iterate3(bool[,,] src)
{
    src = Expand3(src);
    var dest = CloneEmpty3(src);
    (int numx, int numy, int numz) = GetDimensions3(src);
    for (int x = 0; x < numx; x++)
    {
        for (int y = 0; y < numy; y++)
        {
            for (int z = 0; z < numz; z++)
            {
                int activeNeighbors = GetActiveNeighborCount3(src, x, y, z);
                if (src[x, y, z])
                {
                    dest[x, y, z] = activeNeighbors == 2 || activeNeighbors == 3;
                }
                else
                {
                    dest[x, y, z] = activeNeighbors == 3;
                }
            }
        }
    }
    return dest;
}

bool[,,,] Iterate4(bool[,,,] src)
{
    src = Expand4(src);
    var dest = CloneEmpty4(src);
    (int numx, int numy, int numz, int numw) = GetDimensions4(src);
    for (int x = 0; x < numx; x++)
    {
        for (int y = 0; y < numy; y++)
        {
            for (int z = 0; z < numz; z++)
            {
                for (int w = 0; w < numw; w++)
                {

                    int activeNeighbors = GetActiveNeighborCount4(src, x, y, z, w);
                    if (src[x, y, z, w])
                    {
                        dest[x, y, z, w] = activeNeighbors == 2 || activeNeighbors == 3;
                    }
                    else
                    {
                        dest[x, y, z, w] = activeNeighbors == 3;
                    }
                }
            }
        }
    }
    return dest;
}

(bool[,,], bool[,,,]) Parse(string[] lines)
{
    bool[,,] grid3 = new bool[lines[0].Length, lines.Length, 1];
    bool[,,,] grid4 = new bool[lines[0].Length, lines.Length, 1, 1];
    for (int y = 0; y < lines.Length; y++)
    {
        string line = lines[y];
        for (int x = 0; x < line.Length; x++)
        {
            bool active = line[x] == '#';
            grid3[x, y, 0] = active;
            grid4[x, y, 0, 0] = active;
        }
    }
    return (grid3, grid4);
}

int CountActive3(bool[,,] grid)
{
    (int numx, int numy, int numz) = GetDimensions3(grid);
    int count = 0;
    for (int x = 0; x < numx; x++)
    {
        for (int y = 0; y < numy; y++)
        {
            for (int z = 0; z < numz; z++)
            {
                if (grid[x, y, z])
                {
                    count++;
                }
            }
        }
    }
    return count;
}

int CountActive4(bool[,,,] grid)
{
    (int numx, int numy, int numz, int numw) = GetDimensions4(grid);
    int count = 0;
    for (int x = 0; x < numx; x++)
    {
        for (int y = 0; y < numy; y++)
        {
            for (int z = 0; z < numz; z++)
            {
                for (int w = 0; w < numw; w++)
                {
                    if (grid[x, y, z, w])
                    {
                        count++;
                    }
                }
            }
        }
    }
    return count;
}

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/17.txt";
string[] lines = File.ReadAllLines(path).Select(l => l.Trim())
    .Where(l => l.Length > 0).ToArray();

(bool[,,] grid3, bool[,,,] grid4) = Parse(lines);

DateTime start = DateTime.Now;

for (int i = 0; i < 6; i++)
{
    grid3 = Iterate3(grid3);
    grid4 = Iterate4(grid4);
}

TimeSpan time = DateTime.Now - start;

Console.WriteLine($"Part 1: {CountActive3(grid3)}");
Console.WriteLine($"Part 2: {CountActive4(grid4)}");
Console.WriteLine($"Time: {time.TotalMilliseconds}ms");
