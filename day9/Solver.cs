namespace day9;

public class Solver
{
    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        string text = File.ReadAllText(filePath).Trim();

        // text = "2333133121414131402";

        var compactDisk = DefragPart1(text);
        // Console.WriteLine(String.Join(',', compactDisk));

        long checkSum = CalculateCheckSum(compactDisk);
        Console.WriteLine(checkSum);

    	// part2
        var compactDisk2 = DefragPart2(text);
        // Console.WriteLine(String.Join(',', compactDisk2));
        
        checkSum = CalculateCheckSum(compactDisk2);
        Console.WriteLine(checkSum);
    }

    private static IList<int> DefragPart1(string text)
    {
        List<int> disk = new List<int>();
        for (int i = 0; i < text.Length; i++)
        {
            int size = int.Parse(text[i].ToString());
            disk.AddRange(new int[size].Select(x => i % 2 == 0 ? i / 2 : -1));
        }

        List<int> reverseDiskWithoutSpaces = Enumerable
            .Reverse(disk)
            .Where(x => x != -1)
            .ToList();

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
        return compactDisk;
    }

    private static IList<int> DefragPart2(string text)
    {
        LinkedList<(int, int)> linkedListDisk = new LinkedList<(int, int)>(text
            .Select((f, i) => (f - '0', i % 2 == 0 ? i / 2 : -1))
        );
    
        var fileToInsert = linkedListDisk.Last;
        while (fileToInsert != null)
        {
            if (fileToInsert.Value.Item2 == -1) // skip free space
            {
                fileToInsert = fileToInsert.Previous;
                continue;
            }
            
            var file = linkedListDisk.First;
            var nextFileToInsert = fileToInsert.Previous;
            while (file != null)
            {
                if (fileToInsert == file)
                {
                    break;
                }
                
                if (file.Value.Item2 != -1 || file.Value.Item1 < fileToInsert.Value.Item1) // skip files and 2 smol free space
                {
                    file = file.Next;
                    continue;
                }
    
                if (nextFileToInsert != null) // increase free space at move point
                {
                    nextFileToInsert.Value = nextFileToInsert.Value with { Item1 = nextFileToInsert.Value.Item1 + fileToInsert.Value.Item1 };
                }
                linkedListDisk.Remove(fileToInsert);
                linkedListDisk.AddBefore(file, fileToInsert);
                
                // remove free space at insert point
                file.Value = file.Value with { Item1 = file.Value.Item1 - fileToInsert.Value.Item1 };
                
                break;
            }
            fileToInsert = nextFileToInsert;
        }

        
        List<int> compactDisk = new List<int>();
        foreach (var file in linkedListDisk)
        {
            compactDisk.AddRange(new int[file.Item1].Select(x => file.Item2 == -1 ? -1 : file.Item2));
        }
        
        return compactDisk;
    }

    private static long CalculateCheckSum(IList<int> compactDisk)
    {
        return compactDisk
            .Select((x, i) => new { Value = x, Index = i })
            .Aggregate(
                0L,
                (acc, x) => acc + (x.Value == -1 ? 0 : x.Value) * x.Index
            );
    }
}

