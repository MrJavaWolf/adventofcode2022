using System.Text;

namespace adventofcode2022.day5;

class Program
{

    class Ship
    {
        public List<Stack<char>> Stacks = new();

        public void ExecuteCrateMover9000(CraneInstruction instruction)
        {
            for (int i = 0; i < instruction.NumberOfCrates; i++)
            {
                Stacks[instruction.ToStack - 1].Push(Stacks[instruction.FromStack - 1].Pop());
            }
        }

        public void ExecuteCrateMover9000(IEnumerable<CraneInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                ExecuteCrateMover9000(instruction);
            }
        }

        public void ExecuteCrateMover9001(CraneInstruction instruction)
        {
            Stack<char> crane = new Stack<char>();
            for (int i = 0; i < instruction.NumberOfCrates; i++)
            {
                crane.Push(Stacks[instruction.FromStack - 1].Pop());
            }
            for (int i = 0; i < instruction.NumberOfCrates; i++)
            {
                Stacks[instruction.ToStack - 1].Push(crane.Pop());
            }
        }

        public void ExecuteCrateMover9001(IEnumerable<CraneInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                ExecuteCrateMover9001(instruction);
            }
        }


        public string GetTopOfTheStacks()
        {
            StringBuilder stringBuilder = new();
            foreach (var stack in Stacks)
            {
                if (stack.TryPeek(out char top))
                {
                    stringBuilder.Append(top);
                }
                else
                {
                    stringBuilder.Append(" ");
                }
            }
            return stringBuilder.ToString();
        }
    }

    class CraneInstruction
    {
        public int NumberOfCrates { get; set; }
        public int FromStack { get; set; }
        public int ToStack { get; set; }
    }

    public static void Run()
    {
        (Ship ship, List<CraneInstruction> instructions) = Load();
        ship.ExecuteCrateMover9000(instructions);
        string topOfTheStacks = ship.GetTopOfTheStacks();
        Console.WriteLine($"Part 1: {topOfTheStacks}");
        (ship, instructions) = Load();
        ship.ExecuteCrateMover9001(instructions);
        topOfTheStacks = ship.GetTopOfTheStacks();
        Console.WriteLine($"Part 2: {topOfTheStacks}");
    }

    static (Ship ship, List<CraneInstruction> instructions) Load()
    {
        string file = "day5/input";
        string[] lines = File.ReadAllLines(file);

        // Number of stacks
        int numberOfstacksLine = 9;
        int numberOfStacks = lines[numberOfstacksLine - 1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;
        Ship ship = new();
        for (int i = 0; i < numberOfStacks; i++)
        {
            ship.Stacks.Add(new Stack<char>());
        }

        // Initial setup
        int bottomCargoSetupLine = 8;
        int topCargoSetupLine = 1;
        int cargoSizeInFile = 4; // characters wide
        for (int i = bottomCargoSetupLine; i >= topCargoSetupLine; i--)
        {
            for (int stack = 0; stack < ship.Stacks.Count; stack++)
            {
                var cargo = lines[i - 1][cargoSizeInFile * stack + 1];
                if (cargo == ' ') continue;
                ship.Stacks[stack].Push(cargo);
            }
        }

        // Instructions
        List<CraneInstruction> instructions = new List<CraneInstruction>();
        int startInstructionLine = 11;
        for (int i = startInstructionLine - 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(" ");
            CraneInstruction instruction = new CraneInstruction();
            instruction.NumberOfCrates = int.Parse(parts[1]);
            instruction.FromStack = int.Parse(parts[3]);
            instruction.ToStack = int.Parse(parts[5]);
            instructions.Add(instruction);
        }

        return (ship, instructions);
    }

}
