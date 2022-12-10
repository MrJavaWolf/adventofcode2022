namespace adventofcode2022.day8;

internal class Program
{

    class TreeGrid
    {
        public int[,] Trees { get; set; }

        public TreeGrid(int width, int height)
        {
            Trees = new int[width, height];
        }

        public bool IsTreeVisible(int x, int y)
        {
            // Trees are visible on the edge
            if (x == 0 || y == 0 || x + 1 == Trees.GetLength(0) || y + 1 == Trees.GetLength(1))
                return true;

            int treeHeight = Trees[x, y];

            if (AreTreesSmallerThan(TreesToTheLeft(x, y), treeHeight) ||
                AreTreesSmallerThan(TreesAbove(x, y), treeHeight) ||
                AreTreesSmallerThan(TreesToTheRight(x, y), treeHeight) ||
                AreTreesSmallerThan(TreesBelow(x, y), treeHeight))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CalculaScenicScore(int x, int y)
        {
            // Trees are visible on the edge
            int treeHeight = Trees[x, y];

            int score =
                CountTreesVisible(TreesToTheLeft(x, y), treeHeight) *
                CountTreesVisible(TreesAbove(x, y), treeHeight) *
                CountTreesVisible(TreesToTheRight(x, y), treeHeight) *
                CountTreesVisible(TreesBelow(x, y), treeHeight);
            return score;

        }


        private bool AreTreesSmallerThan(IEnumerable<int> trees, int treeHeight) => trees.All(t => t < treeHeight);
        private int CountTreesVisible(IEnumerable<int> trees, int treeHeight)
        {
            int numberOfVisibleTrees = 0;
            foreach (var height in trees)
            {
                numberOfVisibleTrees++;
                if (height >= treeHeight) break;
            }
            return numberOfVisibleTrees;
        }

        private IEnumerable<int> TreesToTheLeft(int x, int y) => GetTreeHeightsX(CreateRange(x - 1, 0), y);
        private IEnumerable<int> TreesAbove(int x, int y) => GetTreeHeightsY(CreateRange(y - 1, 0), x);
        private IEnumerable<int> TreesToTheRight(int x, int y) => GetTreeHeightsX(CreateRange(x + 1, Trees.GetLength(0) - 1), y);
        private IEnumerable<int> TreesBelow(int x, int y) => GetTreeHeightsY(CreateRange(y + 1, Trees.GetLength(0) - 1), x);

        public List<int> GetTreeHeightsX(List<int> treeXs, int y)
        {
            List<int> heights = new List<int>();
            foreach (var x in treeXs)
            {
                if (x < 0 || y < 0 || x >= Trees.GetLength(0) || y >= Trees.GetLength(1)) continue;
                heights.Add(this.Trees[x, y]);
            }
            return heights;
        }

        public List<int> GetTreeHeightsY(List<int> treeYs, int x)
        {
            List<int> heights = new List<int>();
            foreach (var y in treeYs)
            {
                if (x < 0 || y < 0 || x >= Trees.GetLength(0) || y >= Trees.GetLength(1)) continue;
                heights.Add(this.Trees[x, y]);
            }
            return heights;
        }

        public List<int> CreateRange(int from, int to)
        {
            List<int> range = new List<int>();
            if (from > to)
            {
                for (int i = from; i >= to; i--)
                {
                    range.Add(i);
                }
            }
            else
            {
                for (int i = from; i <= to; i++)
                {
                    range.Add(i);
                }
            }
            return range;
        }
    }

    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("- - - - - Day 8 - - - - -");

        TreeGrid trees = Load();
        int numberOfTreesVisible = 0;
        int bestScenicScore = 0;
        for (int y = 0; y < trees.Trees.GetLength(1); y++)
        {
            for (int x = 0; x < trees.Trees.GetLength(0); x++)
            {
                bool isVisible = trees.IsTreeVisible(x, y);
                if (isVisible) numberOfTreesVisible++;
                int scenicScore = trees.CalculaScenicScore(x, y);
                if (scenicScore > bestScenicScore)
                    bestScenicScore = scenicScore;
            }
        }
        Console.WriteLine($"Part 1: {numberOfTreesVisible}");
        Console.WriteLine($"Part 2: {bestScenicScore}");
    }

    private static TreeGrid Load()
    {
        string inputFile = "day8/input";
        string[] lines = File.ReadAllLines(inputFile);
        TreeGrid treeGrid = new(lines.Length, lines[0].Length);
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                char c = line[x];
                treeGrid.Trees[x, y] = int.Parse(c.ToString());
            }
        }
        return treeGrid;
    }
}
