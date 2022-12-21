var lines = File.ReadAllLines(args[0]).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
Parser parser = new Parser();
parser.Parse(lines);
var dirs = new HashSet<DirItem>();
parser.Root.FindDirectoriesNoLargerThan(100000, dirs);
Console.WriteLine(dirs.Sum(d => d.Size));
var spaceToClear = 30000000 - (70000000 - parser.Root.Size);
parser.Root.FindDirectoriesNoLargerThan(parser.Root.Size, dirs);
Console.WriteLine(dirs.Select(d => d.Size).Where(s => s >= spaceToClear).Min());

public class Parser
{
    public DirItem Root { get; } = new DirItem("", null);
    public void Parse(IEnumerable<string> lines)
    {
        DirItem? m_Dir = null;
        foreach (string line in lines)
        {
            if (line.StartsWith("$ cd "))
            {
                string dirname = line.Substring(5).Trim();
                if (dirname == "/")
                {
                    m_Dir = Root;
                }
                else if (dirname == "..")
                {
                    m_Dir = m_Dir?.Parent;
                }
                else {
                    m_Dir = m_Dir?.GetChildDir(dirname);
                }
            }
            else if (line.StartsWith("dir "))
            {
                string dirname = line.Substring(4).Trim();
                m_Dir?.GetChildDir(dirname);
            }
            else if (!"$ ls".Equals(line))
            {
                string[] parts = line.Split(' ');
                m_Dir?.AddFile(parts[1], int.Parse(parts[0]));
            }
        }
    }
}
public class DirItem : Item
{
    private int? m_Size = null;
    private Dictionary<string, DirItem> m_Dirs = new Dictionary<string, DirItem>();
    private Dictionary<string, FileItem> m_Files = new Dictionary<string, FileItem>();
    public IReadOnlyDictionary<string, DirItem> Dirs => m_Dirs;
    public IReadOnlyDictionary<string, FileItem> Files => m_Files;
    public override int Size
    {
        get
        {
            if (m_Size.HasValue)
            {
                return m_Size.Value;
            }
            m_Size = Files.Values.Sum(f => f.Size) + Dirs.Values.Sum(d => d.Size);
            return m_Size.Value;
        }
    }
    public DirItem(string name, DirItem? parent) : base(name, parent)
    {

    }
    public DirItem GetChildDir(string name)
    {
        if (!Dirs.ContainsKey(name))
        {
            m_Dirs[name] = new DirItem(name, this);
        }
        return Dirs[name];
    }
    public void AddFile(string name, int size)
    {
        m_Files[name] = new FileItem(name, this, size);
    }
    public void FindDirectoriesNoLargerThan(int maxSize, HashSet<DirItem> dirs)
    {
        if (Size <= maxSize)
        {
            dirs.Add(this);
        }
        foreach (DirItem dir in Dirs.Values)
        {
            dir.FindDirectoriesNoLargerThan(maxSize, dirs);
        }
    }
}

public class FileItem : Item
{
    private int m_Size;
    public override int Size => m_Size;
    public FileItem(string name, DirItem parent, int size) : base(name, parent)
    {
        m_Size = size;
    }
}

public abstract class Item
{
    public string Name { get; }
    public abstract int Size { get; }
    public DirItem? Parent { get; }
    public string Path => Parent == null ? "/" : System.IO.Path.Combine(Parent.Path, Name);
    public Item(string name, DirItem? parent)
    {
        Name = name;
        Parent = parent;
    }
}