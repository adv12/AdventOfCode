Tracker tracker = new Tracker(2);
tracker.Run(args[0]);
Console.WriteLine(tracker.TailLocationCount);

tracker = new Tracker(10);
tracker.Run(args[0]);
//tracker.Print();
Console.WriteLine(tracker.TailLocationCount);

public enum Axis
{
    Horizontal,
    Vertical
}
public class Tracker
{
    private int[] m_KnotXValues;
    private int[] m_KnotYValues;
    private HashSet<(int, int)> m_TailLocations = new HashSet<(int, int)>();
    public int TailLocationCount => m_TailLocations.Count;
    public Tracker(int numKnots)
    {
        m_KnotXValues = new int[numKnots];
        m_KnotYValues = new int[numKnots];
        m_TailLocations.Add((0, 0));
    }
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        foreach (string line in lines)
        {
            char direction = line[0];
            int distance = int.Parse(line.Substring(2));
            Axis axis = Axis.Horizontal;
            if (direction == 'U' || direction == 'D')
            {
                axis = Axis.Vertical;
            }
            int sign = 1;
            if (direction == 'D' || direction == 'L')
            {
                sign = -1;
            }
            MoveHead(axis, sign, distance);
        }
    }
    public void MoveHead(Axis axis, int sign, int distance)
    {
        for (int i = 0; i < distance; i++)
        {
            switch(axis)
            {
                case Axis.Horizontal:
                    m_KnotXValues[0] += sign;
                    break;
                case Axis.Vertical:
                    m_KnotYValues[0] += sign;
                    break;
            }
            MoveOtherKnots();
        }
    }
    public void MoveOtherKnots()
    {
        for (int i = 1; i < m_KnotXValues.Length; i++)
        {
            int xDiff = m_KnotXValues[i - 1] - m_KnotXValues[i];
            int yDiff = m_KnotYValues[i - 1] - m_KnotYValues[i];
            if (Math.Abs(xDiff) > 1 || Math.Abs(yDiff) > 1)
            {
                m_KnotXValues[i] += Math.Sign(xDiff);
                m_KnotYValues[i] += Math.Sign(yDiff);
            }
        }
        m_TailLocations.Add((m_KnotXValues.Last(), m_KnotYValues.Last()));
    }
    public void Print()
    {
        int xmax = int.MinValue;
        int xmin = int.MaxValue;
        int ymax = int.MinValue;
        int ymin = int.MaxValue;
        foreach (var loc in m_TailLocations)
        {
            xmax = Math.Max(loc.Item1, xmax);
            xmin = Math.Min(loc.Item1, xmin);
            ymax = Math.Max(loc.Item2, ymax);
            ymin = Math.Min(loc.Item2, ymin);
        }
        for (int y = ymax; y >= ymin; y--)
        {
            for (int x = xmin; x <= xmax; x++)
            {
                Console.Write(m_TailLocations.Contains((x, y)) ? "#" : ".");
            }
            Console.WriteLine();
        }
    }
}