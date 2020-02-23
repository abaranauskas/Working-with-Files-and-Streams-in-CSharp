using System;
using Xunit;
using System.IO.Abstractions.TestingHelpers;
using DataProcesor;

namespace DataProcessor.Tests
{
    public class TesxtFileProcesorShould
    {
        [Fact]
        public void MakeSecondLineUpperCase()
        {
            //Assert
            var mockInputFile = new MockFileData("Line 1\nLine 2\nLine 3");
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(@"a:\root\in\myfile.txt", mockInputFile);
            mockFileSystem.AddDirectory(@"a:\root\out");

            var sut = new TextFileProcessor(
                @"a:\root\in\myfile.txt",
                @"a:\root\out\myfile.txt", mockFileSystem);

            //act
            sut.Process();

            //Assert
            Assert.True(mockFileSystem.FileExists(@"a:\root\out\myfile.txt"));

            var processedFile = mockFileSystem.GetFile(@"a:\root\out\myfile.txt");
            var lines = processedFile.TextContents.SplitLines();
            Assert.Equal("Line 1", lines[0]);
            Assert.Equal("LINE 2", lines[1]);
            Assert.Equal("Line 3", lines[2]);
        }
    }
}
