using System.Text.RegularExpressions;

public class Solver
{
    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string text = File.ReadAllText(filePath).Trim();

        // text = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
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
        
        Console.WriteLine(products.Sum());
    }
}