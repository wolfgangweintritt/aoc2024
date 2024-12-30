public class Solver
{
    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);

        // lines =
        // [
        //     "....#.....",
        //     ".........#",
        //     "..........",
        //     "..#.......",
        //     ".......#..",
        //     "..........",
        //     ".#..^.....",
        //     "........#.",
        //     "#.........",
        //     "......#..."
        // ];
        
        Dictionary<char, (int, int)> directions = new Dictionary<char, (int, int)>
        {
            { '^', (-1, 0) },
            { '>', (0, 1) },
            { 'v', (1, 0) },
            { '<', (0, -1) },
        };
        char[] directionOrder = directions.Keys.ToArray();
        (int, int) FindStartPosition()
        {
            for (int r = 0; r < lines.Length; r++)
            {
                for (int c = 0; c < lines[0].Length; c++)
                {
                    if (directions.ContainsKey(lines[r][c]))
                    {
                        return (r, c);
                    }
                }
            }

            throw new Exception("Invalid input, no starting position found");
        }

        (int, int) currentPos = FindStartPosition();
        char currentDirection = lines[currentPos.Item1][currentPos.Item2];
        
        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        Func<(int, int), bool> inBounds = x => x.Item1 >= 0 && x.Item1 < lines.Length && x.Item2 >= 0 && x.Item2 < lines[0].Length;

        while (inBounds(currentPos))
        {
            visited.Add(currentPos);
            (int, int) nextPos = (currentPos.Item1 + directions[currentDirection].Item1, currentPos.Item2 + directions[currentDirection].Item2);

            if (inBounds(nextPos) && lines[nextPos.Item1][nextPos.Item2] == '#')
            {
                int nextDirectionIndex = Array.IndexOf(directionOrder, currentDirection) < directionOrder.Length - 1
                    ? Array.IndexOf(directionOrder, currentDirection) + 1
                    : 0;
                currentDirection = directionOrder[nextDirectionIndex];
                nextPos = (currentPos.Item1 + directions[currentDirection].Item1, currentPos.Item2 + directions[currentDirection].Item2);
            }

            currentPos = nextPos;
        }

        Console.WriteLine(visited.Count);
    }
}