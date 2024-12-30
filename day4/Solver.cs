using System.Text.RegularExpressions;

public class Solver
{
    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);

        // lines = new string[]
        // {
        //     "..X...",
        //     ".SAMX.",
        //     ".A..A.",
        //     "XMAS.S",
        //     ".X....",
        // };
        //
        // lines = new string[]
        // {
        //     "MMMSXXMASM",
        //     "MSAMXMSMSA",
        //     "AMXSXMAAMM",
        //     "MSAMASMSMX",
        //     "XMASAMXAMM",
        //     "XXAMMXXAMA",
        //     "SMSMSASXSS",
        //     "SAXAMASAAA",
        //     "MAMMMXMMMM",
        //     "MXMXAXMASX",
        // };

        Func<IEnumerable<string>, int> xmasSum = inputLines => inputLines
            .Select(l => Regex.Matches(l, @"XMAS").Count + Regex.Matches(l, @"SAMX").Count)
            .Sum();

        string[] rotatedMinus90 = RotateMatrixCounterClockwise(lines);
        string[] diagonals = DiagonalsFromMatrix(lines);
        string[] diagonals2 = Diagonals2FromMatrix(lines);

        Console.WriteLine(xmasSum(lines) + xmasSum(rotatedMinus90) + xmasSum(diagonals) + xmasSum(diagonals2));

        Console.WriteLine(Part2(lines));
    }

    private static int Part2(string[] lines)
    {
        Func<string[], int, int, bool> containsMasCross = (string[] l, int r, int c) =>
        {
            if (r <= 0 || r >= l[0].Length - 1 || c <= 0 || c >= l.Length - 1 || l[r][c] != 'A')
            {
                return false;
            }

            char topLeft = lines[r - 1][c - 1];
            char topRight = lines[r - 1][c + 1];
            char bottomLeft = lines[r + 1][c - 1];
            char bottomRight = lines[r + 1][c + 1];

            bool IsMOrS(char ch) => ch is 'S' or 'M';

            return IsMOrS(topLeft) && IsMOrS(topRight) && IsMOrS(bottomLeft) && IsMOrS(bottomRight)
                   && topLeft != bottomRight
                   && topRight != bottomLeft;
        };

        int masCrossCount = 0;
        for (int r = 0; r < lines.Length; r++)
        {
            for (int c = 0; c < lines[0].Length; c++)
            {
                if (containsMasCross(lines, r, c))
                {
                    masCrossCount++;
                }
            }
        }

        return masCrossCount;
    }

    private static string[] DiagonalsFromMatrix(string[] lines)
    {
        string[] newMatrix = new string[lines.Length + lines[0].Length - 1];
        for (int r = 0; r < lines.Length; r++)
        {
            int tmpR = r;
            for (int c = 0; c < lines[r].Length && tmpR < lines.Length; c++)
            {
                newMatrix[r] += lines[tmpR++][c].ToString();
            }
        }

        for (int c = 1; c < lines[0].Length; c++)
        {
            int tmpC = c;
            for (int r = 0; r < lines.Length && tmpC < lines[0].Length; r++)
            {
                newMatrix[c + lines.Length - 1] += lines[r][tmpC++].ToString();
            }
        }

        return newMatrix;
    }

    private static string[] Diagonals2FromMatrix(string[] lines)
    {
        string[] newMatrix = new string[lines.Length + lines[0].Length - 1];
        for (int r = 0; r < lines.Length; r++)
        {
            int tmpR = r;
            for (int c = lines[r].Length - 1; c >= 0 && tmpR < lines.Length; c--)
            {
                newMatrix[r] += lines[tmpR++][c].ToString();
            }
        }

        for (int c = 0; c < lines[0].Length - 1; c++)
        {
            int tmpC = c;
            for (int r = 0; r < lines.Length && tmpC >= 0; r++)
            {
                newMatrix[c + 1 + lines.Length - 1] += lines[r][tmpC--].ToString();
            }
        }

        return newMatrix;
    }

    static string[] RotateMatrixCounterClockwise(string[] oldMatrix)
    {
        string[] newMatrix = new string[oldMatrix[0].Length];
        int newRow = 0;
        for (int oldColumn = oldMatrix[0].Length - 1; oldColumn >= 0; oldColumn--)
        {
            for (int oldRow = 0; oldRow < oldMatrix.Length; oldRow++)
            {
                newMatrix[newRow] += oldMatrix[oldRow][oldColumn].ToString();
            }

            newRow++;
        }

        return newMatrix;
    }
}