using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

//[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads =4)]
namespace LAB_NUnit_xUnit.Tests
{
    [Trait("Category", "Preparation")]
    public class xUnitTest1
    {
        private Lexer lexer;

        public xUnitTest1()
        {
            lexer = new Lexer();
        }

        [Fact]
        public void GetRegularExpression_ReturnsRegex()
        {
            Regex regex = lexer.GetRegularExpression();
            Assert.NotNull(regex);
            Thread.Sleep(1000); // Delay
        }

        [Theory]
        [InlineData("test.txt", "This is a test file content.")]
        [InlineData("nonexistent.txt", "")]
        public void ReadFile_ValidPath_ReturnsFileContent(string path, string expectedContent)
        {
            string actualContent = Lexer.ReadFile(path);
            Assert.Equal(expectedContent, actualContent);
            Thread.Sleep(1000); // Delay
        }

        [Theory]
        [InlineData("This is a line with // comments", "This is a line with")]
        [InlineData("This is a line without comments", "This is a line without comments")]
        public void PrepareTextForLA_InputWithComments_RemovesComments(string inputText, string expectedText)
        {
            string actualText = Lexer.PrepareTextForLA(inputText);
            Assert.NotEmpty(actualText);
            Assert.Equal(expectedText, actualText);
            Thread.Sleep(1000); // Delay
        }

        

        [Fact]
        public void PrepareTextForLA_RemovesComments()
        {
            string code = @"#include <iostream>
                        int main()
                        {
                            // This is a comment
                            /* Multi-line comment */
                            return 0;
                        }";
            
            string preparedCode = Lexer.PrepareTextForLA(code);

            Assert.NotEmpty(preparedCode);

            Assert.DoesNotMatch(@"//.*|/\*.*?\*/", preparedCode);
            Thread.Sleep(1000); // Delay
        }
    }
}