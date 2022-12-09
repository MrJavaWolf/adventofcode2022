namespace adventofcode2022.day1;


class Elf
{
    public List<int> CarrcarriedCalories = new();
    public long TotalCalories => CarrcarriedCalories.Sum();
}


class Program
{

    public static void Run()
    {
        List<Elf> elfs = GetElfs();

        // Part 1
        long mostCalories = elfs.Max(x => x.TotalCalories);
        Console.WriteLine($"Elf with most calories carries: {mostCalories} calories");

        // Part 2
        List<Elf> orderedListOfElfs = elfs.OrderByDescending(x => x.TotalCalories).ToList();
        long top3Combined = orderedListOfElfs[0].TotalCalories + orderedListOfElfs[1].TotalCalories + orderedListOfElfs[2].TotalCalories;
        Console.WriteLine($"The three Elfs which carries the most calories, carries combined: {top3Combined} calories");

    }

    private static List<Elf> GetElfs()
    {
        string inputFile = "day1/input";
        string[] lines = File.ReadAllLines(inputFile);
        List<Elf> elfs = new List<Elf>();
        Elf currentElf = new Elf();
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Trim() == string.Empty)
            {
                currentElf = new Elf();
                elfs.Add(currentElf);
            }
            else
            {
                int calories = int.Parse(lines[i]);
                currentElf.CarrcarriedCalories.Add(calories);
            }
        }
        return elfs;
    }
}
