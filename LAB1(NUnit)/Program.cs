using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LAB_NUnit_xUnit
{
    public class Program
    {
        public static void Main()
        {
            string path = @"C:\Users\ostre\OneDrive\Books\3rd_course\2d semester\Program specification and verification\LAB1(NUnit)\LAB1(NUnit)\Program3.txt";
            
            Lexer highlighter = new();
            string sourceCode = Lexer.ReadFile(path);
            string preparedCode = Lexer.PrepareTextForLA(sourceCode);
            highlighter.HighlightLexemes(preparedCode);
        }
    }
}