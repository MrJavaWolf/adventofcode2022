using System.Text;

namespace adventofcode2022.day16;

public class Valve
{
    public List<Valve> TunnelToValves { get; set; } = new();
    public int FlowRate { get; set; }
    public string Name { get; set; } = null!;
    public int Line { get; set; } = -1;

    public override string ToString()
    {
        return $"Valve {Name} has a flow rate={FlowRate}; tunnels lead to valves {string.Join(", ", TunnelToValves.Select(t => t.Name))}";
    }
}

public enum Action
{
    Move,
    Open
}
public class Path1Player
{
    public List<(Valve, Action)> Actions { get; set; } = new();

    public Valve CurrentPosition => Actions.Last().Item1;

    public int PressureReleased()
    {
        int releasedPressure = 0;
        List<Valve> ValvesOpened = new();
        for (int i = 0; i < Actions.Count; i++)
        {
            releasedPressure += ValvesOpened.Sum(x => x.FlowRate);
            (Valve, Action) action = Actions[i];
            if (action.Item2 == Action.Open)
            {
                ValvesOpened.Add(action.Item1);
            }
        }
        return releasedPressure;
    }

    public List<(Valve, Action)> GetPossibleActions()
    {
        List<(Valve, Action)> actions = new List<(Valve, Action)>();
        var currentPosition = CurrentPosition;
        if (!IsValveOpen(currentPosition))
        {
            actions.Add((currentPosition, Action.Open));
        }
        foreach (var moveToValve in currentPosition.TunnelToValves)
        {
            actions.Add((moveToValve, Action.Move));
        }
        return actions;
    }

    public bool IsValveOpen(Valve valve)
    {
        return Actions.Any(x => x.Item2 == Action.Open && x.Item1 == valve);
    }

    public Path1Player Copy()
    {
        return new Path1Player()
        {
            Actions = Actions.ToList()
        };
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        int releasedPressure = 0;
        List<Valve> ValvesOpened = new();
        for (int i = 0; i < Actions.Count; i++)
        {
            stringBuilder.Append($"===== Minut {i} ===== ");
            int pressureReleasedThisMinut = ValvesOpened.Sum(x => x.FlowRate);
            releasedPressure += pressureReleasedThisMinut;
            (Valve, Action) action = Actions[i];

            if (action.Item2 == Action.Open)
            {
                stringBuilder.Append($"Opens Valve: {action.Item1}. ");
                ValvesOpened.Add(action.Item1);
            }
            else
            {
                stringBuilder.Append($"Moves to Valve: {action.Item1}. ");
            }
            stringBuilder.AppendLine($"Presure released this minut: {pressureReleasedThisMinut}, Total pressure released: {releasedPressure}");
        }
        return stringBuilder.ToString();
    }
}

public class Path2Player
{
    public List<(Valve, Action)> Actions1 { get; set; } = new();
    public List<(Valve, Action)> Actions2 { get; set; } = new();

    public Valve CurrentPositionPlayer1 => Actions1.Last().Item1;
    public Valve CurrentPositionPlayer2 => Actions2.Last().Item1;

    public int NumberOfOpenValves
    {
        get
        {
            HashSet<Valve> ValvesOpened = new();
            for (int i = 0; i < Actions1.Count; i++)
            {
                (Valve, Action) action1 = Actions1[i];
                (Valve, Action) action2 = Actions2[i];
                if (action1.Item2 == Action.Open && !ValvesOpened.Contains(action1.Item1))
                    ValvesOpened.Add(action1.Item1);
                if (action2.Item2 == Action.Open && !ValvesOpened.Contains(action2.Item1))
                    ValvesOpened.Add(action2.Item1);
            }
            return ValvesOpened.Count;
        }
    }

    public int PressureReleasedPerMinut
    {
        get
        {
            HashSet<Valve> ValvesOpened = new();
            for (int i = 0; i < Actions1.Count; i++)
            {
                (Valve, Action) action1 = Actions1[i];
                (Valve, Action) action2 = Actions2[i];
                if (action1.Item2 == Action.Open && !ValvesOpened.Contains(action1.Item1))
                    ValvesOpened.Add(action1.Item1);
                if (action2.Item2 == Action.Open && !ValvesOpened.Contains(action2.Item1))
                    ValvesOpened.Add(action2.Item1);
            }
            return ValvesOpened.Sum(x => x.FlowRate);
        }
    }

    public int PressureReleased()
    {
        int releasedPressure = 0;
        HashSet<Valve> ValvesOpened = new();
        for (int i = 0; i < Actions1.Count; i++)
        {
            releasedPressure += ValvesOpened.Sum(x => x.FlowRate);
            (Valve, Action) action1 = Actions1[i];
            (Valve, Action) action2 = Actions2[i];
            if (action1.Item2 == Action.Open && !ValvesOpened.Contains(action1.Item1))
                ValvesOpened.Add(action1.Item1);
            if (action2.Item2 == Action.Open && !ValvesOpened.Contains(action2.Item1))
                ValvesOpened.Add(action2.Item1);
        }
        return releasedPressure;
    }

    public List<(Valve, Action)> GetPossibleActionsPlayer1()
    {
        List<(Valve, Action)> actions = new List<(Valve, Action)>();
        var currentPositionPlayer1 = CurrentPositionPlayer1;
        if (currentPositionPlayer1.FlowRate > 0 && !IsValveOpen(currentPositionPlayer1))
        {
            actions.Add((currentPositionPlayer1, Action.Open));
        }
        foreach (var moveToValve in currentPositionPlayer1.TunnelToValves)
        {
            if (!Player1CameFrom(moveToValve))
            {
                actions.Add((moveToValve, Action.Move));
            }
        }
        return actions;
    }

    public bool Player1CameFrom(Valve valve)
    {
        for (int i = Actions1.Count - 1; i >= 0; i--)
        {
            if (Actions1[i].Item2 != Action.Move)
                return false;
            if (Actions1[i].Item1 == valve)
                return true;
        }
        return false;
    }

    public bool Player2CameFrom(Valve valve)
    {
        for (int i = Actions2.Count - 1; i >= 0; i--)
        {
            if (Actions2[i].Item2 != Action.Move)
                return false;
            if (Actions2[i].Item1 == valve)
                return true;
        }
        return false;
    }

    public List<(Valve, Action)> GetPossibleActionsPlayer2()
    {
        List<(Valve, Action)> actions = new List<(Valve, Action)>();
        var currentPositionPlayer2 = CurrentPositionPlayer2;
        if (currentPositionPlayer2.FlowRate > 0 && !IsValveOpen(currentPositionPlayer2))
        {
            actions.Add((currentPositionPlayer2, Action.Open));
        }
        foreach (var moveToValve in currentPositionPlayer2.TunnelToValves)
        {
            if (!Player2CameFrom(moveToValve))
            {
                actions.Add((moveToValve, Action.Move));
            }
        }
        return actions;
    }

    public bool IsValveOpen(Valve valve)
    {
        return
            Actions1.Any(x => x.Item2 == Action.Open && x.Item1 == valve) ||
            Actions2.Any(x => x.Item2 == Action.Open && x.Item1 == valve);
    }

    public bool AreAllValvesWithFlowOpen(List<Valve> valves)
    {
        foreach (var valve in valves)
        {
            if (valve.FlowRate > 0 && !IsValveOpen(valve))
            {
                return false;
            }
        }
        return true;
    }


    public Path2Player Copy()
    {
        return new Path2Player()
        {
            Actions1 = Actions1.ToList(),
            Actions2 = Actions2.ToList()
        };
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        int releasedPressure = 0;
        List<Valve> ValvesOpened = new();
        for (int i = 0; i < Actions1.Count; i++)
        {
            stringBuilder.Append($"===== Minut {i} ===== ");
            int pressureReleasedThisMinut = ValvesOpened.Sum(x => x.FlowRate);
            releasedPressure += pressureReleasedThisMinut;
            (Valve, Action) action = Actions1[i];

            if (action.Item2 == Action.Open)
            {
                stringBuilder.Append($"Opens Valve: {action.Item1}. ");
                ValvesOpened.Add(action.Item1);
            }
            else
            {
                stringBuilder.Append($"Moves to Valve: {action.Item1}. ");
            }
            stringBuilder.AppendLine($"Presure released this minut: {pressureReleasedThisMinut}, Total pressure released: {releasedPressure}");
        }
        return stringBuilder.ToString();
    }
}

internal class Program
{
    public static void Run()
    {
        List<Valve> valves = Load();
        foreach (Valve valve in valves)
        {
            Console.WriteLine(valve.ToString());
        }

        List<Path1Player> part1ActivePaths = Part1(valves);
        int mostPossiblePressureReleasedPart1 = part1ActivePaths.Max(x => x.PressureReleased());
        var bestPaths = part1ActivePaths.Where(x => x.PressureReleased() == mostPossiblePressureReleasedPart1).ToList();
        Console.WriteLine($"Part 1: {mostPossiblePressureReleasedPart1}");
        int mostPossiblePressureReleasedPart2 = Part2(valves);
        Console.WriteLine($"Part 2: {mostPossiblePressureReleasedPart2}");
   
    }

    private static List<Path1Player> Part1(List<Valve> valves)
    {
        List<Path1Player> activePaths = new();
        Path1Player startPosition = new Path1Player();
        var startValve = valves.Where(x => x.Name == "AA").First();
        startPosition.Actions.Add((startValve, Action.Move));
        activePaths.Add(startPosition);
        for (int i = 0; i < 30; i++)
        {
            int maxPressureReleased = activePaths.Max(x => x.PressureReleased());
            Console.WriteLine($"Step: {(i + 1)}, number of possible actions: {activePaths.Count}, most pressure released: {maxPressureReleased}");
            List<Path1Player> newActivePaths = new();
            for (int j = 0; j < activePaths.Count; j++)
            {
                Path1Player p = activePaths[j];
                int pressureReleased = p.PressureReleased();
                var possibleActions = p.GetPossibleActions();
                if (pressureReleased < maxPressureReleased - 50)
                {
                    continue;
                }
                foreach (var possibleAction in possibleActions)
                {
                    Path1Player newPath = p.Copy();
                    newPath.Actions.Add(possibleAction);
                    newActivePaths.Add(newPath);
                }
            }
            activePaths = newActivePaths;
        }

        return activePaths;
    }

    private static int Part2(List<Valve> valves)
    {
        List<Path2Player> activePaths = new();
        Path2Player startPosition = new Path2Player();
        var startValve = valves.Where(x => x.Name == "AA").First();
        startPosition.Actions1.Add((startValve, Action.Move));
        startPosition.Actions2.Add((startValve, Action.Move));
        activePaths.Add(startPosition);
        for (int i = 0; i < 26; i++)
        {
            int maxPressureReleased = activePaths.Max(x => x.PressureReleased());
            Console.WriteLine($"Step: {i}, number of possible actions: {activePaths.Count}, most pressure released: {maxPressureReleased}");
            List<Path2Player> newActivePaths = new();
            for (int j = 0; j < activePaths.Count; j++)
            {
                Path2Player p = activePaths[j];
                int pressureReleased = p.PressureReleased();
                if (p.AreAllValvesWithFlowOpen(valves) && pressureReleased == maxPressureReleased)
                {
                    Console.WriteLine("All Valves have been opened, predicts the rest!");
                    int totalPressureReleased = pressureReleased + p.PressureReleasedPerMinut * (26 - i);
                    return totalPressureReleased;
                }
                if (pressureReleased < maxPressureReleased - 75)
                {
                    continue;
                }

                var possibleActionsPlayer1 = p.GetPossibleActionsPlayer1();
                var possibleActionsPlayer2 = p.GetPossibleActionsPlayer2();
                foreach (var possibleActionPlayer1 in possibleActionsPlayer1)
                {
                    foreach (var possibleActionPlayer2 in possibleActionsPlayer2)
                    {
                        Path2Player newPath = p.Copy();
                        newPath.Actions1.Add(possibleActionPlayer1);
                        newPath.Actions2.Add(possibleActionPlayer2);
                        newActivePaths.Add(newPath);
                    }
                }
            }
            activePaths = newActivePaths;
        }

        return activePaths.Max(x => x.PressureReleased());
    }

    public static List<Valve> Load()
    {
        string inputFile = "day16/input";
        string[] lines = File.ReadAllLines(inputFile);
        Dictionary<string, Valve> valves = new Dictionary<string, Valve>();
        int lineNumber = 0;
        foreach (string line in lines)
        {
            string[] parts = line.Split(" ");
            string name = parts[1];
            string flowRateString = parts[4];
            flowRateString = flowRateString.Replace("rate=", "").Replace(";", "");
            int flowRate = int.Parse(flowRateString);
            List<string> connectedValves = new List<string>();
            for (int i = 9; i < parts.Length; i++)
            {
                string connectedValve = parts[i];
                connectedValve = connectedValve.Replace(",", "");
                connectedValves.Add(connectedValve);
            }
            Valve valve;
            if (valves.ContainsKey(name))
                valve = valves[name];
            else
                valve = new Valve();

            valve.Line = ++lineNumber;
            valve.Name = name;
            valve.FlowRate = flowRate;
            foreach (string connectedValveName in connectedValves)
            {
                Valve connectedValve;
                if (valves.ContainsKey(connectedValveName))
                    connectedValve = valves[connectedValveName];
                else
                    connectedValve = new Valve();
                connectedValve.Name = connectedValveName;
                valve.TunnelToValves.Add(connectedValve);

                if (!valves.ContainsKey(connectedValveName))
                {
                    valves.Add(connectedValveName, connectedValve);
                }
            }
            if (!valves.ContainsKey(name))
            {
                valves.Add(name, valve);
            }
        }

        return valves.Values.OrderBy(x => x.Line).ToList();
    }
}
