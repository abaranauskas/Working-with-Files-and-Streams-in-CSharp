using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace DataProcesor
{
    class Program
    {
        //private static ConcurrentDictionary<string, string> FilesToProcess =
        //    new ConcurrentDictionary<string, string>();

        private static MemoryCache FilesToProcess = MemoryCache.Default;

        static void Main(string[] args)
        {
            Console.WriteLine("Parsing comman line options");

            var directoryToWatch = args[0];

            if (!Directory.Exists(directoryToWatch))
                Console.WriteLine($"ERROR: Direcctory {directoryToWatch} does not exist.");
            else
            {
                Console.WriteLine($"Watcing Direcctory {directoryToWatch} for changes.");

                ProcessExistingFiles(directoryToWatch);

                using (var inputFileWathcer = new FileSystemWatcher(directoryToWatch))
                //using (var timer = new Timer(ProcessFiles, null, 0, 1000))
                {
                    inputFileWathcer.IncludeSubdirectories = false;
                    inputFileWathcer.InternalBufferSize = 32768; //32kb default 8kb
                    //inputFileWathcer.Filter = "*.txt"; // default "*.*"
                    inputFileWathcer.NotifyFilter =
                        NotifyFilters.FileName | NotifyFilters.LastWrite;

                    inputFileWathcer.Created += FileCreted;
                    inputFileWathcer.Changed += FileChanged;
                    inputFileWathcer.Deleted += FileDeleted;
                    inputFileWathcer.Renamed += FileRenamed;
                    inputFileWathcer.Error += WacherError;

                    inputFileWathcer.EnableRaisingEvents = true;
                    Console.WriteLine("\nPress enter to quit.");
                    Console.ReadLine();
                }
            }

            //ProcessFilesOrDirectoriesofFiles(args);           
        }

        private static void ProcessExistingFiles(string directoryToWatch)
        {
            Console.WriteLine($"Checking {directoryToWatch} for existing files.");

            foreach (var filePath in Directory.EnumerateFiles(directoryToWatch))
            {
                Console.WriteLine($"   - Found {filePath}");
                AddToCache(filePath);
            }
        }

        //private static void ProcessFiles(object state)
        //{
        //    foreach (var file in FilesToProcess)
        //    {
        //        if (FilesToProcess.TryRemove(file.Key, out _))
        //        {
        //            var procesor = new FileProcessor(file.Key);
        //            procesor.Process();
        //        }
        //    }
        //}

        private static void FileCreted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File created: {e.Name} - type: {e.ChangeType}");

            //var procesor = new FileProcessor(e.FullPath);
            //procesor.Process();

            //FilesToProcess.TryAdd(e.FullPath, e.FullPath);
            AddToCache(e.FullPath);
        }

        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath, fullPath);

            var policy = new CacheItemPolicy
            {
                RemovedCallback = ProcessFile,
                SlidingExpiration = TimeSpan.FromSeconds(2) //jei nebuvo pasiekiamas 2 sekundes bus pashalintas
            };

            FilesToProcess.Add(item, policy);
        }

        private static void ProcessFile(CacheEntryRemovedArguments arguments)
        {
            Console.WriteLine($"* cache item removes: {arguments.CacheItem.Key} because {arguments.RemovedReason}");

            if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var procesor = new FileProcessor(arguments.CacheItem.Key);
                procesor.Process();
            }
            else
            {
                Console.WriteLine($"Warning: {arguments.CacheItem.Key} removed unexpectedly.");
            }
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File changed: {e.Name} - type: {e.ChangeType}");

            //var procesor = new FileProcessor(e.FullPath);
            //procesor.Process();

            //FilesToProcess.TryAdd(e.FullPath, e.FullPath);
            AddToCache(e.FullPath);
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File deleted: {e.Name} - type: {e.ChangeType}");
        }

        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"File renameed: {e.OldName} to {e.Name} - type: {e.ChangeType}");
        }

        private static void WacherError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Error: file system atcher may no longer be active: {e.GetException()}");
        }





        private static void ProcessFilesOrDirectoriesofFiles(string[] args)
        {
            var comman = args[0];

            if (comman == "--file" || comman == "-f")
            {
                var filePath = args[1];
                Console.WriteLine($"Single file {filePath} selected");
                ProcessSingleFile(filePath);
            }
            else if (comman == "--dir" || comman == "-d")
            {
                var directoryPath = args[1];
                var fileType = args[2];
                Console.WriteLine($"Directory {directoryPath} selected for file type {fileType}");
                ProcessDirectory(directoryPath, fileType);
            }
            else
            {
                Console.WriteLine("Invalid command line options");
            }

            Console.WriteLine("\nPress enter to quit.");
            Console.ReadLine();
        }

        private static void ProcessSingleFile(string filePath)
        {
            var fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }

        private static void ProcessDirectory(string directoryPath, string fileType)
        {
            var allFiles = Directory.GetFiles(directoryPath);

            switch (fileType)
            {
                case "TEXT":
                    var textFiles = Directory.GetFiles(directoryPath, "*.txt");
                    textFiles.ToList().ForEach(x => new FileProcessor(x).Process());
                    break;
                default:
                    Console.WriteLine($"ERROR: {fileType} is not suported");
                    break;
            }
        }

    }
}


//-d  "C:\Users\aidas\source\repos\Working with Files and Streams in CSharp\psdata\in" TEXT
