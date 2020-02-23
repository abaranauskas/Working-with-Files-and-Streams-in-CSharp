using DataProcesor;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace DataProcessor.Tests
{
    public class BinaryFileProcesorShould
    {
        [Fact]
        public void AddLargestNumber()
        {
            //Assert
            var mockInputFile =
            new MockFileData(new byte[] { 0xFF, 0x34, 0x56, 0xD2 });
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(@"a:\root\in\myfile.data", mockInputFile);
            mockFileSystem.AddDirectory(@"a:\root\out");

            var sut = new BinaryFileProcessor(
                @"a:\root\in\myfile.data",
                @"a:\root\out\myfile.data", mockFileSystem);

            //act
            sut.Process();

            //Assert
            Assert.True(mockFileSystem.FileExists(@"a:\root\out\myfile.data"));

            var processedFile = mockFileSystem.GetFile(@"a:\root\out\myfile.data");
            var data = processedFile.Contents;
            Assert.Equal(5, data.Length);
            Assert.Equal(0xFF, data[4]);
            
        }
    }
}
