using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LAB_NUnit_xUnit.Tests
{
    [TestFixture]
    [Category("MainPart")]
    public class NUnitTest2
    {
        private Lexer lexer;

        [SetUp]
        public void Setup()
        {
            lexer = new Lexer();
        }
        [Test]
        public void HighlightLexemes_EmptyCode_NoOutput()
        {
            string emptyCode = "";
            Assert.DoesNotThrow(() => lexer.HighlightLexemes(emptyCode));
            Thread.Sleep(1000); // Delay
        }

        [Test]
        public void HighlightLexemes_NullCode_ThrowsArgumentNullException()
        {
            string nullCode = null;
            Assert.Throws<ArgumentNullException>(() => lexer.HighlightLexemes(nullCode));
            Thread.Sleep(1000); // Delay
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
            Thread.Sleep(1000); // Delay
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
            Thread.Sleep(1000); // Delay
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
            Thread.Sleep(1000); // Delay
        }
    }
}
