using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace DataProcesor
{
    public class CsvFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public CsvFileProcessor(string inputFilePath, string outputFilePath) :
           this(inputFilePath, outputFilePath, new FileSystem())
        {
        }

        public CsvFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public string InputFilePath { get; private set; }
        public string OutputFilePath { get; private set; }

        public void Process()
        {
            using (StreamReader input = _fileSystem.File.OpenText(InputFilePath))
            using (CsvReader csvReader = new CsvReader(input, CultureInfo.InvariantCulture))
            using (StreamWriter output = _fileSystem.File.CreateText(OutputFilePath))
            using (CsvWriter csvWriter = new CsvWriter(output, CultureInfo.InvariantCulture))
            {              
                
                csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                csvReader.Configuration.Comment = '@'; //default # symbol
                csvReader.Configuration.AllowComments = true;              
                csvReader.Configuration.RegisterClassMap<ProcessedOrderMap>();

                IEnumerable<ProcessedOrder> records =
                    csvReader.GetRecords<ProcessedOrder>();

                //csvWriter.WriteRecords(records); //nepasiims visu irasu i memory

                csvWriter.WriteHeader<ProcessedOrder>();
                csvWriter.NextRecord();

                var recordsArray = records.ToArray(); //pasiims visu irasu i memory
                for (int i = 0; i < recordsArray.Length; i++)
                {
                    csvWriter.WriteField(recordsArray[i].OrderNumber);
                    csvWriter.WriteField(recordsArray[i].Customer);
                    csvWriter.WriteField(recordsArray[i].Amount);

                    bool isLast = i == recordsArray.Length - 1;

                    if (!isLast)
                    {
                        csvWriter.NextRecord();
                    }
                }
            }
        }
        
        //internal void Process()
        //{
        //    using (StreamReader input = File.OpenText(InputFilePath))
        //    using (CsvReader csvReader = new CsvReader(input, CultureInfo.InvariantCulture)) using (StreamReader input = File.OpenText(InputFilePath))
        //    {
        //        //IEnumerable<Order> records = csvReader.GetRecords<Order>(); //suportina strongly types
        //        IEnumerable<ProcessedOrder> records = 
        //            csvReader.GetRecords<ProcessedOrder>(); //Sumappinsim neatitinkancias prop names

        //        csvReader.Configuration.TrimOptions = TrimOptions.Trim;
        //        csvReader.Configuration.Comment = '@'; //default # symbol
        //        csvReader.Configuration.AllowComments = true;
        //        //csvReader.Configuration.HeaderValidated = null;               
        //        //csvReader.Configuration.MissingFieldFound  = null; 
        //        csvReader.Configuration.RegisterClassMap<ProcessedOrderMap>();                

        //        foreach (ProcessedOrder record in records)
        //        {
        //            Console.WriteLine(record.OrderNumber);
        //            Console.WriteLine(record.Customer);                    
        //            Console.WriteLine(record.Amount);
        //        } 
                
        //        //foreach (Order record in records)
        //        //{
        //        //    Console.WriteLine(record.OrderNumber);
        //        //    Console.WriteLine(record.CustomerNumber);
        //        //    Console.WriteLine(record.Description);
        //        //    Console.WriteLine(record.Quantity);
        //        //}                
        //    }
        //}
        
        //internal void Process()
        //{
        //    using (StreamReader input = File.OpenText(InputFilePath))
        //    using (CsvReader csvReader = new CsvReader(input, CultureInfo.InvariantCulture))
        //    {
        //        IEnumerable<dynamic> records = csvReader.GetRecords<dynamic>(); //one line of csv

        //        csvReader.Configuration.TrimOptions = TrimOptions.Trim;
        //        csvReader.Configuration.Comment = '@'; //default # symbol
        //        csvReader.Configuration.AllowComments = true;
        //        //csvReader.Configuration.IgnoreBlankLines = false; //default true// bet jei bus tada ismes exceptiona
        //        //csvReader.Configuration.Delimiter = ";"; //default "," csv :) jei nebus atskirta mes exceptiona
        //        //csvReader.Configuration.HasHeaderRecord = false; //default true

        //        foreach (var record in records)
        //        {
        //            Console.WriteLine(record.OrderNumber);
        //            Console.WriteLine(record.CustomerNumber);
        //            Console.WriteLine(record.Description);
        //            Console.WriteLine(record.Quantity);
        //        }

        //        //foreach (var record in records) 
        //        //{
        //        //    Console.WriteLine(record.Field1); //jei header nera ir jnaudojamas dynamic
        //        //    Console.WriteLine(record.Field2); //tada naudoto bendrinius "field2" ir t.t.
        //        //    Console.WriteLine(record.Field3);
        //        //    Console.WriteLine(record.Field4);
        //        //}
                
        //    }
        //}
    }
}