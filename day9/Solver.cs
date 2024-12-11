namespace day9;

public class Solver
{
    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string text = File.ReadAllText(filePath).Trim();

        // text = "2333133121414131402";

        List<int> disk = new List<int>();
        for (int i = 0; i < text.Length; i++)
        {
            int size = int.Parse(text[i].ToString());
            if (i % 2 == 0)
            {
                disk.AddRange(new int[size].Select(x => i / 2));
            }
            else
            {
                disk.AddRange(new int[size].Select(x => -1));
            }
        }
        
        Console.WriteLine(String.Join(',', disk));

        List<int> reverseDiskWithoutSpaces = Enumerable
            .Reverse(disk)
            .Where(x => x != -1)
            .ToList();
        
        Console.WriteLine(String.Join(',', reverseDiskWithoutSpaces));

        int reverseIndex = 0;
        IList<int> compactDisk = disk
            .Select(
                (x, i) =>
                {
                    if (i > reverseDiskWithoutSpaces.Count - 1)
                    {
                        return -1;
                    }

                    if (x != -1)
                    {
                        return x;
                    }
                    
                    return reverseDiskWithoutSpaces[reverseIndex++];
                })
            .ToList();

        Console.WriteLine(String.Join(',', compactDisk));

        long checkSum = compactDisk
            .Select((x, i) => new { Value = x, Index = i })
            .Aggregate(
                0L,
                (acc, x) => acc + (x.Value == -1 ? 0 : x.Value) * x.Index
            );
        
        Console.WriteLine(checkSum);
    }
}

