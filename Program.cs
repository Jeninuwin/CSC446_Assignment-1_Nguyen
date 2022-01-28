using System;


namespace CSC446_Assignment_1_Nguyen
{
    class Program
    {
        static void Main(string[] args)
        {

            string lines = System.IO.File.ReadAllText(@"C:\Users\Daehy\OneDrive - South Dakota State University - SDSU\Desktop\Class\Compliers\test.txt");

            // Display the file contents to the console. Variable text is a string.

            LexAnalyzer(lines);

            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        static void LexAnalyzer(string text)
        {
            Console.WriteLine("This is the LexAnalyzer");
            Console.WriteLine("Contents of WriteText.txt = {0}", text);

        }

        static void SymbolTable()
        {

        }
    }
}
