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
        Dictionary<int, HashSet<int>> rulesAfter = rules // key = must be before, set = values that must be after
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

    private static Func<List<int>, List<int>> CorrectUpdate(Dictionary<int, HashSet<int>> rulesAfter)
    {
        return update =>
        {
            int i = 0;
            while (i < update.Count)
            {
                int newIndex = i + 1;
                for (int b = 0; b < i; b++)
                {
                    if (rulesAfter.ContainsKey(update[i]) && rulesAfter[update[i]].Contains(update[b]))
                    {
                        newIndex = b;
                        break;
                    }
                }

                if (newIndex != i + 1)
                {
                    update.Insert(newIndex, update[i]);
                    update.RemoveAt(i + 1);
                }

                i = newIndex;
            }

            return update;
        };
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