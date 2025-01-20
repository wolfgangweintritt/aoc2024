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

        lines =
        [
            "..#.....",
            ".......#",
            "........",
            ".#......",
            "#...#.O.",
            "#O......",
            "..^O..#.",
        ];
        
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
        
        Func<(int, int), bool> inBounds = x => x.Item1 >= 0 && x.Item1 < lines.Length && x.Item2 >= 0 && x.Item2 < lines[0].Length;
        Func<(int, int), char, (int, int)> calculateNextPosition = (cPos, cDir) => (cPos.Item1 + directions[cDir].Item1, cPos.Item2 + directions[cDir].Item2);

        Console.WriteLine(Part1CountUniquePositions(inBounds, calculateNextPosition, currentPos, directions, currentDirection, lines, directionOrder));
        
        Console.WriteLine(Part2CountLoopObstaclePositions(inBounds, calculateNextPosition, currentPos, directions, currentDirection, lines, directionOrder));
    }

    private static int Part2CountLoopObstaclePositions(Func<(int, int), bool> inBounds, Func<(int, int), char, (int, int)> calculateNextPosition, (int, int) currentPos, Dictionary<char, (int, int)> directions, char currentDirection, string[] lines, char[] directionOrder)
    {
        HashSet<(int, int)> obstaclePositions = new HashSet<(int, int)>();
        Dictionary<(int, int), IList<char>> visited = new Dictionary<(int, int), IList<char>>();
        Func<(int, int), char, bool> canAddObstacle = (cPos, cDir) =>
        {
            char nextDirection = GetNextDirection(cDir, directionOrder);
            while (inBounds(cPos))
            {
                if (lines[cPos.Item1][cPos.Item2] == '#')
                {
                    return false;
                }

                if (visited.ContainsKey(cPos) && visited[cPos].Contains(nextDirection))
                {
                    return true;
                }
                cPos = calculateNextPosition(cPos, nextDirection);
            } 

            return false;
        };

        while (inBounds(currentPos))
        {
            if (visited.ContainsKey(currentPos) == false)
            {
                visited[currentPos] = new List<char>();
            }
            visited[currentPos].Add(currentDirection);
            
            (int, int) nextPos = calculateNextPosition(currentPos, currentDirection);
            if (canAddObstacle(currentPos, currentDirection))
            {
                obstaclePositions.Add(nextPos);
            }

            if (inBounds(nextPos) && lines[nextPos.Item1][nextPos.Item2] == '#')
            {
                currentDirection = GetNextDirection(currentDirection, directionOrder);
                nextPos = calculateNextPosition(currentPos, currentDirection);
            }

            currentPos = nextPos;
        }

        return obstaclePositions.Count;
    }

    private static char GetNextDirection(char currentDirection, char[] directionOrder)
    {
        int nextDirectionIndex = Array.IndexOf(directionOrder, currentDirection) < directionOrder.Length - 1
            ? Array.IndexOf(directionOrder, currentDirection) + 1
            : 0;
        return directionOrder[nextDirectionIndex];
    }

    private static int Part1CountUniquePositions(Func<(int, int), bool> inBounds, Func<(int, int), char, (int, int)> calculateNextPosition, (int, int) currentPos, Dictionary<char, (int, int)> directions, char currentDirection, string[] lines, char[] directionOrder)
    {
        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        while (inBounds(currentPos))
        {
            visited.Add(currentPos);
            (int, int) nextPos = calculateNextPosition(currentPos, currentDirection);

            if (inBounds(nextPos) && lines[nextPos.Item1][nextPos.Item2] == '#')
            {
                currentDirection = GetNextDirection(currentDirection, directionOrder);
                nextPos = calculateNextPosition(currentPos, currentDirection);
            }

            currentPos = nextPos;
        }

        return visited.Count;
    }
}