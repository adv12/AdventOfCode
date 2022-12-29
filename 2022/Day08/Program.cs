var lines = File.ReadAllLines(args[0]).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

var numRows = lines.Length;
var numCols = lines[0].Length;
var heights = new byte[numRows, numCols];

var visible = new bool[numRows, numCols];
var visibleCount = 0;

for (var i = 0; i < numRows; i++)
{
    var highest = -1;
    for (var j = 0; j < numCols; j++)
    {

        heights[i, j] = byte.Parse(lines[i][j].ToString());
        if (heights[i, j] > highest)
        {
            highest = heights[i, j];
            visible[i, j] = true;
        }
    }
    highest = -1;
    for (var j = numCols - 1; j >= 0; j--)
    {
        if (heights[i, j] > highest)
        {
            highest = heights[i, j];
            visible[i, j] = true;
        }
    }
}

for (var j = 0; j < numCols; j++)
{
    var highest = -1;
    for (int i = 0; i < numRows; i++)
    {
        if (heights[i, j] > highest)
        {
            highest = heights[i, j];
            visible[i, j] = true;
        }
    }
    highest = -1;
    for (var i = numRows - 1; i >= 0; i--)
    {
        if (heights[i, j] > highest)
        {
            highest = heights[i, j];
            visible[i, j] = true;
        }
        if (visible[i, j])
        {
            visibleCount++;
        }
    }
}

Console.WriteLine(visibleCount);

var highestScore = 0;
for (var i = 1; i < numRows - 1; i++)
{
    for (var j = 1; j < numCols - 1; j++)
    {
        int height = heights[i, j];
        var leftCount = 0;
        var rightCount = 0;
        var topCount = 0;
        var bottomCount = 0;

        for (var j2 = j - 1; j2 >= 0; j2--)
        {
            leftCount++;
            if (heights[i, j2] >= height)
            {
                break;
            }
        }

        for (var j2 = j + 1; j2 < numCols; j2++)
        {
            rightCount++;
            if (heights[i, j2] >= height)
            {
                break;
            }
        }

        for (var i2 = i - 1; i2 >= 0; i2--)
        {
            topCount++;
            if (heights[i2, j] >= height)
            {
                break;
            }
        }

        for (var i2 = i + 1; i2 < numRows; i2++)
        {
            bottomCount++;
            if (heights[i2, j] >= height)
            {
                break;
            }
        }

        var score = leftCount * rightCount * topCount * bottomCount;
        highestScore = Math.Max(score, highestScore);
    }
}

Console.WriteLine(highestScore);