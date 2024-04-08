using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LAB_NUnit_xUnit
{
    public class Lexer
    {
        private Dictionary<string, string> regexToColor;

        public Lexer()
        {
            InitializeRegexToColor();
        }

        private void InitializeRegexToColor()
        {
            regexToColor = new Dictionary<string, string>
                    {
                        { "#include <.*?>", "4" },
                        { @"\b(int|float|double|char)\b", "32" },
                        { @"\b\d+(\.\d+)?\b", "31" },
                        { @"(\"".*?\"")|('.')", "42" },
                        { @"\b(if|else|while|return|cout|cin|endl)\b", "36" },
                        { @"[+\-*/=<>%&\?:]", "35" },
                        { @"[;,{}()\[\]]", "33" },
                        { @"\b[a-zA-Z]+\b", "34" }
                    };
        }

        public Regex GetRegularExpression()
        {
            string combinedRegex = string.Join("|", regexToColor.Keys);
            return new(combinedRegex);
        }

        public static string ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failed to open file: " + e.Message);
                return "";
            }
        }

        public static string PrepareTextForLA(string inputText)
        {
            // Removing comments (single and multi-line)
            string processedText = Regex.Replace(inputText, @"//.*|/\*(.|[\r\n])*?\*/", "");
            // Removing extra spaces and line breaks
            processedText = Regex.Replace(processedText, @"\s+", " ");
            // Removing leading and trailing spaces
            processedText = processedText.Trim();
            return processedText;
        }

        private static void ColorizeText(string text, string color)
        {
            Console.Write("\x1B[" + color + "m" + text + "\x1B[0m");
        }

        public void HighlightLexemes(string code)
        {
            
            Regex lexemeRegex = GetRegularExpression();
            MatchCollection matches = lexemeRegex.Matches(code);

            int position = 0;
            foreach (Match match in matches.Cast<Match>())
            {
                if (match.Index > position)
                {
                    Console.Write(code[position..match.Index]);
                }

                if (match.Value != "")
                {
                    foreach (var entry in regexToColor)
                    {
                        if (Regex.IsMatch(match.Value, entry.Key))
                        {
                            ColorizeText(match.Value, entry.Value);
                            break;
                        }
                    }
                }

                position = match.Index + match.Length;
            }

            if (position < code.Length)
            {
                Console.Write(code[position..]);
            }
        }
    }
}
