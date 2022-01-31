using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSC446_Assignment_1_Nguyen
{
    class Program
    {
        static int intgerToken; //interger token 
        static string literalToken; //literal token 
        static double realToken; //real token 
        static string Lexeme;
        static int line;
        static string Token;
        static char temp;
        static char character;
        static string[] reservedWords = { "if", "else", "while", "float", "int", "char", "break", "continue", "float", "void" };
        public enum Symbols
        {
            ift, elset, whilet, floatt, intt, chart, breakt, continuet, voidt
        }

        static string[] addop = { "+", "-", "||" };
        static string[] mulop = { "*", "/", "%", "&&" };
        static string[] assignop = { "=" };
        static string[] symbols = { "(", ")", "{", "}", "[", "]", ",", ";", ".", "\"", };
        static string[] relop = { "==", "!=", "<", "<=", ">", ">=" };

        static StreamReader readingLines;

        static void Main(string[] args)
        {
            string directory = Directory.GetCurrentDirectory() + "\\";
            string fileName = "";

        filecheck:
            if (args.Length == 0)
            {
                do
                {
                    Console.WriteLine("Enter in your c-- file name: ");
                    fileName = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(fileName));
            }

            if (args.Length > 0 && File.Exists(directory + args[0]))
            {
                fileName = directory + args[0];
            }

            else if (File.Exists(directory + fileName))
            {
                fileName = directory + fileName;
            }

            else
            {
                Console.WriteLine("Error! File was not found!");
                goto filecheck;
            }

            LexicalAnalyzer(fileName);

            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }


        /*Lexical Analyzer*/
        static void LexicalAnalyzer(string fileName)
        {
            readingLines = new StreamReader(fileName);
            character = (char)readingLines.Read();

            Console.WriteLine("This is the Lexcial Analyzer reading all lines");

            processToken();
        }

        static void processToken()
        {
            Console.WriteLine("This is the processToken");

            Lexeme = character.ToString();

            Console.WriteLine(Lexeme[0]);
            getNextChar(); // one character lookahead

            if (Lexeme[0] >= 'A' && Lexeme[0] <= 'Z' || Lexeme[0] >= 'a' && Lexeme[0] <= 'z')
            {
                processWordToken();

            }
            else if (Lexeme[0] >= '0' && Lexeme[0] <= '9')
            {
                processNumToken();

            }
            else
            {
                processSingleToken();

            }

        }

        static void processWordToken()
        {
            Console.WriteLine("This is processWordToken");

            if (Lexeme.Length <= 27)
            {
                Console.WriteLine("Invalid");
                Lexeme = "unknown";
            }

        }

        static void getNextChar()
        {
            Console.WriteLine("getting getNextChar");
            do
            {
                temp = (char)readingLines.Read();
                Console.WriteLine(temp);
                line++;

            } while (temp != ' ');
        }

        static void getNextToken()
        {

        }


        static void processNumToken()
        {
            Console.WriteLine("This is processNumToken");

        }

        static void processSingleToken()
        {

        }

        static void display()
        {

        }

    }
}
