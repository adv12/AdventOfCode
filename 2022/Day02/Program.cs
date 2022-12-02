List<Round> ParseInput(string path, bool part2)
{
    var rounds = new List<Round>();
    foreach (var line in File.ReadAllLines(path))
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            if (part2)
            {
                rounds.Add(new Round(CharToShape(line[0]), null, CharToOutcome(line[2])));
            }
            else
            {
                rounds.Add(new Round(CharToShape(line[0]), CharToShape(line[2]), null));
            }
        }
    }
    return rounds;
}

Shape CharToShape(char c)
{
    switch (c) 
    {
        case 'A':
        case 'X':
            return Shape.Rock;
        case 'B':
        case 'Y':
            return Shape.Paper;
        default:
            return Shape.Scissors;
    }
}

Outcome CharToOutcome(char c)
{
    switch (c)
    {
        case 'X':
            return Outcome.Loss;
        case 'Y':
            return Outcome.Draw;
        default:
            return Outcome.Win;
    }
}

int GetScore(Round round)
{
    return (int)round.MyShape + (int)round.MyOutcome;
}

int GetTotalScore(List<Round> rounds)
{
    return rounds.Sum(r => GetScore(r));
}

var rounds1 = ParseInput(args[0], false);
var rounds2 = ParseInput(args[0], true);
Console.WriteLine(GetTotalScore(rounds1));
Console.WriteLine(GetTotalScore(rounds2));

class Round {
    public Shape ElfShape { get; private set; }
    public Shape? MyShape { get; private set; }
    public Outcome? MyOutcome { get; private set; }
    public Round(Shape elfShape, Shape? myShape, Outcome? outcome)
    {
        ElfShape = elfShape;
        MyShape = myShape;
        MyOutcome = outcome;
        if (MyShape.HasValue && !MyOutcome.HasValue)
        {
            MyOutcome = GetMyOutcome();
        }
        else if (MyOutcome.HasValue && !MyShape.HasValue)
        {
            MyShape = GetMyShape();
        }
    }

    Outcome GetMyOutcome()
    {
        int diff = (int)MyShape! - (int)ElfShape;
        if (diff < 0) {
            diff += 3;
        }
        switch (diff)
        {
            case 0:
                return Outcome.Draw;
            case 1:
                return Outcome.Win;
            default:
                return Outcome.Loss;
        }
    }

    Shape GetMyShape()
    {
        switch (MyOutcome!) {
            case Outcome.Draw:
                return ElfShape;
            case Outcome.Win:
                int shapeVal = (int)ElfShape + 1;
                if (shapeVal > 3)
                {
                    shapeVal -= 3;
                }
                return (Shape)shapeVal;
            default:
                shapeVal = (int)ElfShape - 1;
                if (shapeVal < 1)
                {
                    shapeVal += 3;
                }
                return (Shape)shapeVal;
        }
    }
}

public enum Shape {
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

public enum Outcome {
    Loss = 0,
    Draw = 3,
    Win = 6
}

