namespace adventofcode2022.day16;

public class Valve
{
    public List<Valve> TunnelToValves { get; set; } = new();
    public int FlowRate { get; set; }
    public string Name { get; set; } = null!;
    public int Line { get; set; } = -1;

    public override string ToString()
    {
        return $"Line {Line}: Valve {Name} has a flow rate={FlowRate}; tunnels lead to valves {string.Join(", ", TunnelToValves.Select(t => t.Name))}";
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
    }

    public static List<Valve> Load()
    {
        string inputFile = "day16/input-example";
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
