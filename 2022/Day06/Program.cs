var text = args[0];
var window1 = new Window<char>(4);
var window2 = new Window<char>(14);
for (var i = 0; i < text.Length; i++)
{
    if (window1.AddAndTest(text[i]))
    {
        break;
    }
}

for (var i = 0; i < text.Length; i++)
{
    if (window2.AddAndTest(text[i]))
    {
        break;
    }
}

Console.WriteLine(window1.Count);
Console.WriteLine(window2.Count);

public class Window<T>
{
    private T[] m_Buffer;
    private int m_Index;
    public int Count { get; private set; }
    public bool Full => Count >= m_Buffer.Length;

    public bool ValuesUnique {
        get
        {
            for (var i = 0; i < m_Buffer.Length; i++)
            {
                for (var j = i + 1; j < m_Buffer.Length; j++)
                {
                    if (Object.Equals(m_Buffer[i], m_Buffer[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public Window(int size)
    {
        m_Buffer = new T[size];
    }

    public void Add(T item)
    {
        if (m_Index == m_Buffer.Length)
        {
            m_Index = 0;
        }
        m_Buffer[m_Index++] = item;
        Count++;
    }

    public bool AddAndTest(T item)
    {
        Add(item);
        return Full && ValuesUnique;
    }

}
