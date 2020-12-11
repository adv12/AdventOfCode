using System;
using System.IO;
using System.Linq;

int GetAdjacentOccupiedCount(char[][] seats, int row, int col)
{
    int count = 0;
    for (int r = Math.Max(row - 1, 0); r < Math.Min(row + 2, seats.Length); r++)
    {
        for (int c = Math.Max(col - 1, 0); c < Math.Min(col + 2, seats[r].Length); c++)
        {
            if ((r != row || c != col) && seats[r][c] == '#')
            {
                count++;
            }
        }
    }
    return count;
}

int GetVisibleOccupiedCount(char[][] seats, int row, int col)
{
    int count = 0;
    for (int dr = -1; dr < 2; dr++)
    {
        for (int dc = -1; dc < 2; dc++)
        {
            if ((dr != 0 || dc != 0) && CanSeeOccupiedSeat(seats, row, col, dr, dc))
            {
                count++;
            }
        }
    }
    return count;
}

bool CanSeeOccupiedSeat(char[][] seats, int row, int col, int dr, int dc)
{
    for (int r = row + dr, c = col + dc;
        r >= 0 && r < seats.Length && c >= 0 && c < seats[0].Length;
        r += dr, c += dc)
    {
        if (seats[r][c] == '#')
        {
            return true;
        }
        if (seats[r][c] == 'L')
        {
            return false;
        }
    }
    return false;
}

char[][] IterateUntilNoChange(char[][] seats, bool countVisible, int adjacentFullThreshhold)
{
    char[][] newSeats = null;
    bool changed = false;
    do
    {
        changed = false;
        newSeats = new char[seats.Length][];
        for (int r = 0; r < seats.Length; r++)
        {
            newSeats[r] = new char[seats[r].Length];
            for (int c = 0; c < seats[r].Length; c++)
            {
                char seat = seats[r][c];
                char newSeat = seat;
                int count = countVisible ? GetVisibleOccupiedCount(seats, r, c) :
                    GetAdjacentOccupiedCount(seats, r, c);
                if (seat == 'L' && count == 0)
                {
                    changed = true;
                    newSeat = '#';
                }
                else if (seat == '#' && count >= adjacentFullThreshhold)
                {
                    changed = true;
                    newSeat = 'L';
                }
                newSeats[r][c] = newSeat;
            }
        }
        seats = newSeats;
    } while (changed);

    return newSeats;
}

string path = "/Users/andrewvardeman/Desktop/AdventOfCodeInput/11.txt";
char[][] seats = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l))
    .Select(l => l.Trim().ToArray()).ToArray();

char[][] seats1 = IterateUntilNoChange(seats, false, 4);
char[][] seats2 = IterateUntilNoChange(seats, true, 5);

int seatCount1 = seats1.SelectMany(l => l).Count(c => c == '#');
int seatCount2 = seats2.SelectMany(l => l).Count(c => c == '#');
Console.WriteLine($"Part 1: {seatCount1}");
Console.WriteLine($"Part 2: {seatCount2}");