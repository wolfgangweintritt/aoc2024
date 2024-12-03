string filePath = "input.txt";
string[] lines = File.ReadAllLines(filePath);

IEnumerable<IEnumerable<int>> levelLines = lines.Select(
    line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(level => int.Parse(level))
);

IEnumerable<IEnumerable<int>> diffsLines = levelLines.Select(levels =>
{
    var enumerable = levels.ToList();
    return enumerable.Skip(1).Zip(enumerable, (current, previous) => current - previous);
});

Func<IEnumerable<int>, bool> allNegative = l => l.Aggregate(true, (current, i) => current && i < 0);
Func<IEnumerable<int>, bool> allPositive = l => l.Aggregate(true, (current, i) => current && i > 0);
Func<IEnumerable<int>, bool> allAbsLteThree = l => l.Aggregate(true, (current, i) => current && Math.Abs(i) <= 3);

IEnumerable<bool> safeReports = diffsLines.Select(levels =>
{
    var enumerable = levels.ToList();
    return (allNegative(enumerable) || allPositive(enumerable)) && allAbsLteThree(enumerable);
});

Console.WriteLine(safeReports.Aggregate(0, (carry, safe) => carry + (safe ? 1 : 0)));