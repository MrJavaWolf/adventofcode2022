namespace adventofcode2022.day3;

class Program
{
    class Backpack
    {
        public readonly string content;
        private string LeftPocket => content.Substring(0, content.Length / 2);
        private string rightPocket => content.Substring(content.Length / 2);

        public Backpack(string content)
        {
            this.content = content;
        }

        public char getErrorItem()
        {
            char errorItem = LeftPocket.Intersect(rightPocket).First();
            return errorItem;
        }

    }

    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("- - - - - Day 3 - - - - -");

        List<Backpack> backpacks = LoadInput();
        Console.WriteLine($"total number of backpacks: {backpacks.Count}");
        Console.WriteLine($"Part 1: {backpacks.Sum(x => getErrorPriority(x.getErrorItem()))}");
        int total = 0;
        for (int i = 0; i < backpacks.Count / 3; i++)
        {
            char badge =
                backpacks[i * 3 + 0].content
                    .Intersect(backpacks[i * 3 + 1].content)
                    .Intersect(backpacks[i * 3 + 2].content).First();
            total += getErrorPriority(badge);
        }
        Console.WriteLine($"Part 2: {total}");
    }

    private static int getErrorPriority(char errorItem)
    {
        if (errorItem >= 'a' && errorItem <= 'z')
            return errorItem - 'a' + 1;
        else
            return errorItem - 'A' + 27;
    }

    static List<Backpack> LoadInput()
    {
        string file = "day3/input";
        string[] lines = File.ReadAllLines(file);
        List<Backpack> backpacks = new List<Backpack>();
        foreach (var line in lines)
        {
            backpacks.Add(new Backpack(line));
        }
        return backpacks;
    }
}
