using System.Collections;
using System.Drawing;

namespace adventofcode2022.day13;

public class PacketPair
{
    public PacketList Left { get; set; } = null!;
    public PacketList Right { get; set; } = null!;
    public int Index { get; set; }

    public bool IsValid()
    {
        //Console.WriteLine($"Index: {Index}");
        var res = Left.IsValid(Right);
        if (res != null) return res.Value;
        return true;
    }
}
public abstract class PacketBase
{

}


public class PacketList : PacketBase
{
    public List<PacketBase> Packets { get; set; } = new List<PacketBase>();

    public bool? IsValid(PacketList right)
    {
        //Console.WriteLine($"Compare {this} vs {right}");
        for (int i = 0; i < Packets.Count; i++)
        {
            if (right.Packets.Count <= i)
            {
                //Console.WriteLine($"Right hand side ran out of elements, is invalid");
                return false;
            }
            PacketList? childListLeft = Packets[i] as PacketList;
            PacketList? childListRight = right.Packets[i] as PacketList;
            PacketNumber? childNumberLeft = Packets[i] as PacketNumber;
            PacketNumber? childNumberRight = right.Packets[i] as PacketNumber;

            if (childListLeft != null && childListRight != null)
            {
                //Console.WriteLine($"Starts subcompare {childListLeft} vs {childListRight}");
                bool? res = childListLeft.IsValid(childListRight);
                if (res != null) return res;
            }
            else if (childNumberLeft != null && childNumberRight != null)
            {
                //Console.WriteLine($"Starts number compare {childNumberLeft} vs {childNumberRight}");
                if (childNumberLeft.Number > childNumberRight.Number)
                {

                    //Console.WriteLine($"Left {childNumberLeft} is bigger than {childNumberRight}, compare is invalid");
                    return false;
                }
                else if (childNumberLeft.Number < childNumberRight.Number)
                {
                    //Console.WriteLine($"Left {childNumberLeft} is lower than {childNumberRight}, pair compare is valid");
                    return true;
                }
                else
                {
                    //Console.WriteLine($"Left {childNumberLeft} equal to {childNumberRight}, continue compare");
                }
            }
            else if (childListLeft != null && childNumberRight != null)
            {
                //Console.WriteLine($"Left list {childListLeft} vs right number {childNumberRight}");
                PacketList fakeChildListRight = new PacketList();
                fakeChildListRight.Packets.Add(childNumberRight);
                //Console.WriteLine($"Converts right {childNumberRight} to {fakeChildListRight}");
                var res = childListLeft.IsValid(fakeChildListRight);
                if (res != null) return res;
            }
            else if (childNumberLeft != null && childListRight != null)
            {
                //Console.WriteLine($"Left number {childNumberLeft} vs right list {childListRight}");
                PacketList fakeChildListLeft = new PacketList();
                fakeChildListLeft.Packets.Add(childNumberLeft);
                //Console.WriteLine($"Converts left {childNumberLeft} to {fakeChildListLeft}");
                var res = fakeChildListLeft.IsValid(childListRight);
                if (res != null) return res;
            }
        }
        if (right.Packets.Count > this.Packets.Count)
        {
            //Console.WriteLine($"Left hand side ran out of numbers to compare, is valid");
            return true;
        }
        else
        {
            //Console.WriteLine($"Compare is inconclusive, continue compare");
            return null;
        }

    }

    public override string ToString()
    {
        return $"[{string.Join(",", Packets)}]";
    }
}


public class PacketNumber : PacketBase
{
    public int Number { get; set; }

    public override string ToString()
    {
        return Number.ToString();
    }
}


public class PacketComparerer : IComparer<PacketList>
{
    public int Compare(PacketList? x, PacketList? y)
    {
        var res = x.IsValid(y);
        if (res == true) return -1;
        if (res == null) return 0;
        return 1;
    }
}


public class Program
{
    public static void Run()
    {
        List<PacketPair> packets = Load();
        int sum = 0;
        for (int i = 0; i < packets.Count; i++)
        {
            //Console.WriteLine();
            if (packets[i].IsValid())
            {
                sum += packets[i].Index;
            }
        }
        Console.WriteLine($"Part 1: {sum}");


        List<PacketList> allPackages = new List<PacketList>();
        foreach (var packet in packets)
        {
            allPackages.Add(packet.Left);
            allPackages.Add(packet.Right);
        }
        PacketList divider1 = CreateDividerPacket(2);
        PacketList divider2 = CreateDividerPacket(6);
        allPackages.Add(divider1);
        allPackages.Add(divider2);
        PacketComparerer packetComparerer = new PacketComparerer();
        PacketList[] packetArray = allPackages.ToArray();
        Array.Sort(packetArray, packetComparerer);
        int divider1Index = -1;
        int divider2Index = -1;
        for (int i = 0; i < packetArray.Length; i++)
        {
            //Console.WriteLine($"Index: {i + 1}: {packetArray[i]}");
            if (packetArray[i] == divider1)
            {
                divider1Index = i + 1;
            }
            if (packetArray[i] == divider2)
            {
                divider2Index = i + 1;
            }
        }
        int part2Result = divider1Index * divider2Index;
        Console.WriteLine($"Part 2: {part2Result}");

    }

    private static PacketList CreateDividerPacket(int number)
    {
        return new PacketList()
        {
            Packets = new List<PacketBase>()
            {
                new PacketList()
                {
                    Packets = new List<PacketBase>()
                    {
                        new PacketNumber()
                        {
                            Number = number
                        }
                    }
                }
            }
        };
    }

    public static List<PacketPair> Load()
    {
        string inputFile = "day13/input";
        string[] lines = File.ReadAllLines(inputFile);
        List<PacketPair> packes = new();
        int packetIndex = 0;
        for (int i = 0; i < lines.Length / 3 + 1; i++)
        {
            string line1 = lines[i * 3];
            string line2 = lines[i * 3 + 1];
            PacketPair packet = new();
            packet.Left = new PacketList();
            packet.Right = new PacketList();
            packet.Left.Packets.AddRange(Parse(line1, 1).Item1);
            packet.Right.Packets.AddRange(Parse(line2, 1).Item1);
            packetIndex++;
            packet.Index = packetIndex;
            packes.Add(packet);
        }
        return packes;
    }

    public static (List<PacketBase>, int parsedCharacters) Parse(string line, int startIndex)
    {
        List<PacketBase> packets = new();
        string number = "";
        for (int i = startIndex; i < line.Length; i++)
        {
            if (line[i] == ',')
            {
                if (!string.IsNullOrWhiteSpace(number))
                {
                    packets.Add(new PacketNumber()
                    {
                        Number = int.Parse(number)
                    });
                }

                number = "";
            }
            else if (line[i] == '[')
            {
                PacketList list = new PacketList();
                var result = Parse(line, i + 1);
                list.Packets.AddRange(result.Item1);
                i += result.parsedCharacters + 1;
                packets.Add(list);
            }
            else if (line[i] == ']')
            {
                if (!string.IsNullOrWhiteSpace(number))
                {
                    packets.Add(new PacketNumber()
                    {
                        Number = int.Parse(number)
                    });
                    number = "";
                }
                return (packets, i - startIndex);
            }
            else
            {
                number += line[i];
            }
        }
        return (packets, line.Length - startIndex);
    }
}
