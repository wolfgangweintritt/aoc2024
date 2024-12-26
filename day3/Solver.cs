using System.Text.RegularExpressions;

public class Solver
{
    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string text = File.ReadAllText(filePath).Trim();

        // text = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        
        var products = Part1(text);
        Console.WriteLine(products.Sum());
        
        // part 2
        products = Part2(text);
        Console.WriteLine(products.Sum());
    }

    private static IEnumerable<int> Part2(string text)
    {
        var matches = Regex.Matches(text, @"mul\(\d+,\d+\)|don't\(\)|do\(\)");
        
        foreach (Match m in matches)
        {
            Console.WriteLine(m.Value);
        }
        
        bool multiply = true;
        
        IEnumerable<int> products = matches
            .Where(m =>
            {
                if (m.Value == "do()")
                {
                    multiply = true;
                    return false;
                };

                if (m.Value == "don't()")
                {
                    multiply = false;
                    return false;
                }

                return multiply;
            })
            .Select(m =>
            {
                int commaIndex = m.Value.IndexOf(',');
                return int.Parse(m.Value.Substring(4, commaIndex - 4)) * int.Parse(m.Value.Substring(commaIndex + 1, m.Value.Length - commaIndex - 2));
            });
        
        return products;
    }

    private static IEnumerable<int> Part1(string text)
    {
        var matches = Regex.Matches(text, @"mul\(\d+,\d+\)");

        foreach (Match m in matches)
        {
            Console.WriteLine(m.Value);
        }
        
        IEnumerable<int> products = matches
            .Select(m =>
            {
                int commaIndex = m.Value.IndexOf(',');
                return int.Parse(m.Value.Substring(4, commaIndex - 4)) * int.Parse(m.Value.Substring(commaIndex + 1, m.Value.Length - commaIndex - 2));
            });
        return products;
    }
}