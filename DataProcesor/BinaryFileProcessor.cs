using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace DataProcesor
{
    public class BinaryFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public BinaryFileProcessor(string inputFilePath, string outputFilePath) :
           this(inputFilePath, outputFilePath, new FileSystem())
        {
        }

        public BinaryFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public void Process()
        {
            //byte[] data = File.ReadAllBytes(InputFilePath);
            //byte lagest = data.Max();
            //byte[] newData = new byte[data.Length + 1];
            //Array.Copy(data, newData, data.Length);
            //newData[newData.Length - 1] = lagest;
            //File.WriteAllBytes(OutputFilePath, newData);


            //Binary streamm
            //using (FileStream input = File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            //using (FileStream output = File.Create(OutputFilePath))
            //{
            //    const int endOfStream = -1;
            //    int largestByte = 0;
            //    int currentByte = input.ReadByte(); //jei pabaiga grazina -1

            //    while (currentByte != endOfStream)
            //    {
            //        output.WriteByte((byte)currentByte);
            //        largestByte = largestByte < currentByte ? currentByte : largestByte;

            //        currentByte = input.ReadByte();
            //    }

            //    output.WriteByte((byte)largestByte);
            //}

            //Binary streamm
            //using (FileStream inputFileStream = File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            //using (BinaryReader binaryFileReader = new BinaryReader(inputFileStream))
            //using (FileStream outputFileStream = File.Create(OutputFilePath))
            //using (BinaryWriter binaryFileWriter = new BinaryWriter(outputFileStream))
            //{
            //    byte largestByte = 0;

            //    while (binaryFileReader.BaseStream.Position < binaryFileReader.BaseStream.Length)
            //    {
            //        var currentByte = binaryFileReader.ReadByte();
            //        binaryFileWriter.Write(currentByte);
            //        largestByte = largestByte < currentByte ? currentByte : largestByte;
            //    }

            //    binaryFileWriter.Write(largestByte);
            //}

            ////Binary streamm
            //using (FileStream inputFileStream = File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            //using (BinaryReader binaryFileReader = new BinaryReader(inputFileStream))
            //using (FileStream outputFileStream = File.Create(OutputFilePath))
            //using (BinaryWriter binaryFileWriter = new BinaryWriter(outputFileStream))
            //{
            //    int largestByte = 0;

            //    while (binaryFileReader.BaseStream.Position < binaryFileReader.BaseStream.Length)
            //    {
            //        int currentByte = binaryFileReader.ReadByte();
            //        binaryFileWriter.Write(currentByte);
            //        largestByte = largestByte < currentByte ? currentByte : largestByte;
            //    }

            //    binaryFileWriter.Write(largestByte);
            //}

            //Binary streamm System.IO.Abstract;
            using (Stream inputFileStream = _fileSystem.File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryFileReader = new BinaryReader(inputFileStream))
            using (Stream outputFileStream = _fileSystem.File.Create(OutputFilePath))
            using (BinaryWriter binaryFileWriter = new BinaryWriter(outputFileStream))
            {
                byte largestByte = 0;
                //int largestByte = 0;

                while (binaryFileReader.BaseStream.Position < binaryFileReader.BaseStream.Length)
                {
                    byte currentByte = binaryFileReader.ReadByte();
                    //int currentByte = binaryFileReader.ReadByte();
                    binaryFileWriter.Write(currentByte);
                    largestByte = largestByte < currentByte ? currentByte : largestByte;
                }

                binaryFileWriter.Write(largestByte);
            }
        }
    }
}
