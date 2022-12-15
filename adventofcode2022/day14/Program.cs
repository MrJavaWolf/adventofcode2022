using System.Text;

namespace adventofcode2022.day14;

class TilePoint
{
    public int x;
    public int y;
    public override string ToString()
    {
        return $"{x},{y}";
    }
}
class StonePath
{
    public List<TilePoint> Points = new List<TilePoint>();

    public override string ToString()
    {
        return string.Join(" -> ", Points.Select(x => x.ToString()));
    }
}


enum Tile
{
    Air = 0,
    Stone = 1,
    Sand = 2
}
class SimulationSpace
{
    public Tile[,] Space;
    public SimulationSpace(List<StonePath> paths)
    {
        int totalMaxX = paths.Max(x => x.Points.Max(p => p.x)) + 2;
        int totalMaxY = paths.Max(x => x.Points.Max(p => p.y)) + 2;
        int totalMinX = paths.Min(x => x.Points.Min(p => p.x));
        int totalMinY = paths.Min(x => x.Points.Min(p => p.y));
        Space = new Tile[totalMaxX, totalMaxY];
        foreach (var path in paths)
        {
            for (int i = 0; i < path.Points.Count - 1; i++)
            {
                TilePoint point1 = path.Points[i];
                TilePoint point2 = path.Points[i + 1];
                if (point1.x != point2.x)
                {
                    int minX = Math.Min(point1.x, point2.x);
                    int maxX = Math.Max(point1.x, point2.x);
                    for (int x = minX; x <= maxX; x++)
                    {
                        Space[x, point1.y] = day14.Tile.Stone;
                    }
                }
                else if (point1.y != point2.y)
                {
                    int minY = Math.Min(point1.y, point2.y);
                    int maxY = Math.Max(point1.y, point2.y);
                    for (int y = minY; y <= maxY; y++)
                    {
                        Space[point1.x, y] = day14.Tile.Stone;
                    }
                }
                else
                {
                    throw new Exception($"Point 1 {point1} and point 2 {point2} are the same");
                }
            }
        }
    }

    public bool TryAddSand(int x, int y)
    {
        bool isLanded = false;
        TilePoint currentSandPosition = new TilePoint()
        {
            x = x,
            y = y
        };

        if (Space[currentSandPosition.x, currentSandPosition.y] != Tile.Air) return false;

        while (!isLanded)
        {
            if (currentSandPosition.y + 1 >= Space.GetLength(1))
                break;
            else if (Space[currentSandPosition.x, currentSandPosition.y + 1] == day14.Tile.Air)
            {
                currentSandPosition.y++;
            }
            else if (Space[currentSandPosition.x, currentSandPosition.y + 1] == day14.Tile.Stone ||
                Space[currentSandPosition.x, currentSandPosition.y + 1] == day14.Tile.Sand)
            {
                if (Space[currentSandPosition.x - 1, currentSandPosition.y + 1] == day14.Tile.Air)
                {
                    currentSandPosition.y++;
                    currentSandPosition.x--;
                }
                else if (Space[currentSandPosition.x + 1, currentSandPosition.y + 1] == day14.Tile.Air)
                {
                    currentSandPosition.y++;
                    currentSandPosition.x++;
                }
                else
                {
                    Space[currentSandPosition.x, currentSandPosition.y] = day14.Tile.Sand;
                    isLanded = true;
                }
            }
        }
        return isLanded;
    }

    public string GetString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int y = 0; y < Space.GetLength(1); y++)
        {
            for (int x = 0; x < Space.GetLength(0); x++)
            {
                if (Space[x, y] == day14.Tile.Air)
                    stringBuilder.Append(".");
                else if (Space[x, y] == day14.Tile.Stone)
                    stringBuilder.Append("#");
                else if (Space[x, y] == day14.Tile.Sand)
                    stringBuilder.Append("O");
            }
            stringBuilder.AppendLine();
        }
        string result = stringBuilder.ToString();
        return result;
    }
    public void Print()
    {
        Console.WriteLine(GetString());
    }
}

internal class Program
{
    public static void Run()
    {
        List<StonePath> paths = Load();
        SimulationSpace simulationPart1 = new SimulationSpace(paths);
        int sandAdded = 0;
        while (simulationPart1.TryAddSand(500, 0))
        {
            sandAdded++;
        }
        //simulationPart1.Print();
        Console.WriteLine($"Part 1: {sandAdded}");

        int totalMaxX = paths.Max(x => x.Points.Max(p => p.x));
        int totalMaxY = paths.Max(x => x.Points.Max(p => p.y));
        paths.Add(new StonePath()
        {
            Points = new List<TilePoint>()
            {
                new TilePoint()
                {
                    x = 0,
                    y = totalMaxY + 2
                },
                new TilePoint()
                {
                    x = 800,
                    y = totalMaxY + 2
                }
            }
        });
        SimulationSpace simulationPart2 = new SimulationSpace(paths);
        sandAdded = 0;
        while (simulationPart2.TryAddSand(500, 0))
        {
            sandAdded++;
        }
        string result = simulationPart2.GetString();
        File.WriteAllText("day14-part2.txt", result);
        Console.WriteLine($"Part 2: {sandAdded}");
    }

    static List<StonePath> Load()
    {
        string inputFile = "day14/input";
        string[] lines = File.ReadAllLines(inputFile);

        List<StonePath> paths = new List<StonePath>();
        foreach (string line in lines)
        {
            var parts = line.Split("->");
            StonePath stonePath = new StonePath();
            foreach (var part in parts)
            {
                var position = part.Split(",");
                stonePath.Points.Add(new TilePoint()
                {
                    x = int.Parse(position[0].Trim()),
                    y = int.Parse(position[1].Trim()),
                });
            }
            paths.Add(stonePath);
        }
        return paths;
    }


}
