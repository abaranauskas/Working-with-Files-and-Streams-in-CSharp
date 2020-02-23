using System;
using System.IO;

namespace DataProcesor
{
    internal class FileProcessor
    {
        private static readonly string BackUpDirName = "backup";
        private static readonly string InProgresssDirName = "inprogress";
        private static readonly string CompletedDirName = "complete";

        public FileProcessor(string filePath)
        {
            this.InputFilePath = filePath;
        }

        public string InputFilePath { get; }

        internal void Process()
        {
            Console.WriteLine($"Beggining process of {InputFilePath}");

            //check for file existance
            if (!File.Exists(InputFilePath))
            {
                Console.WriteLine($"\nERROR: File {InputFilePath} does not exist.");
                return;
            }

            var rootDirectoryPath = new DirectoryInfo(InputFilePath)
                .Parent.Parent.FullName;

            Console.WriteLine($"\nRoot data path is {rootDirectoryPath}");

            //check if back up dir exist
            var inputFileDirectoryPath = Path.GetDirectoryName(InputFilePath);
            var backUpDirPath = Path.Combine(rootDirectoryPath, BackUpDirName);

            //if (!Directory.Exists(backUpDirPath))
            //{
            //    Console.WriteLine($"\nCreating {backUpDirPath}");
            Directory.CreateDirectory(backUpDirPath);
            //}

            //copy file to back up
            var inputFileName = Path.GetFileName(InputFilePath);
            var backUpFilePath = Path.Combine(backUpDirPath, inputFileName);
            Console.WriteLine($"\nCopying the file {InputFilePath} to {backUpFilePath}");
            File.Copy(InputFilePath, backUpFilePath, true);

            //move to inprogress dir           
            Directory.CreateDirectory(Path.Combine(rootDirectoryPath, InProgresssDirName));
            var inProgressFilePath = Path.Combine(rootDirectoryPath, InProgresssDirName, inputFileName);

            if (File.Exists(inProgressFilePath))
            {
                Console.WriteLine($"ERROR: File {inProgressFilePath} has been processed!");
                return;
            }

            //Console.WriteLine($"\nMoving the file {InputFilePath} to {inProgressFilePath}");
            //File.Move(InputFilePath, inProgressFilePath);

            //determine type of file
            var extension = Path.GetExtension(InputFilePath);

            var completedDirPath = Path.Combine(rootDirectoryPath, CompletedDirName);
            Directory.CreateDirectory(completedDirPath);

            var completedFileName = $"{Path.GetFileNameWithoutExtension(InputFilePath)}-{Guid.NewGuid()}{extension}";

            switch (extension)
            {
                case ".txt":
                    var textProcessor =
                        new TextFileProcessor(InputFilePath,
                        Path.Combine(completedDirPath, completedFileName));
                    textProcessor.Process();
                    break;
                case ".data":
                    var binaryProcessor =
                        new BinaryFileProcessor(InputFilePath,
                        Path.Combine(completedDirPath, completedFileName));
                    binaryProcessor.Process();
                    break;
                case ".csv":
                    var csvProcessor =
                        new CsvFileProcessor(InputFilePath,
                        Path.Combine(completedDirPath, completedFileName));
                    csvProcessor.Process();
                    break;
                default:
                    break;
            }

            //move to inprogress dir   
            //var completedDirPath = Path.Combine(rootDirectoryPath, CompletedDirName);
            //Directory.CreateDirectory(completedDirPath);

            //var completedFileName = $"{Path.GetFileNameWithoutExtension(InputFilePath)}-{Guid.NewGuid()}{extension}";

            //completedFileName = Path.ChangeExtension(completedFileName, ".completed");

            //Console.WriteLine($"\nMoving the file {inProgressFilePath} to {completedDirPath}");
            //File.Move(inProgressFilePath, Path.Combine(completedDirPath, completedFileName));


            //var inProgressDirPath = Path.GetDirectoryName(inProgressFilePath);
            //Directory.Delete(inProgressDirPath, true);

            Console.WriteLine($"Completed processing of {inProgressFilePath}");
            Console.WriteLine($"Deleting {inProgressFilePath}");
            File.Delete(inProgressFilePath);
        }

        private void ProcessTextFile(string inProgressFilePath)
        {

        }
    }
}