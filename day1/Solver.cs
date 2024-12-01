string filePath = "input.txt";
string[] lines = File.ReadAllLines(filePath);

Func<string, int, int> selectIdxFromLine =
    (string line, int x) => int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[x]);
int[] leftColumn = lines.Select(line => selectIdxFromLine(line, 0)).ToArray();
int[] rightColumn = lines.Select(line => selectIdxFromLine(line, 1)).ToArray();

// Part1
int[] leftColumnSorted = leftColumn.OrderBy(x => x).ToArray();
int[] rightColumnSorted = rightColumn.OrderBy(x => x).ToArray();

int diffSum = leftColumnSorted.Zip(rightColumnSorted, (x, y) => Math.Abs(x - y)).Sum();
Console.WriteLine(diffSum);

// Part2
int similarity = leftColumn.Select(l => l * rightColumn.Count(r => r == l)).Sum();
Console.WriteLine(similarity);