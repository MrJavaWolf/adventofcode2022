namespace adventofcode2022.day12;

class HeightMap
{
    public char[,] Map { get; set; }
    public HeightMap(int x, int y)
    {
        Map = new char[x, y];
    }

    public (int x, int y) GetStartPosition()
    {
        return GetPosition('S');
    }

    public (int x, int y) GetEndPosition()
    {
        return GetPosition('E');
    }

    public List<(int x, int y)> FindAllScenicStartingPositions()
    {
        List<(int x, int y)> startingPositions = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] == 'a')
                    startingPositions.Add((i, j));
            }
        }
        return startingPositions;
    }

    public (int x, int y) GetPosition(char c)
    {
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] == c)
                    return (i, j);
            }
        }

        throw new Exception($"Did not find position with '{c}'");
    }
}

internal class Program
{

    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("- - - - - Day 12 - - - - -");
        HeightMap map = Load();
        var startPosition = map.GetStartPosition();
        var endPosition = map.GetEndPosition();
        Astar astar = new Astar(map.Map);
        var path = astar.FindPath(startPosition.x, startPosition.y, endPosition.x, endPosition.y);
        Console.WriteLine($"Part 1: {path?.Count}");

        //for (int y = 0; y < map.Map.GetLength(1); y++)
        //{
        //    for (int x = 0; x < map.Map.GetLength(0); x++)
        //    {
        //        Node? step = path?.Where(n => n.PositionX == x && n.PositionY == y).FirstOrDefault();
        //        if (step != null)
        //        {
        //            Console.Write(".");
        //        }
        //        else
        //        {
        //            Console.Write(map.Map[x, y]);
        //        }
        //    }
        //    Console.WriteLine();
        //}
        var bestPath = astar.FindPath(0, 26, endPosition.x, endPosition.y);
        //var possibleStartingPositions = map.FindAllScenicStartingPositions();
        //int minimum = int.MaxValue;
        //Stack<Node>? bestPath = null;
        //foreach (var possibleStartingPosition in possibleStartingPositions)
        //{
        //    //Console.Write(".");
        //    var possiblePath = astar.FindPath(possibleStartingPosition.x, possibleStartingPosition.y, endPosition.x, endPosition.y);
        //    if(possiblePath != null && possiblePath.Count < minimum)
        //    {
        //        minimum = possiblePath.Count;
        //        bestPath = possiblePath;
        //    }
        //}
        Console.WriteLine($"Part 2: {bestPath?.Count}");
    }

    public static HeightMap Load()
    {
        string inputFile = "day12/input";
        string[] lines = File.ReadAllLines(inputFile);
        HeightMap heightMap = new HeightMap(lines[0].Length, lines.Length);
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                heightMap.Map[x, y] = lines[y][x];
            }
        }
        return heightMap;
    }
}
