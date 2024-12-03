string filePath = "input.txt";
string[] lines = File.ReadAllLines(filePath);

int[][] levelLines = lines.Select(
    line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(level => int.Parse(level)).ToArray()
).ToArray();

int[][] diffsLines = levelLines.Select(
    levels => levels.Skip(1).Zip(levels, (current, previous) => current - previous).ToArray()
).ToArray();

Func<int[], bool> allNegative = l => l.Aggregate(true, (current, i) => current && i < 0);
Func<int[], bool> allPositive = l => l.Aggregate(true, (current, i) => current && i > 0);
Func<int[], bool> allAbsLteThree = l => l.Aggregate(true, (current, i) => current && Math.Abs(i) <= 3);
Func<int[], bool> isValidDiffLine = l => (allNegative(l) || allPositive(l)) && allAbsLteThree(l);

int[][] safeReports = diffsLines.Where(
    diffs => isValidDiffLine(diffs)
).ToArray();
Console.WriteLine(safeReports.Count());

// part2
int[][] unsafeReports = diffsLines.Where(
    diffs => !isValidDiffLine(diffs)
).ToArray();

int[][] unsafeReportsWithOneError = unsafeReports.Where(line =>
{
    int[][] permutations = line
        .Select((_, index) => line
            .Select((n, i) => i == index - 1 ? line[i] + line[index] : n) // if previous index => add the number we're gonna remove
            .Where((_, i) => i != index) // filter out current idx
            .ToArray()
        )
        .Append(line.Take(line.Length - 1).ToArray()) // remove the last level = remove the last diff
        .ToArray();

    return permutations.Any(diffs => isValidDiffLine(diffs));
}).ToArray();

Console.WriteLine(unsafeReportsWithOneError.Count() + safeReports.Count());