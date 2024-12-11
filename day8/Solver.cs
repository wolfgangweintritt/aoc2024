namespace day8;

public class Solver
{
    private class Coordinate(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public bool IsValid((int, int) dimensions)
        {
            return X >= 0
                   && Y >= 0
                   && X < dimensions.Item1
                   && Y < dimensions.Item2;
        }

        public Diff GetDiff(Coordinate other)
        {
            return new Diff(X - other.X, Y - other.Y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Coordinate other)
            {
                return X == other.X && Y == other.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    private class Diff(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);

        // lines = new string[]
        // {
        //     "............",
        //     "........0...",
        //     ".....0......",
        //     ".......0....",
        //     "....0.......",
        //     "......A.....",
        //     "............",
        //     "............",
        //     "........A...",
        //     ".........A..",
        //     "............",
        //     "............",
        // };

        Dictionary<char, List<Coordinate>> antennas = new Dictionary<char, List<Coordinate>>();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '.')
                {
                    continue;
                }

                if (!antennas.ContainsKey(lines[y][x]))
                {
                    antennas.Add(lines[y][x], new List<Coordinate>());
                }

                antennas[lines[y][x]].Add(new Coordinate(x, y));
            }
        }

        (int, int) dimensions = (lines[0].Length, lines.Length);
        var antinodes = Part1(antennas, dimensions);
        Console.WriteLine(antinodes.Count);

        // part 2
        var antinodesPart2 = Part2(antennas, dimensions);
        Console.WriteLine(antinodesPart2.Count);

        foreach (Coordinate antinode in antinodesPart2)
        {
            Console.WriteLine(antinode.X + ", " + antinode.Y);
        }

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                Console.Write(antinodesPart2.Contains(new Coordinate(x, y)) ? '#' : lines[y][x]);
            }

            Console.Write("\n");
        }
    }

    private static HashSet<Coordinate> Part1(Dictionary<char, List<Coordinate>> antennas, (int, int) dimensions)
    {
        HashSet<Coordinate> antinodes = new HashSet<Coordinate>();
        foreach (KeyValuePair<char, List<Coordinate>> antenna in antennas)
        {
            for (int a = 0; a < antenna.Value.Count - 1; a++)
            {
                for (int b = a + 1; b < antenna.Value.Count; b++)
                {
                    Diff diff = antenna.Value[a].GetDiff(antenna.Value[b]);
                    Coordinate antinode1 = new Coordinate(antenna.Value[a].X + diff.X, antenna.Value[a].Y + diff.Y);
                    Coordinate antinode2 = new Coordinate(antenna.Value[b].X - diff.X, antenna.Value[b].Y - diff.Y);
                    if (antinode1.IsValid(dimensions))
                    {
                        antinodes.Add(antinode1);
                    }

                    if (antinode2.IsValid(dimensions))
                    {
                        antinodes.Add(antinode2);
                    }
                }
            }
        }

        return antinodes;
    }

    private static HashSet<Coordinate> Part2(Dictionary<char, List<Coordinate>> antennas, (int, int) dimensions)
    {
        HashSet<Coordinate> antinodes = new HashSet<Coordinate>();
        foreach (KeyValuePair<char, List<Coordinate>> antenna in antennas)
        {
            for (int a = 0; a < antenna.Value.Count - 1; a++)
            {
                for (int b = a + 1; b < antenna.Value.Count; b++)
                {
                    Diff diff = antenna.Value[a].GetDiff(antenna.Value[b]);
                    Coordinate c = antenna.Value[a];
                    antinodes.Add(c);
                    while ((c = new Coordinate(c.X + diff.X, c.Y + diff.Y)).IsValid(dimensions))
                    {
                        antinodes.Add(c);
                    }

                    c = antenna.Value[b];
                    antinodes.Add(c);
                    while ((c = new Coordinate(c.X - diff.X, c.Y - diff.Y)).IsValid(dimensions))
                    {
                        antinodes.Add(c);
                    }
                }
            }
        }

        return antinodes;
    }
}