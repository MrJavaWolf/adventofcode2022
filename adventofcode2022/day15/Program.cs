using System.Numerics;

namespace adventofcode2022.day15;

public class Beacon
{
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}

public class Sensor
{
    public int X { get; set; }
    public int Y { get; set; }
    public override string ToString()
    {
        return $"({X},{Y})";
    }
}

public class ScanResult
{
    public Beacon Beacon { get; } = null!;
    public Sensor Sensor { get; } = null!;

    public ScanResult(Beacon beacon, Sensor sensor)
    {
        Beacon = beacon;
        Sensor = sensor;
        SensorToBeaconDistance = Math.Abs(Sensor.X - Beacon.X) + Math.Abs(Sensor.Y - Beacon.Y);
    }

    public int SensorToBeaconDistance { get; private set; }


    public void LineLineIntersection(
        BigInteger a1X, BigInteger a1Y, BigInteger a2X, BigInteger a2Y,
        BigInteger b1X, BigInteger b1Y, BigInteger b2X, BigInteger b2Y,
        out int hitX, out int hitY)
    {
        // determinant
        BigInteger d = (a1X - a2X) * (b1Y - b2Y) - (a1Y - b2Y) * (b1X - b2X);
        BigInteger px = (a1X * a2Y - a1Y * a2X) * (b1X - b2X) - (a1X - a2X) * (b1X * b2Y - b1Y * b2X);
        BigInteger py = (a1X * a2Y - a1Y * a2X) * (b1Y - b2Y) - (a1Y - a2Y) * (b1X * b2Y - b1Y * b2X);
        hitX = (int)(px / d);
        hitY = (int)(py / d);
    }

    // Find the points of intersection.
    public int FindLineCircleIntersections(
        double cx,
        double cy,
        double radius,
        double lineStartX,
        double lineStartY,
        double lineEndX,
        double lineEndY,
        out int out1X,
        out int out1Y,
        out int out2X,
        out int out2Y)
    {
        double dx, dy, A, B, C, det, t;

        dx = lineEndX - lineStartX;
        dy = lineEndY - lineStartY;

        A = dx * dx + dy * dy;
        B = 2 * (dx * (lineStartX - cx) + dy * (lineStartY - cy));
        C = (lineStartX - cx) * (lineStartX - cx) + (lineStartY - cy) * (lineStartY - cy) - radius * radius;

        det = B * B - 4 * A * C;
        if ((A <= 0.0000001) || (det < 0))
        {
            // No real solutions.
            out1X = int.MinValue;
            out1Y = int.MinValue;
            out2X = int.MinValue;
            out2Y = int.MinValue;
            return 0;
        }
        else if (det == 0)
        {
            // One solution.
            t = -B / (2 * A);
            out1X = (int)(lineStartX + t * dx);
            out1Y = (int)(lineStartY + t * dy);
            out2X = int.MinValue;
            out2Y = int.MinValue;
            return 1;
        }
        else
        {
            // Two solutions.
            t = (double)((-B + Math.Sqrt(det)) / (2 * A));
            out1X = (int)(lineStartX + t * dx);
            out1Y = (int)(lineStartY + t * dy);
            t = (double)((-B - Math.Sqrt(det)) / (2 * A));
            out2X = (int)(lineStartX + t * dx);
            out2Y = (int)(lineStartY + t * dy);
            return 2;
        }
    }

    public bool IsWithinSensorRange(int x, int y)
    {
        int distanceToPoint = Math.Abs(Sensor.X - x) + Math.Abs(Sensor.Y - y);
        return distanceToPoint <= SensorToBeaconDistance;
    }


    public int GetEstimateLowestXValue()
    {
        return Sensor.X - SensorToBeaconDistance;
    }


    public int GetEstimateHigestXValue()
    {
        return Sensor.X + SensorToBeaconDistance;
    }

    public override string ToString()
    {
        return $"Sensor: {Sensor}, Beacon: {Beacon}, Distance: {SensorToBeaconDistance}";
    }
}
public class Program
{
    public static void Run()
    {
        List<ScanResult> scanResults = Load();
        int minX = scanResults.Min(x => x.GetEstimateLowestXValue());
        int maxX = scanResults.Max(x => x.GetEstimateHigestXValue());
        int scanY = 2000000;
        HashSet<int> positionsNotContainingABeacon = new HashSet<int>();
        for (int x = minX; x <= maxX; x++)
        {
            if (scanResults.Any(s => s.Beacon.X == x && s.Beacon.Y == scanY))
            {
                continue;
            }

            foreach (ScanResult scanResult in scanResults)
            {
                if (scanResult.IsWithinSensorRange(x, scanY))
                {
                    if (!positionsNotContainingABeacon.Contains(x))
                    {
                        positionsNotContainingABeacon.Add(x);
                    }
                }
            }
        }

        Console.WriteLine($"Part 1: {positionsNotContainingABeacon.Count}");

        for (int x = 0; x <= 4000000; x++)
        {
            //if (x % 100000 == 0)
                //Console.WriteLine($"x: {x}");
            for (int y = 0; y <= 4000000; y++)
            {
                bool foundScanResult = false;
                foreach (ScanResult scanResult in scanResults)
                {
                    if (scanResult.IsWithinSensorRange(x, y))
                    {
                        foundScanResult = true;
                        int a1X = x < scanResult.Sensor.X ?
                            scanResult.Sensor.X - scanResult.SensorToBeaconDistance :
                            scanResult.Sensor.X + scanResult.SensorToBeaconDistance;

                        scanResult.LineLineIntersection(
                            a1X,
                            scanResult.Sensor.Y,
                            scanResult.Sensor.X,
                            scanResult.Sensor.Y + scanResult.SensorToBeaconDistance,
                            x,
                            0,
                            x,
                            4000000,
                            out int hitX,
                            out int hitY);

                        int prevY = y;
                        y = Math.Max(y, hitY);
                        if (y - prevY == 0)
                        {
                            //Console.WriteLine($"y increase: {(y - prevY)}");
                        }

                    }
                }

                if (!foundScanResult)
                {

                    BigInteger tuningFrequency = (BigInteger)x * (BigInteger)4000000 + y;
                    Console.WriteLine($"Part 2: {tuningFrequency} ({x},{y})");
                }
            }
        }
    }



    public static List<ScanResult> Load()
    {
        List<ScanResult> scanResults = new List<ScanResult>();
        string inputFile = "day15/input";
        string[] lines = File.ReadAllLines(inputFile);

        foreach (string line in lines)
        {
            var parts = line.Split(" ");
            string xSensorPart = parts[2];
            string ySensorPart = parts[3];
            string xBeaconPart = parts[8];
            string yBeaconPart = parts[9];

            ScanResult scanResult = new ScanResult(
                new Beacon()
                {
                    X = int.Parse(CleanInput(xBeaconPart)),
                    Y = int.Parse(CleanInput(yBeaconPart)),
                }, new Sensor()
                {
                    X = int.Parse(CleanInput(xSensorPart)),
                    Y = int.Parse(CleanInput(ySensorPart)),
                });
            scanResults.Add(scanResult);
        }
        return scanResults;
    }

    private static string CleanInput(string part)
    {
        return part
            .Replace("y=", "")
            .Replace("x=", "")
            .Replace(",", "")
            .Replace(":", "")
            .Trim();
    }
}
