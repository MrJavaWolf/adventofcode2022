using System.Collections.Generic;

namespace adventofcode2022.day10;

public class Program
{
    enum InstructionType
    {
        noop,
        addx
    }
    class Instruction
    {
        public InstructionType InstructionType { get; set; }
        public int Value { get; set; }
    }

    class CrtSimulator
    {
        public const int Height = 6;
        public const int Width = 40;
        public int currentX { get; set; }
        public int currentY { get; set; }

        public void Clock(CpuSimulator cpu)
        {
            if (currentX >= Width)
            {
                currentY++;
                currentX = 0;
                Console.WriteLine();
            }
            if (cpu.RegisterX == currentX ||
                cpu.RegisterX - 1 == currentX ||
                cpu.RegisterX + 1 == currentX)
                Console.Write("#");
            else
                Console.Write(".");

            currentX++;
        }
    }


    class CpuSimulator
    {
        public int RegisterX { get; set; } = 1;
        public bool IsDone => currentInstruction >= instructions.Count;
        public int CurrentCycle { get; set; } 

        private bool AddXStarted = false;

        private readonly List<Instruction> instructions;
        private int currentInstruction;

        public CpuSimulator(List<Instruction> instructions)
        {
            this.instructions = instructions;
        }


        public void Clock()
        {
            if (IsDone) return;

            CurrentCycle++;
            // Handle Noop
            if (instructions[currentInstruction].InstructionType == InstructionType.noop)
            {
                currentInstruction++;
                return;
            }

            // Handle AddX
            if (!AddXStarted)
            {
                AddXStarted = true;
            }
            else
            {
                AddXStarted = false;
                RegisterX += instructions[currentInstruction].Value;
                currentInstruction++;
            }
        }
    }

    public static void Run()
    {

        Console.WriteLine();
        Console.WriteLine("- - - - - Day 10 - - - - -");

        List<Instruction> instructions = Load();
        CrtSimulator crt = new CrtSimulator();
        CpuSimulator cpu = new(instructions);
        int sumSignalStrength = 0;
        while (!cpu.IsDone)
        {
            crt.Clock(cpu);
            cpu.Clock();
            if (cpu.CurrentCycle == 20 - 1 ||
                cpu.CurrentCycle == 60 - 1 ||
                cpu.CurrentCycle == 100 - 1 ||
                cpu.CurrentCycle == 140 - 1 ||
                cpu.CurrentCycle == 180 - 1 ||
                cpu.CurrentCycle == 220 - 1)
            {

                int singalStrength = (cpu.CurrentCycle + 1) * cpu.RegisterX;
                sumSignalStrength += singalStrength;
            }
        }
        Console.WriteLine();
        Console.WriteLine($"Part 1: {sumSignalStrength}");
    }

    static List<Instruction> Load()
    {
        List<Instruction> instructions = new();
        string inputFile = "day10/input";
        string[] lines = File.ReadAllLines(inputFile);
        foreach (var line in lines)
        {
            Instruction instruction = new Instruction();
            if (line == "noop")
            {
                instruction.InstructionType = InstructionType.noop;
            }
            else
            {
                string[] parts = line.Split(" ");
                instruction.InstructionType = InstructionType.addx;
                instruction.Value = int.Parse(parts[1]);
            }

            instructions.Add(instruction);
        }
        return instructions;
    }
}
