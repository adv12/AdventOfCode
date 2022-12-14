var specs = File.ReadAllLines(args[0]).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
var assignmentPairs = specs.Select(s => new AssignmentPair(s));
Console.WriteLine(assignmentPairs.Count(ap => ap.HasContainedAssignment));
Console.WriteLine(assignmentPairs.Count(ap => ap.AssignmentsOverlap));

public class AssignmentPair {

    public Assignment Assignment1 { get; }

    public Assignment Assignment2 { get; }

    public bool HasContainedAssignment => Assignment1.Contains(Assignment2) || Assignment2.Contains(Assignment1);

    public bool AssignmentsOverlap => Assignment1.Overlaps(Assignment2);

    public AssignmentPair (string spec)
    {
        var pairSpecs = spec.Split(',');
        Assignment1 = new Assignment(pairSpecs[0]);
        Assignment2 = new Assignment(pairSpecs[1]);
    }
}

public class Assignment {

    public int First { get; }

    public int Last { get; }

    public Assignment(string spec)
    {
        var extremes = spec.Split('-');
        First = int.Parse(extremes[0]);
        Last = int.Parse(extremes[1]);
    }

    public bool Contains(Assignment that)
    {
        return this.First <= that.First && this.Last >= that.Last;
    }

    public bool Overlaps(Assignment that)
    {
        return (this.First <= that.First && this.Last >= that.First) ||
                (this.First <= that.Last && this.Last >= that.Last) ||
                (that.First <= this.First && that.Last >= this.First) ||
                (that.First <= this.Last && that.Last >= this.Last);
    }
}