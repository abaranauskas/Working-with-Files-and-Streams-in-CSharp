using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcesor
{
    public class TextFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public TextFileProcessor(string inputFilePath, string outputFilePath) :
            this(inputFilePath, outputFilePath, new FileSystem())
        {
        }

        public TextFileProcessor(string inputFilePath, string outputFilePath,
            IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public void Process()
        {
            //using read all
            //var originalTex = File.ReadAllText(InputFilePath);
            //File.WriteAllText(OutputFilePath, originalTex.ToUpperInvariant());
            //File.WriteAllLines(OutputFilePath, originalTex);

            //using read all lines
            //var originalTex = File.ReadAllLines(InputFilePath);          
            //originalTex[1] = originalTex[1].ToUpperInvariant();
            //File.WriteAllLines(OutputFilePath, originalTex);

            //using FileStreams 
            //1 using (var inputFileStream = new FileStream(InputFilePath, FileMode.Open))
            //2 using (var inputStreamReader = new StreamReader(inputFileStream))
            //using (var outputtFileStream = new FileStream(OutputFilePath, FileMode.Create))
            //using (var outpuStreamWriter = new StreamWriter(outputtFileStream))
            //{
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        var line = inputStreamReader.ReadLine();
            //        var processedLine = line.ToUpper();
            //        outpuStreamWriter.WriteLine(processedLine);
            //    }
            //}


            ////using FileStreams simplified           
            ////using (var inputStreamReader = File.OpenText(InputFilePath))
            ////1 i2 antra atlikama under the hood negalima nurodyt encoding bus tik utf-8
            //using (var inputStreamReader = new StreamReader(InputFilePath)) //cia dar supaprastinama
            ////using (var outputtFileStream = new FileStream(OutputFilePath, FileMode.Create))
            //using (var outpuStreamWriter = new StreamWriter(OutputFilePath)) //
            //{
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        var line = inputStreamReader.ReadLine();
            //        var processedLine = line.ToUpper();


            //        if (inputStreamReader.EndOfStream)
            //        {
            //            outpuStreamWriter.Write(processedLine);
            //        }
            //        else
            //        {
            //            outpuStreamWriter.WriteLine(processedLine);
            //        }
            //    }
            //}

            ////using FileStreams     

            //using (var inputStreamReader = new StreamReader(InputFilePath))
            //using (var outpuStreamWriter = new StreamWriter(OutputFilePath))
            //{
            //    var currentLine = 1;
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        var line = inputStreamReader.ReadLine();
            //        if (currentLine == 2)
            //            Write(line.ToUpper());
            //        else
            //            Write(line);

            //        currentLine++;

            //        void Write(string line)
            //        {
            //            if (inputStreamReader.EndOfStream)
            //                outpuStreamWriter.Write(line);
            //            else
            //                outpuStreamWriter.WriteLine(line);
            //        }

            //    }
            //}


            using (var inputStreamReader = _fileSystem.File.OpenText(InputFilePath))
            using (var outpuStreamWriter = _fileSystem.File.CreateText(OutputFilePath))
            {
                var currentLine = 1;
                while (!inputStreamReader.EndOfStream)
                {
                    var line = inputStreamReader.ReadLine();
                    if (currentLine == 2)
                        Write(line.ToUpperInvariant());
                    else
                        Write(line);

                    currentLine++;

                    void Write(string line)
                    {
                        if (inputStreamReader.EndOfStream)
                            outpuStreamWriter.Write(line);
                        else
                            outpuStreamWriter.WriteLine(line);
                    }

                }
            }

        }
    }
}
