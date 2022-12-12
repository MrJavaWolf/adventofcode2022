namespace adventofcode2022.day12
{
    public class Node
    {
        // Change this depending on what the desired size is for each element in the grid
        public Node? Parent;
        public int PositionX;
        public int PositionY;
        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public char Elevation;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public Node(int positionX, int positionY, char type)
        {
            Parent = null;
            PositionX = positionX;
            PositionY = positionY;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = 1;
            Elevation = type;
        }

        public override string ToString()
        {
            return $"({PositionX},{PositionY}) {Elevation}";
        }
    }

    public class Astar
    {
        Node[,] Grid;
        int GridCols => Grid.GetLength(0);
        int GridRows => Grid.GetLength(1);

        public Astar(char[,] grid)
        {
            Grid = new Node[grid.GetLength(0), grid.GetLength(1)];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    char elevation = grid[x, y];
                    if (elevation == 'S') elevation = 'a';
                    if (elevation == 'E') elevation = 'z';
                    Grid[x, y] = new Node(x, y, elevation);
                }
            }
        }

        public Stack<Node>? FindPath(int startX, int startY, int endX, int endY)
        {

            Node start = Grid[startX, startY];
            Node end = Grid[endX, endY];

            Stack<Node> Path = new Stack<Node>();
            //PriorityQueue<Node, float> OpenList = new PriorityQueue<Node, float>();
            Queue<Node> OpenList = new Queue<Node>();
            List<Node> ClosedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;

            // add start node to Open List
            //OpenList.Enqueue(start, start.F);
            OpenList.Enqueue(start);

            while (OpenList.Count != 0 && !ClosedList.Exists(x => x.PositionX == end.PositionX && x.PositionY == end.PositionY))
            {
                //current = OpenList.Dequeue();
                current = OpenList.Dequeue();
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

                foreach (Node adjacentNode in adjacencies)
                {
                    if (!ClosedList.Contains(adjacentNode))
                    {

                        adjacentNode.Parent = current;
                        adjacentNode.DistanceToTarget = Math.Abs(adjacentNode.PositionX - end.PositionX) + Math.Abs(adjacentNode.PositionY - end.PositionY);
                        adjacentNode.Cost = adjacentNode.Weight + current.Cost;
                        //OpenList.Enqueue(adjacentNode, adjacentNode.F);
                        if (!OpenList.Contains(adjacentNode))
                            OpenList.Enqueue(adjacentNode);
                    }
                    else
                    {
                        if (adjacentNode.Cost > adjacentNode.Weight + current.Cost)
                        {
                            adjacentNode.Parent = current;
                            adjacentNode.Cost = adjacentNode.Weight + current.Cost;
                            //OpenList.Enqueue(adjacentNode, adjacentNode.F);
                            if (!OpenList.Contains(adjacentNode))
                                OpenList.Enqueue(adjacentNode);
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!ClosedList.Exists(x => x.PositionX == end.PositionX && x.PositionY == end.PositionY))
            {
                return null;
            }

            // if all good, return path
            Node? temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null);
            return Path;
        }

        private List<Node> GetAdjacentNodes(Node n)
        {
            List<Node> temp = new List<Node>();

            int row = n.PositionY;
            int col = n.PositionX;

            if (row + 1 < GridRows)
            {
                temp.Add(Grid[col, row + 1]);
            }
            if (row - 1 >= 0)
            {
                temp.Add(Grid[col, row - 1]);
            }
            if (col - 1 >= 0)
            {
                temp.Add(Grid[col - 1, row]);
            }
            if (col + 1 < GridCols)
            {
                temp.Add(Grid[col + 1, row]);
            }

            // Check if we can walk to the Adjacent Node
            for (int i = 0; i < temp.Count; i++)
            {
                if (!CanWalk(n, temp[i]))
                {
                    temp.RemoveAt(i);
                    i--;
                }
            }
            return temp;
        }

        private bool CanWalk(Node from, Node to)
        {
            return to.Elevation - from.Elevation <= 1;
        }
    }
}
