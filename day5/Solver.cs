namespace day5;

public class Solver
{
    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);

        int[][] rules = lines
            .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .Select(
                line => line
                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(level => int.Parse(level))
                    .ToArray()
            ).ToArray();
        Dictionary<int, HashSet<int>> rulesAfter = rules // key = value must be before, set = values that must be after
            .GroupBy(rule => rule[0])
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x[1]).ToHashSet()
            );

        int[][] updates = lines
            .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
            .Skip(1)
            .Select(
                line => line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(level => int.Parse(level)).ToArray()
            ).ToArray();
        int[][] validUpdates = updates
            .Where(IsUpdateValid(rulesAfter))
            .ToArray();

        Console.WriteLine(validUpdates
            .Select(a => a[a.Length / 2])
            .Sum()
        );
    }

    private static Func<int[], bool> IsUpdateValid(Dictionary<int, HashSet<int>> rulesAfter)
    {
        return update => update
            .Select((page, index) => new { Page = page, Index = index })
            .All(v => update
                .Take(v.Index)
                .All(b => !rulesAfter[v.Page].Contains(b))
            );
    }
}