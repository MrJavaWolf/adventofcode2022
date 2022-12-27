namespace adventofcode2022.day18;

public record Position(int X, int Y, int Z);

public class Path
{
    public List<Position> CheckedPositions { get; set; } = new();

    public Path() { }
    public Path(Position position) { CheckedPositions.Add(position); }

    public Path Copy()
    {
        return new Path()
        {
            CheckedPositions = CheckedPositions.ToList()
        };
    }
}

public class LavaCubes
{
    public List<Position> Cubes { get; set; } = new();
    public Position MinimumExterior { get; }
    public Position MaximumExterior { get; }

    public LavaCubes(List<Position> cubes)
    {
        Cubes = cubes;
        var exterior = CalculateExteriorSurface();
        MinimumExterior = exterior.Minimum;
        MaximumExterior = exterior.Maximum;
    }

    public (Position Minimum, Position Maximum) CalculateExteriorSurface()
    {
        int maxX = int.MinValue;
        int minX = int.MaxValue;
        int maxY = int.MinValue;
        int minY = int.MaxValue;
        int maxZ = int.MinValue;
        int minZ = int.MaxValue;
        foreach (var cube in Cubes)
        {
            if (cube.X < minX) minX = cube.X;
            if (cube.X > maxX) maxX = cube.X;
            if (cube.Y < minY) minY = cube.Y;
            if (cube.Y > maxY) maxY = cube.Y;
            if (cube.Z < minZ) minZ = cube.Z;
            if (cube.Z > maxZ) maxZ = cube.Z;
        }
        return
            (new Position(minX - 1, minY - 1, minZ - 1),
             new Position(maxX + 1, maxY + 1, maxZ + 1));
    }

    public int CalculateSurface()
    {
        int surfaceArea = 0;
        foreach (var cube in Cubes)
        {
            if (!IsCubeAt(cube.X + 1, cube.Y, cube.Z)) surfaceArea++;
            if (!IsCubeAt(cube.X - 1, cube.Y, cube.Z)) surfaceArea++;
            if (!IsCubeAt(cube.X, cube.Y + 1, cube.Z)) surfaceArea++;
            if (!IsCubeAt(cube.X, cube.Y - 1, cube.Z)) surfaceArea++;
            if (!IsCubeAt(cube.X, cube.Y, cube.Z + 1)) surfaceArea++;
            if (!IsCubeAt(cube.X, cube.Y, cube.Z - 1)) surfaceArea++;
        }
        return surfaceArea;
    }

    public int CalculateExteriorSurfaceArea()
    {
        int surfaceArea = 0;
        int count = 0;
        foreach (var cube in Cubes)
        {
            //Console.WriteLine($"cube: {count} / {Cubes.Count}");
            count++;

            if (IsConnectedToTheOutside(new Position(cube.X + 1, cube.Y, cube.Z))) surfaceArea++;
            if (IsConnectedToTheOutside(new Position(cube.X - 1, cube.Y, cube.Z))) surfaceArea++;
            if (IsConnectedToTheOutside(new Position(cube.X, cube.Y + 1, cube.Z))) surfaceArea++;
            if (IsConnectedToTheOutside(new Position(cube.X, cube.Y - 1, cube.Z))) surfaceArea++;
            if (IsConnectedToTheOutside(new Position(cube.X, cube.Y, cube.Z + 1))) surfaceArea++;
            if (IsConnectedToTheOutside(new Position(cube.X, cube.Y, cube.Z - 1))) surfaceArea++;
        }
        return surfaceArea;
    }


    public bool IsCubeAt(Position position) =>
        IsCubeAt(position.X, position.Y, position.Z);
    public bool IsCubeAt(int x, int y, int z) =>
        Cubes.Any(cube => cube.X == x && cube.Y == y && cube.Z == z);
    public bool IsCubeAtExterior(int x, int y, int z) =>
        x <= MinimumExterior.X || y <= MinimumExterior.Y || z <= MinimumExterior.Z ||
        x >= MaximumExterior.X || y >= MaximumExterior.Y || z >= MaximumExterior.Z;

    public bool IsConnectedToTheOutside(Position position)
    {
        if (IsCubeAt(position)) return false;

        HashSet<Position> checkedPositions = new();
        HashSet<Position> positionsToCheck = new HashSet<Position>()
        {
            position
        };
        while (positionsToCheck.Count > 0)
        {
            Position currentPosition = positionsToCheck.Last();
            checkedPositions.Add(currentPosition);
            positionsToCheck.Remove(currentPosition);
            if (
                CheckPosition(positionsToCheck, checkedPositions, currentPosition.X + 1, currentPosition.Y, currentPosition.Z) ||
                CheckPosition(positionsToCheck, checkedPositions, currentPosition.X - 1, currentPosition.Y, currentPosition.Z) ||
                CheckPosition(positionsToCheck, checkedPositions, currentPosition.X, currentPosition.Y + 1, currentPosition.Z) ||
                CheckPosition(positionsToCheck, checkedPositions, currentPosition.X, currentPosition.Y - 1, currentPosition.Z) ||
                CheckPosition(positionsToCheck, checkedPositions, currentPosition.X, currentPosition.Y, currentPosition.Z + 1) ||
                CheckPosition(positionsToCheck, checkedPositions, currentPosition.X, currentPosition.Y, currentPosition.Z - 1))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckPosition(HashSet<Position> positionsToCheck, HashSet<Position> checkedPositions, int x, int y, int z)
    {
        Position nextPosition = new Position(x, y, z);
        if (IsCubeAtExterior(x, y, z))
            return true;
        else if (
            !checkedPositions.Contains(nextPosition) &&
            !positionsToCheck.Contains(nextPosition) &&
            !IsCubeAt(x, y, z))
        {
            positionsToCheck.Add(nextPosition);
        }
        return false;
    }
}

internal class Program
{
    public static void Run()
    {
        LavaCubes cubes = Load();
        int surfaceArea = cubes.CalculateSurface();
        Console.WriteLine($"Part 1: {surfaceArea}");
        int exteriorSurfaceArea = cubes.CalculateExteriorSurfaceArea();
        Console.WriteLine($"Part 2: {exteriorSurfaceArea}");
    }

    public static LavaCubes Load()
    {
        List<Position> cubes = new List<Position>();
        string file = "day18/input";
        string[] lines = File.ReadAllLines(file);
        foreach (string line in lines)
        {
            var parts = line.Split(",");
            cubes.Add(
               new Position(int.Parse(parts[0]),
                int.Parse(parts[1]),
                int.Parse(parts[2])));
        }
        LavaCubes lavaCubes = new LavaCubes(cubes);
        return lavaCubes;
    }
}
