using System.Drawing;

namespace adventofcode2022.day9;

internal class Program
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    class Movement
    {
        public Direction Direction { get; set; }
        public int NumberOfSteps { get; set; }
    }

    class Robe
    {
        public int HeadX { get; private set; }
        public int HeadY { get; private set; }

        public int TailX { get; private set; }
        public int TailY { get; private set; }

        public Point Tail => new Point(TailX, TailY);
        public Point Head => new Point(HeadX, HeadY);

        public void MoveHeadUp() => HeadY++;
        public void MoveHeadDown() => HeadY--;
        public void MoveHeadRight() => HeadX++;
        public void MoveHeadLeft() => HeadX--;
        public void MoveHeadTo(int x, int y)
        {
            HeadX = x;
            HeadY = y;
        }

        public void UpdateTail()
        {

            if (IsRight(2))
            {
                if (IsAbove(1))
                    TailY++;
                if (IsBelow(1))
                    TailY--;
                TailX++;
            }
            if (IsLeft(2))
            {
                if (IsAbove(1))
                    TailY++;
                if (IsBelow(1))
                    TailY--;
                TailX--;
            }

            if (IsAbove(2))
            {
                if (IsRight(1))
                    TailX++;
                if (IsLeft(1))
                    TailX--;

                TailY++;
            }
            if (IsBelow(2))
            {
                if (IsRight(1))
                    TailX++;
                if (IsLeft(1))
                    TailX--;

                TailY--;
            }
        }

        private bool IsAbove(int distance)
        {
            return HeadY > TailY && Math.Abs(HeadY - TailY) >= distance;
        }

        private bool IsBelow(int distance)
        {
            return HeadY < TailY && Math.Abs(TailY - HeadY) >= distance;
        }

        private bool IsRight(int distance)
        {
            return HeadX > TailX && Math.Abs(HeadX - TailX) >= distance;
        }

        private bool IsLeft(int distance)
        {
            return HeadX < TailX && Math.Abs(TailX - HeadX) >= distance;
        }



    }

    class RobeSimulation
    {
        public Robe Robe { get; private set; } = new Robe();

        public HashSet<Point> VisitedPositions { get; private set; } = new HashSet<Point>();

        public void Simulate(List<Movement> movements)
        {
            UpdateVisitedPositions();

            foreach (var movement in movements)
            {
                for (int i = 0; i < movement.NumberOfSteps; i++)
                {
                    switch (movement.Direction)
                    {
                        case Direction.Up: Robe.MoveHeadUp(); break;
                        case Direction.Down: Robe.MoveHeadDown(); break;
                        case Direction.Left: Robe.MoveHeadLeft(); break;
                        case Direction.Right: Robe.MoveHeadRight(); break;
                        default: throw new Exception($"Unsupported direction: {movement.Direction}");
                    }
                    Robe.UpdateTail();
                    UpdateVisitedPositions();
                }
            }
        }

        public void UpdateVisitedPositions()
        {
            Point p = new Point()
            {
                X = Robe.TailX,
                Y = Robe.TailY
            };

            if (!VisitedPositions.Contains(p))
                VisitedPositions.Add(p);
        }
    }

    class MultiRobeSimulation
    {
        public List<Robe> Robes { get; private set; } = new();

        public HashSet<Point> VisitedPositions { get; private set; } = new HashSet<Point>();

        public void Simulate(List<Movement> movements)
        {
            for (int i = 0; i < 9; i++)
                Robes.Add(new Robe());

            UpdateVisitedPositions();

            foreach (var movement in movements)
            {
                for (int i = 0; i < movement.NumberOfSteps; i++)
                {
                    switch (movement.Direction)
                    {
                        case Direction.Up: Robes[0].MoveHeadUp(); break;
                        case Direction.Down: Robes[0].MoveHeadDown(); break;
                        case Direction.Left: Robes[0].MoveHeadLeft(); break;
                        case Direction.Right: Robes[0].MoveHeadRight(); break;
                        default: throw new Exception($"Unsupported direction: {movement.Direction}");
                    }
                    Robes[0].UpdateTail();
                    for (int j = 1; j < Robes.Count; j++)
                    {
                        Robes[j].MoveHeadTo(Robes[j - 1].TailX, Robes[j - 1].TailY);
                        Robes[j].UpdateTail();
                    }

                    UpdateVisitedPositions();
                }
            }
        }

        public void UpdateVisitedPositions()
        {
            Point p = new Point()
            {
                X = Robes[Robes.Count - 1].TailX,
                Y = Robes[Robes.Count - 1].TailY
            };

            if (!VisitedPositions.Contains(p))
                VisitedPositions.Add(p);
        }
    }

    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("- - - - - Day 9 - - - - -");

        List<Movement> movements = Load();
        RobeSimulation simulation = new RobeSimulation();
        simulation.Simulate(movements);

        MultiRobeSimulation multiRobeSimulation = new MultiRobeSimulation();
        multiRobeSimulation.Simulate(movements);

        Console.WriteLine($"Part 1: {simulation.VisitedPositions.Count}");
        Console.WriteLine($"Part 2: {multiRobeSimulation.VisitedPositions.Count}");
    }

    private static List<Movement> Load()
    {
        string inputFile = "day9/input";
        string[] lines = File.ReadAllLines(inputFile);
        List<Movement> movements = new();
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] parts = line.Split(" ");
            movements.Add(new Movement()
            {
                Direction = GetDirection(parts[0]),
                NumberOfSteps = int.Parse(parts[1])
            });
        }
        return movements;
    }

    private static Direction GetDirection(string s)
    {
        if (s == "D") return Direction.Down;
        if (s == "U") return Direction.Up;
        if (s == "L") return Direction.Left;
        if (s == "R") return Direction.Right;
        throw new Exception($"Unknown directions: {s}");
    }
}
