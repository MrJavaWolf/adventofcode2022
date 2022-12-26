using System.Text;

namespace adventofcode2022.day17;

public class Chamber
{
    private readonly WindDirection wind;
    public int[,] Area = new int[7, 400_000_000];
    public int RocksPlaces = 0;
    public int CurrentTopOfStack = 0;

    public Dictionary<int[,], HashSet<int>> CycleDetection = new Dictionary<int[,], HashSet<int>>();

    public Chamber(WindDirection wind)
    {
        this.wind = wind;
    }

    public void AddRock(int[,] rock)
    {
        //Console.WriteLine("New rock");
        int currentY = CurrentTopOfStack + 3;
        int currentX = 2;
        bool isRockPlaced = false;
        while (!isRockPlaced)
        {
            // Wind blows
            if (wind.IsWindBlowingRight())
            {
                // Can only move if it is not up against the wall
                if (currentX + rock.GetLength(0) < Area.GetLength(0) &&
                    !DoOverlap(rock, currentX + 1, currentY))
                {
                    //Console.WriteLine("Wind moves rock to the right");
                    currentX++;
                }
                else
                {
                    //Console.WriteLine("Wind cannot move rock to the right");
                }
            }
            else
            {
                // Can only move if it is not up against the wall
                if (currentX > 0 &&
                    !DoOverlap(rock, currentX - 1, currentY))
                {
                    //Console.WriteLine("Wind moves rock to the left");
                    currentX--;
                }
                else
                {
                    //Console.WriteLine("Wind cannot move rock to the left");
                }
            }
            wind.NextPosition();

            // Rock falls
            if (currentY == 0 || DoOverlap(rock, currentX, currentY - 1))
            {
                Place(rock, currentX, currentY);
                RocksPlaces++;
                isRockPlaced = true;

                if (!CycleDetection.ContainsKey(rock))
                    CycleDetection.Add(rock, new HashSet<int>());

                if (!CycleDetection[rock].Contains(wind.CurrentPosition))
                {
                    CycleDetection[rock].Add(wind.CurrentPosition);
                }
                else
                {
                    // cycle detected
                }
            }
            else
            {
                currentY--;
            }
        }

        CurrentTopOfStack = GetTopOfStack(CurrentTopOfStack);
    }

    public void Place(int[,] rock, int x, int y)
    {
        for (int i = 0; i < rock.GetLength(0); i++)
        {
            for (int j = 0; j < rock.GetLength(1); j++)
            {
                if (Area[i + x, j + y] != 0 && rock[i, j] != 0)
                    throw new Exception($"Failed to place rock, " +
                        $"the rock overlaps existing rock at {(i + x)},{(j + y)}." +
                        $"Tried to place rock at {x},{y}");
                if (rock[i, j] != 0)
                    Area[i + x, j + y] = rock[i, j];
            }
        }
    }

    public bool DoOverlap(int[,] rock, int x, int y)
    {
        for (int i = 0; i < rock.GetLength(0); i++)
        {
            for (int j = 0; j < rock.GetLength(1); j++)
            {
                if (Area[i + x, j + y] != 0 && rock[i, j] != 0)
                    return true;
            }
        }
        return false;
    }

    public int GetTopOfStack(int fromY)
    {
        for (int y = fromY; y < Area.GetLength(1); y++)
        {
            bool found = false;
            for (int x = 0; x < Area.GetLength(0); x++)
            {
                if (Area[x, y] != 0)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return y;
            }
        }
        throw new Exception("Stack is too tall! Increase size");
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        for (int y = CurrentTopOfStack; y >= 0; y--)
        {
            for (int x = 0; x < Area.GetLength(0); x++)
            {
                if (Area[x, y] == 0)
                {
                    stringBuilder.Append(".");
                }
                else
                {
                    stringBuilder.Append("#");
                }
            }
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString().Trim();
    }
}

public class WindDirection
{
    public string Wind { get; set; }
    public int WindLength => Wind.Length;
    public int CurrentPosition { get; set; }
    public int TotalMovements { get; set; }
    public WindDirection(string wind)
    {
        Wind = wind;
    }

    public bool IsWindBlowingRight()
    {
        if (Wind[CurrentPosition] == '>') return true;
        else if (Wind[CurrentPosition] == '<') return false;
        else throw new Exception("Unkown wind direction");
    }

    public void NextPosition()
    {
        TotalMovements++;
        CurrentPosition++;
        if (CurrentPosition >= Wind.Length)
            CurrentPosition = 0;
    }
}

internal class Program
{
    static string Load()
    {
        string file = "day17/input";
        string[] lines = File.ReadAllLines(file);
        string message = lines[0].Trim();
        return message;
    }

    public static List<int[,]> GetRocks()
    {
        List<int[,]> rocks = new List<int[,]>
        {
            new int[,]
            {
                { 1 },
                { 1 },
                { 1 },
                { 1 }
            },
            new int[,]
            {
                { 0,1,0 },
                { 1,1,1 },
                { 0,1,0 }
            },
            new int[,]
            {
                { 1,0,0 },
                { 1,0,0 },
                { 1,1,1 }
            },
            new int[,]
            {
                { 1,1,1,1 },
            },
            new int[,]
            {
                { 1, 1 },
                { 1, 1 }
            }
        };

        return rocks;
    }

    private static void Part1(WindDirection wind, List<int[,]> rocks)
    {
        Chamber chamber = new Chamber(wind);
        for (long i = 0; i < 2022; i++)
        {
            chamber.AddRock(rocks[(int)(i % rocks.Count)]);
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine($"{chamber}");
            //Console.WriteLine($"_______");
        }
        //Console.WriteLine($"{chamber}");
        Console.WriteLine($"Part 1: {chamber.CurrentTopOfStack}");
    }


    private static void Part2(WindDirection wind, List<int[,]> rocks)
    {
        Chamber chamber = new Chamber(wind);
        for (long i = 0; i < 1_000_000_000_000; i++)
        {
            if (i % 100_000 == 0)
            {
                Console.WriteLine($"{i}");
            }
            chamber.AddRock(rocks[(int)(i % rocks.Count)]);
        }
        Console.WriteLine($"Part 2: {chamber.CurrentTopOfStack}");
    }

    public static void Run()
    {
        WindDirection wind = new WindDirection(Load());
        List<int[,]> rocks = GetRocks();
        Part1(wind, rocks);
        Part2(wind, rocks);
    }


}
