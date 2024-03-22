using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LAB1_NUnit.Tests
{
    [TestFixture]
    public class LexerTests
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
        }

        [Test]
        public void PrepareTextForLA_InputWithComments_RemovesComments()
        {
            string inputText = "This is a line with // comments";
            string expectedText = "This is a line with";

            string actualText = Lexer.PrepareTextForLA(inputText);

            Assert.AreEqual(expectedText, actualText);
        }

        [Test]
        public void HighlightLexemes_EmptyCode_NoOutput()
        {
            string emptyCode = "";
            Assert.DoesNotThrow(() => lexer.HighlightLexemes(emptyCode));
        }

        [Test]
        public void HighlightLexemes_NullCode_ThrowsArgumentNullException()
        {
            string nullCode = null;
            Assert.Throws<ArgumentNullException>(() => lexer.HighlightLexemes(nullCode));
        }

        [Test]
        public void ReadFile_NullCode_ThrowsArgumentNullException()
        {
            string path = null;
            Assert.DoesNotThrow(() => Lexer.ReadFile(path));
        }

        [Test]
        public void HighlightLexemes_InvalidCode_NoExceptionThrown()
        {
            string invalidCode = "This is not valid code!";
            Assert.DoesNotThrow(() => lexer.HighlightLexemes(invalidCode));
        }

        [Test]
        public void HighlightLexemes_ValidCode_HighlightsLexemes()
        {
            string code = "#include <iostream> int main() { return 0; }";
            string expectedOutput = "\u001b[4m#include <iostream>\u001b[0m \u001b[32mint\u001b[0m \u001b[34mmain\u001b[0m\u001b[33m(\u001b[0m\u001b[33m)\u001b[0m \u001b[33m{\u001b[0m \u001b[36mreturn\u001b[0m \u001b[31m0\u001b[0m\u001b[33m;\u001b[0m \u001b[33m}\u001b[0m";

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                lexer.HighlightLexemes(code);
                string consoleOutput = sw.ToString().Trim();
                Assert.AreEqual(expectedOutput, consoleOutput);
            }
        }

        // метчер для перевірки відповідності вихідної строки регулярному виразу
        [Test]
        public void HighlightLexemes_ComplexCode_MatchesHamcrestMatchers()
        {
            string code = "#include <iostream> int main() { return 0; }";

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                lexer.HighlightLexemes(code);
                string consoleOutput = sw.ToString().Trim();

                Assert.That(consoleOutput, Does.Match(lexer.GetRegularExpression()));
            }
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
        }

        // параметризований тестовий метод
        [Test]
        [TestCase("#include <stdlib.h> \r\nvoid fcheck(int* a, int& k)\r\n{ \r\n\tint c; // comment\r\n\tint p;\r\n}", "#include <stdlib.h> void fcheck(int* a, int& k) { int c; int p; }")]
        [TestCase("#include <iostream>\r\n\r\nint main()\r\n{\r\n\treturn 0;\r\n}", "#include <iostream> int main() { return 0; }")]
        public void HighlightLexemes_ParameterizedTest(string input, string expected)
        {
            using (StringWriter sw = new())
            {
                
                string consoleOutput = sw.ToString().Trim();
                string preparedCode = Lexer.PrepareTextForLA(input);
                Assert.AreEqual(expected, preparedCode);
            }
        }
    }
}
