namespace adventofcode2022.day4;

class Program
{
    class ElfGroup
    {
        public Assignment Elf1 { get; set; } = new Assignment();
        public Assignment Elf2 { get; set; } = new Assignment();
    }
    class Assignment
    {
        public int From { get; set; }
        public int To { get; set; }

        public bool Contains(Assignment other)
            => other.From >= this.From && other.To <= this.To;
        public bool Overlaps(Assignment other)
        {
            if (other.From == this.From) return true;
            if (other.To == this.To) return true;
            if (other.From <= this.From && other.To >= this.From) return true;
            if (other.From <= this.To && other.To >= this.To) return true;
            return false;
        }

        /// <summary>
        /// ------------||-----------------
        /// ---------|---|-----------------
        /// ---------|-------|-------------
        /// ------------|----|-------------
        /// -----------|---|---------------
        /// </summary>
        /// <returns></returns>



        public override string ToString()
        {
            return $"{From}-{To}";
        }
    }

    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("- - - - - Day 4 - - - - -");

        List<ElfGroup> ElfGroups = LoadInput();
        Console.WriteLine($"Number of elf groups: {ElfGroups.Count}");
        int count = 0;
        for (int i = 0; i < ElfGroups.Count; i++)
        {
            ElfGroup group = ElfGroups[i];
            if (group.Elf1.Contains(group.Elf2) || group.Elf2.Contains(group.Elf1))
            {
                //Console.WriteLine($"{i + 1}: {group.Elf1}, {group.Elf2}");
                count++;
            }
        }
        Console.WriteLine($"Part 1: {count}");

        count = 0;
        for (int i = 0; i < ElfGroups.Count; i++)
        {
            ElfGroup group = ElfGroups[i];
            if (group.Elf1.Overlaps(group.Elf2) || group.Elf2.Overlaps(group.Elf1) ||
                group.Elf1.Contains(group.Elf2) || group.Elf2.Contains(group.Elf1))
            {
                //Console.WriteLine($"{i + 1}: {group.Elf1}, {group.Elf2}");
                count++;
            }
        }
        Console.WriteLine($"Part 2: {count}");
    }


    static List<ElfGroup> LoadInput()
    {
        string file = "day4/input";
        string[] lines = File.ReadAllLines(file);
        List<ElfGroup> backpacks = new List<ElfGroup>();
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            backpacks.Add(new ElfGroup()
            {
                Elf1 = GetAssignment(parts[0]),
                Elf2 = GetAssignment(parts[1]),
            });
        }
        return backpacks;
    }

    static Assignment GetAssignment(string s)
    {
        Assignment assignment = new Assignment();
        var parts = s.Split("-");
        assignment.From = int.Parse(parts[0]);
        assignment.To = int.Parse(parts[1]);
        return assignment;
    }
}
