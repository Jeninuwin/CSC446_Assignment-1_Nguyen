using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSC446_Assignment_1_Nguyen
{
    class Program
    {
        static string nameFile;
        static char ch;
        static StreamReader reader;
        public enum Symbols
        {
            thent, ift, elset, whilet, floatt, intt, chart, breakt, continuet, voidt, lparent, rparent, unknownt, eoftt, blanks,
            literal, relopt, assignopt, addopt, mulopt, idt, integert, nott, whitespace,
            semit,
            quotet,
            colont,
            commat,
            closeParent,
            openParent,
            period,
            openCurlyParent,
            closeCurlyParent,
            openSquareParent,
            closeSquareParent,
            numt
        }
        static Symbols Token;
        static string Lexeme;
        static int LineNo;
        // static int Value; //integer 
        static double ValueR; //real
        static string ValueL;

        public static List<string> reservedWords;

        static void Main(string[] args)
        {
            reservedWords = new List<string> { "if", "else", "while", "float", "int", "char", "break", "continue", "float", "void" };

            string file_dir = Directory.GetCurrentDirectory() + "\\";

            if (args.Length == 0)
            {
                do
                {
                    Console.WriteLine("Enter your c file name: ");
                    nameFile = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(nameFile));
            }

            if (args.Length > 0 && File.Exists(file_dir + args[0]))
                nameFile = file_dir + args[0];
            else if (File.Exists(file_dir + nameFile))
                nameFile = file_dir + nameFile;
            else
            {
                Console.WriteLine("ERROR: File not found.");
                Environment.Exit(1);
            }

            readFile();

            while (ch != 65535 || ch != '\uffff')
            {
                ProcessToken();
                DisplayToken();
            }

        }

        static void readFile()
        {
            reader = new StreamReader(nameFile);
            ch = (char)reader.Read();
        }

        static void ProcessToken()
        {
            Lexeme = ch.ToString();
            GetNextChar(); //Look ahead

            if (Lexeme[0] >= 'A' && Lexeme[0] <= 'Z' || Lexeme[0] >= 'a' && Lexeme[0] <= 'z')
            {
                ProcessWordToken();
            }
            else if (Lexeme[0] >= '0' && Lexeme[0] <= '9')
            {
                ProcessNumToken();
            }
            else if (Lexeme[0] == '/')
            {
                ProcessCommentToken();
                GetNextToken();
            }
            else if (Lexeme[0] == '"')
            {
                ProcessLiteralToken();

            }
            else if (Lexeme[0] == '<' || Lexeme[0] == '>' || Lexeme[0] == ':')
            {
                //ProcessDoubleToken();
            }
            else if (Lexeme[0] == ' ' || Lexeme[0] == '\r' || Lexeme[0] == '\t' || Lexeme[0] == '\n')
            {
                Token = Symbols.whitespace;
            }
            else
            {
                ProcessSingleToken();
            }

            if (Lexeme.Length > 27)
            {
                Console.WriteLine("Invalid Token. The Length cannot be more than 27");
                Token = Symbols.unknownt;
            }
        }

        static void ProcessCommentToken()
        {
            GetNextChar();

            if (ch == '/' || ch == '*')
            {
                while (ch != 10 && !reader.EndOfStream)
                {
                    GetNextChar();
                }
            }

            Token = Symbols.whitespace;
        }
        static void ProcessWordToken()
        {
            int length = 1;

            while (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z' || ch >= '0' && ch <= '9')
            {
                length++;
                Lexeme += ch.ToString();
                GetNextChar();
            }

            //assign lexeme tokens to their symbols
            switch (Lexeme.ToLower())
            {
                case "if":
                    Token = Symbols.ift;
                    break;
                case "else":
                    Token = Symbols.elset;
                    break;
                case "while":
                    Token = Symbols.whilet;
                    break;
                case "float":
                    Token = Symbols.floatt;
                    break;
                case "int":
                    Token = Symbols.intt;
                    break;
                case "char":
                    Token = Symbols.chart;
                    break;
                case "break":
                    Token = Symbols.breakt;
                    break;
                case "continue":
                    Token = Symbols.continuet;
                    break;
                case "void":
                    Token = Symbols.voidt;
                    break;
                case "or":
                    Token = Symbols.addopt;
                    break;
                case "rem":
                    Token = Symbols.mulopt;
                    break;
                case "mod":
                    Token = Symbols.mulopt;
                    break;
                case "and":
                    Token = Symbols.mulopt;
                    break;
                case "not":
                    Token = Symbols.nott;
                    break;
                default:
                    Token = Symbols.idt;
                    break;

            }

        }
        static void GetNextToken()
        {
            while (ch <= 32)
            {
                GetNextChar();
            }
            if (!reader.EndOfStream)
            {
                ProcessToken();
            }
            else
            {
                Token = Symbols.eoftt;
            }
        }

        static void ProcessNumToken()
        {
            int numOfDecimals = 0;

            while (ch >= '0' && ch <= '9' || (ch == '.' && numOfDecimals < 1))
            {
                if (ch == '.')
                {
                    numOfDecimals += 1;
                }

                Lexeme += ch;
                GetNextChar();
            }

            if (Lexeme[Lexeme.Length - 1] == '.')
            {
                Token = Symbols.unknownt;
                Console.WriteLine("ERROR LINE " + LineNo + ": MALFORMED TOKEN");
                return;
            }
            else if (numOfDecimals == 1)
            {
                ValueR = System.Convert.ToDouble(Lexeme);
                //Token = Symbols.floatt;
                Token = Symbols.numt;
            }
            else
            {
                Value = System.Convert.ToInt32(Lexeme);
                //Token = Symbols.integert;
                Token = Symbols.numt;
            }
        }

        static void GetNextChar()
        {
            if (ch == 10)
                LineNo++;
            ch = (char)reader.Read();

        }
        static void ProcessLiteralToken()
        {
            bool hasEnding = false;
            ValueL = "";

            while (ch != 10 && !reader.EndOfStream && ch != '"')
            {
                if (Lexeme.Length < 17)
                    Lexeme += ch;

                ValueL += ch;
                GetNextChar();

                if (ch == '"')
                {
                    hasEnding = true;
                    GetNextChar();
                    break;
                }
            }
            if (!hasEnding)
            {
                Token = Symbols.unknownt;
                Console.WriteLine("ERROR LINE " + LineNo + ": incomplete literal, missing closing");
            }
            else
            {
                Token = Symbols.literal;
            }
        }

        public static int? Value { get; set; } = null; //this resets the value not sure if this is needed but can remove?

        static void DisplayToken()
        {
            if (Token == Symbols.whitespace)
                return;

            if (Token == Symbols.eoftt)
            {
                Lexeme = "eoft";
            }

            Console.Write("TOKEN: " + Token.ToString().PadRight(17, ' ') + "|  LEXEME: " + Lexeme.PadRight(19, ' '));

            if (Token == Symbols.numt)
            {
                if (Lexeme.Contains('.'))
                    Console.Write("|  REAL VALUE: " + ValueR);
                else
                    Console.Write("|  INT VALUE: " + Value);
            }

            else if (Token == Symbols.literal)
                Console.Write("|  LITERAL VALUE: " + "\"" + ValueL + "\"");

            else if (Token == Symbols.unknownt)
                Console.Write(": ERROR UNKNOWN TOKEN");

            Console.Write("\n");
        }

        static void ProcessSingleToken()
        {

            if (Lexeme[0] == '<' || Lexeme[0] == '>' || Lexeme[0] == '=')
            {
                Token = Symbols.relopt;

            }
            else if (Lexeme[0] == '.')
            {
                Token = Symbols.period;

            }
            else if (Lexeme[0] == '(')
            {
                Token = Symbols.openParent;

            }
            else if (Lexeme[0] == ')')
            {
                Token = Symbols.closeParent;

            }
            else if (Lexeme[0] == '{')
            {
                Token = Symbols.openCurlyParent;
            }
            else if (Lexeme[0] == '}')
            {
                Token = Symbols.closeCurlyParent;
            }
            else if (Lexeme[0] == '[')
            {
                Token = Symbols.openSquareParent;
            }
            else if (Lexeme[0] == ']')
            {
                Token = Symbols.closeSquareParent;
            }
            else if (Lexeme[0] == ',')
            {
                Token = Symbols.commat;

            }
            else if (Lexeme[0] == '+')
            {
                Token = Symbols.addopt;

            }
            else if (Lexeme[0] == '-')
            {
                Token = Symbols.addopt;

            }
            else if (Lexeme[0] == ':')
            {
                Token = Symbols.colont;

            }
            else if (Lexeme[0] == ';')
            {
                Token = Symbols.semit;

            }
            else if (Lexeme[0] == '"')
            {
                Token = Symbols.quotet;

            }
            else
            {
                Token = Symbols.unknownt;

            }
        }

        static void ProcessDoubleToken()
        {

        }
    }
}