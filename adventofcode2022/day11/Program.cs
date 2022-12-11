using System.Numerics;

namespace adventofcode2022.day11;

public class Monkey
{
    public int Number { get; set; }
    public Queue<Item> Items { get; set; } = new();
    public MonkeyOperation Operation { get; set; } = new();
    public MonkeyTest Test { get; set; } = new();

    public Func<long, long> StressManagement { get; set; } = null!;

    public int ItemsInspected { get; private set; }

    public void TakeTurn(List<Monkey> monkeys)
    {
        while (Items.TryDequeue(out Item? item) && item != null)
        {
            ItemsInspected++;
            //Console.WriteLine($"Monkey {Number} takes item with worry level { item.WorryLevel}");
            item.WorryLevel = Operation.Calculate(item.WorryLevel);
            //Console.WriteLine($"while the monkey inspects the item your worry level goes to { item.WorryLevel}");
            item.WorryLevel = StressManagement(item.WorryLevel);
            //Console.WriteLine($"When monkey is down your worry level for the item goes to { item.WorryLevel}");
            (int monkey, bool testResult) = Test.WhichMonkeyToThrowTo(item.WorryLevel);
            //Console.WriteLine($"Monkey throws the item to {monkey}, due to the item levels was {(testResult ? "" : "not")} divisible by {Test.DivisibleTest}");
            monkeys[monkey].Items.Enqueue(item);
        }
    }

    public override string ToString()
    {
        return $"Monkey {Number}, Operation: {Operation}, Test: {Test}, Items Inspected: {ItemsInspected} Items: {string.Join(", ", Items.ToArray().Select(x => x.WorryLevel.ToString()))}";
    }
}


public class Item
{
    public long WorryLevel { get; set; }
}

public class MonkeyOperation
{
    public string LeftHandSide { get; set; } = null!;
    public string Operation { get; set; } = null!;
    public string RightHandSide { get; set; } = null!;

    public long Calculate(long old)
    {
        long lhs = GetValue(old, LeftHandSide);
        long rhs = GetValue(old, RightHandSide);
        if (Operation == "*")
            return lhs * rhs;
        else if (Operation == "+")
            return lhs + rhs;
        else
            throw new NotSupportedException($"Unknown monkey operation: {Operation}");
    }

    private long GetValue(long old, string input)
    {
        if (int.TryParse(input, out int value))
            return value;
        else if (input == "old") return old;
        else
            throw new NotSupportedException($"Unknown value in monkey operation: {input}");
    }

    public override string ToString()
    {
        return $"new = {LeftHandSide} {Operation} {RightHandSide}";
    }
}


public class MonkeyTest
{
    public int DivisibleTest { get; set; }
    public int TrueThrowtoMonkey { get; set; }
    public int FalseThrowtoMonkey { get; set; }


    public (int monkey, bool) WhichMonkeyToThrowTo(BigInteger worryLevel)
    {
        if (worryLevel % DivisibleTest == 0)
            return (TrueThrowtoMonkey, true);
        else
            return (FalseThrowtoMonkey, false);
    }

    public override string ToString()
    {
        return $"Divisible by {DivisibleTest}, true: {TrueThrowtoMonkey}, false: {FalseThrowtoMonkey}";
    }
}

internal class Program
{

    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("- - - - - Day 11 - - - - -");

        Func<long, long> stressManagement1 = i => i / 3;
        List<Monkey> monkeys = Load(stressManagement1);

        //Console.WriteLine($"Initial");
        //foreach (var monkey in monkeys)
        //{
        //    Console.WriteLine(monkey.ToString());
        //}

        int rounds = 20;
        for (int i = 0; i < rounds; i++)
        {
            //Console.WriteLine($"");
            //Console.WriteLine($"Round: {i + 1}");
            foreach (var monkey in monkeys)
            {
                monkey.TakeTurn(monkeys);
            }
            //foreach (var monkey in monkeys)
            //{
            //    Console.WriteLine(monkey.ToString());
            //}
        }

        var monkeyActivity = monkeys.OrderByDescending(x => x.ItemsInspected).ToList();
        long monkeyBusiness = monkeyActivity[0].ItemsInspected * monkeyActivity[1].ItemsInspected;
        Console.WriteLine($"Part 1: {monkeyBusiness}");

        // Reset for part 2
        var leastCommonMultiple = monkeys.Select(monkey => monkey.Test.DivisibleTest).Aggregate((a, b) => a * b);
        Func<long, long> stressManagement2 = i => i % leastCommonMultiple;
        monkeys = Load(stressManagement2);
        rounds = 10000;
        for (int i = 0; i < rounds; i++)
        {

            //if (i % 1000 == 0 || i == 20)
            //{
            //    Console.WriteLine($"");
            //    Console.WriteLine($"Round: {i}");
            //    foreach (var monkey in monkeys)
            //    {
            //        Console.WriteLine(monkey.ToString());
            //    }
            //}

            foreach (var monkey in monkeys)
            {
                monkey.TakeTurn(monkeys);
            }

        }

        //Console.WriteLine($"Round: {rounds}");
        //foreach (var monkey in monkeys)
        //{
        //    Console.WriteLine(monkey.ToString());
        //}

        monkeyActivity = monkeys.OrderByDescending(x => x.ItemsInspected).ToList();
        monkeyBusiness = (long)monkeyActivity[0].ItemsInspected * (long)monkeyActivity[1].ItemsInspected;
        Console.WriteLine($"Part 2: {monkeyBusiness}");
    }

    private static List<Monkey> Load(Func<long, long> stressManagement)
    {
        string inputFile = "day11/input";
        string[] lines = File.ReadAllLines(inputFile);
        List<Monkey> monkeys = new List<Monkey>();
        for (int i = 0; i <= lines.Length / 7; i++)
        {
            int baseLine = i * 7;
            Monkey monkey = new();
            monkey.StressManagement = stressManagement;
            monkey.Number = int.Parse(lines[baseLine].Split(" ")[1].Replace(":", ""));

            // Items
            string[] itemParts = lines[baseLine + 1].Split(" ");
            foreach (var itemPart in itemParts)
            {
                if (int.TryParse(itemPart.Replace(",", ""), out int itemWorryLevel))
                {
                    Item item = new Item()
                    {
                        WorryLevel = itemWorryLevel
                    };
                    monkey.Items.Enqueue(item);
                }
            }

            // Operation
            string[] operationParts = lines[baseLine + 2].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            monkey.Operation = new MonkeyOperation()
            {
                LeftHandSide = operationParts[3],
                Operation = operationParts[4],
                RightHandSide = operationParts[5],
            };

            // Test
            string[] testParts1 = lines[baseLine + 3].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] testParts2 = lines[baseLine + 4].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] testParts3 = lines[baseLine + 5].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            monkey.Test = new MonkeyTest()
            {
                DivisibleTest = int.Parse(testParts1[testParts1.Length - 1]),
                TrueThrowtoMonkey = int.Parse(testParts2[testParts2.Length - 1]),
                FalseThrowtoMonkey = int.Parse(testParts3[testParts3.Length - 1]),

            };

            monkeys.Add(monkey);
        }
        return monkeys;
    }

}
