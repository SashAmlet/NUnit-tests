using LAB_NUnit_xUnit;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_NUnit_xUnit.Tests
{
    [Trait("Category", "MainPart")]
    public class xUnitTest2: IDisposable
    {
        private Lexer lexer;
        private StringWriter consoleOutput;

        public xUnitTest2()
        {
            lexer = new Lexer();
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
        }

        public void Dispose()
        {
            consoleOutput.Dispose();
        }

        [Fact]
        public void HighlightLexemes_NullCode_NoOutput()
        {
            string nullCode = null;
            Assert.Throws<ArgumentNullException>(() => lexer.HighlightLexemes(nullCode));
            Thread.Sleep(1000); // Delay
        }

        [Theory]
        [InlineData("#include <iostream> int main() { return 0; }", "\u001b[4m#include <iostream>\u001b[0m \u001b[32mint\u001b[0m \u001b[34mmain\u001b[0m\u001b[33m(\u001b[0m\u001b[33m)\u001b[0m \u001b[33m{\u001b[0m \u001b[36mreturn\u001b[0m \u001b[31m0\u001b[0m\u001b[33m;\u001b[0m \u001b[33m}\u001b[0m")]
        public void HighlightLexemes_Colorization(string code, string expectedOutput)
        {
            lexer.HighlightLexemes(code);
            string consoleOutputString = consoleOutput.ToString();
            Assert.Equal(expectedOutput, consoleOutputString);
            Thread.Sleep(1000); // Delay
        }
    }
}
