namespace adventofcode2022.day7;


internal class Program
{

    class ElveFolder
    {
        public string Name { get; set; } = "";

        public ElveFolder? Parent { get; set; }
        public Dictionary<string, ElveFolder> SubFolders { get; set; } = new();
        public Dictionary<string, ElveFile> Files { get; set; } = new();

        public long CalculateSize()
        {
            long size = 0;
            foreach (var file in Files)
            {
                size += file.Value.Size;
            }

            foreach (var folder in SubFolders)
            {
                size += folder.Value.CalculateSize();
            }
            return size;
        }

        public void AddIfAtMost100000(List<ElveFolder> folders)
        {
            if (CalculateSize() <= 100000)
            {
                folders.Add(this);
            }
            foreach (var subfolder in SubFolders)
            {
                subfolder.Value.AddIfAtMost100000(folders);
            }
        }
        public void AddIfAtLeast(List<ElveFolder> folders, long size)
        {
            if (CalculateSize() >= size)
            {
                folders.Add(this);
            }
            foreach (var subfolder in SubFolders)
            {
                subfolder.Value.AddIfAtLeast(folders, size);
            }
        }
    }

    class ElveFile
    {
        public string Name { get; set; } = "";
        public long Size { get; set; }
    }

    public static void Run()
    {
        ElveFolder root = Load();
        List<ElveFolder> smallElveFolders = new List<ElveFolder>();
        root.AddIfAtMost100000(smallElveFolders);
        long sizeOfAtMost100000 = smallElveFolders.Sum(x => x.CalculateSize());
        Console.WriteLine($"Part 1: {sizeOfAtMost100000}");

        long totalSize = root.CalculateSize();
        long availableSpace = 70000000;
        long updateSize = 30000000; 
        long spaceNeeded = -((availableSpace - totalSize) - updateSize);
        Console.WriteLine($"Total size: {totalSize}");
        Console.WriteLine($"Space Needed: {spaceNeeded}");
        List<ElveFolder> bigElveFolders = new List<ElveFolder>();
        root.AddIfAtLeast(bigElveFolders, spaceNeeded);
        bigElveFolders = bigElveFolders.OrderBy(x => x.CalculateSize()).ToList();
        Console.WriteLine($"Part 2: {bigElveFolders[0].CalculateSize()}");
    }

    static ElveFolder Load()
    {
        string file = "day7/input";
        string[] lines = File.ReadAllLines(file);

        ElveFolder rootFolder = new ElveFolder()
        {
            Name = "/",
        };
        ElveFolder currentFolder = rootFolder;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (parts[0] == "$")
            {
                // is a command
                if (parts[1] == "cd")
                {
                    // Change directory
                    if (parts[2] == "..")
                    {
                        if (currentFolder.Parent != null)
                        {
                            currentFolder = currentFolder.Parent;
                        }
                        else
                        {
                            throw new Exception($"Folder '{currentFolder.Name}' does not have a parent");
                        }
                    }
                    else if (parts[2] == "/")
                    {
                        currentFolder = rootFolder;
                    }
                    else
                        currentFolder = currentFolder.SubFolders[parts[2]];
                }
                else if (parts[1] == "ls")
                {
                    // Ignore
                }
            }
            else if (parts[0] == "dir")
            {

                currentFolder.SubFolders.Add(
                    parts[1],
                    new ElveFolder()
                    {
                        Name = parts[1],
                        Parent = currentFolder,
                    });
            }
            else if (long.TryParse(parts[0], out long size))
            {
                currentFolder.Files.Add(
                       parts[1],
                       new ElveFile()
                       {
                           Name = parts[1],
                           Size = size
                       });
            }
        }
        return rootFolder;
    }
}
