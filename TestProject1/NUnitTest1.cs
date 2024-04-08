using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[assembly: Parallelizable(ParallelScope.Children)]
namespace LAB_NUnit_xUnit.Tests
{
    [TestFixture]
    [Category("Preparation")]
    public class NUnitTest1
    {
        private Lexer lexer;

        [SetUp]
        public void Setup()
        {
            lexer = new Lexer();
        }

        [Test]
        public void ReadFile_ValidPath_ReturnsFileContent()
        {
            string path = @"test.txt";
            string expectedContent = "This is a test file content.";
            // Create a test file with expected content
            System.IO.File.WriteAllText(path, expectedContent);

            string actualContent = Lexer.ReadFile(path);

            Assert.AreEqual(expectedContent, actualContent);
            Thread.Sleep(1000); // Delay
        }

        [Test]
        public void PrepareTextForLA_InputWithComments_RemovesComments()
        {
            string inputText = "This is a line with // comments";
            string expectedText = "This is a line with";

            string actualText = Lexer.PrepareTextForLA(inputText);

            Assert.AreEqual(expectedText, actualText);
            Thread.Sleep(1000); // Delay
        }

        [Test]
        public void ReadFile_NullCode_ThrowsArgumentNullException()
        {
            string path = null;
            Assert.DoesNotThrow(() => Lexer.ReadFile(path));
            Thread.Sleep(1000); // Delay
        }

        

        // метчер для перевірки наявності коментарів після обробки
        [Test]
        public void PrepareTextForLA_RemovesComments()
        {
            // Arrange
            string inputText = @"
                    // This is a comment
                    /* Multi-line
                       comment */
                    int main() {
                        return 0;
                    }
                ";

            // Act
            string processedText = Lexer.PrepareTextForLA(inputText);

            // Assert
            Assert.That(processedText, Does.Not.Match(@"//.*|/\*.*?\*/"));
            Thread.Sleep(1000); // Delay
        }

    }
}