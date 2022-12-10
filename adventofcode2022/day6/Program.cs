namespace adventofcode2022.day6;

internal class Program
{
    const string example1 = "mjqjpqmgbljsphdztnvjfqwrcgsmlb";
    const string example2 = "bvwbjplbgvbhsrlpgdmjqwftvncz";
    const string example3 = "nppdvjthqldpwncqszvftbrmjlhg";
    const string example4 = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg";
    const string example5 = "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw";

    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("- - - - - Day 6 - - - - -");

        string message = Load();
        //Console.WriteLine($"example1: {FindStartMarker(example1, 4)}, expected: 7");
        //Console.WriteLine($"example2: {FindStartMarker(example2, 4)}, expected: 5");
        //Console.WriteLine($"example3: {FindStartMarker(example3, 4)}, expected: 6");
        //Console.WriteLine($"example4: {FindStartMarker(example4, 4)}, expected: 10");
        //Console.WriteLine($"example5: {FindStartMarker(example5, 4)}, expected: 11");
        Console.WriteLine($"Part 1: {FindStartMarker(message, 4)}");

        //Console.WriteLine($"example1: {FindStartMarker(example1, 14)}, expected: 19");
        //Console.WriteLine($"example2: {FindStartMarker(example2, 14)}, expected: 23");
        //Console.WriteLine($"example3: {FindStartMarker(example3, 14)}, expected: 23");
        //Console.WriteLine($"example4: {FindStartMarker(example4, 14)}, expected: 29");
        //Console.WriteLine($"example5: {FindStartMarker(example5, 14)}, expected: 26");
        Console.WriteLine($"Part 2: {FindStartMarker(message, 14)}");
    }

    static int FindStartMarker(string message, int uniqueCharacters)
    {
        HashSet<char> chars = new HashSet<char>();
        for (int i = 0; i < message.Length; i++)
        {
            chars.Clear();
            bool foundStartMarker = true;
            for (int j = 0; j < uniqueCharacters; j++)
            {
                if (!chars.Contains(message[i + j]))
                    chars.Add(message[i + j]);
                else
                {
                    foundStartMarker = false;
                    break;
                }
            }
            if (foundStartMarker)
            {
                return i + uniqueCharacters;
            }
        }
        return -1;
    }

    static string Load()
    {
        string file = "day6/input";
        string[] lines = File.ReadAllLines(file);
        string message = lines[0];
        return message;
    }
}
