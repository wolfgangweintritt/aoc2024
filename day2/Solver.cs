string filePath = "input.txt";
string[] lines = File.ReadAllLines(filePath);

int[][] levelLines = lines.Select(
    line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(level => int.Parse(level)).ToArray()
).ToArray();

Func<int[], int[]> toDiff = levels => levels.Skip(1).Zip(levels, (current, previous) => current - previous).ToArray();
Func<int[], bool> allDiffsNegative = l => toDiff(l).Aggregate(true, (current, i) => current && i < 0);
Func<int[], bool> allDiffsPositive = l => toDiff(l).Aggregate(true, (current, i) => current && i > 0);
Func<int[], bool> allDiffsAbsLteThree = l => toDiff(l).Aggregate(true, (current, i) => current && Math.Abs(i) <= 3);
Func<int[], bool> isValidLevelLine = l => (allDiffsNegative(l) || allDiffsPositive(l)) && allDiffsAbsLteThree(l);

int[][] safeReports = levelLines
    .Where(line => isValidLevelLine(line))
    .ToArray();
Console.WriteLine(safeReports.Length);

// part2
int[][] unsafeReports = levelLines
    .Where(line => !isValidLevelLine(line)).ToArray();

int[][] unsafeReportsWithOneError = unsafeReports
    .Where(line => line
        .Select((_, index) => line
            .Where((_, i) => i != index) // filter out current idx
            .ToArray()
        )
        .Any(l => isValidLevelLine(l))
    ).ToArray();

Console.WriteLine(unsafeReportsWithOneError.Length + safeReports.Length);