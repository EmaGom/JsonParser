using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JsonParser.Parser;

namespace JsonParser
{
    internal static class Program
    {
        static void Main()
        {
            Console.WriteLine("Paste your json content below and end with an empty line:");

            // Modify the code below to be stream safe
            var input = new MemoryStream(Encoding.ASCII.GetBytes(ReadLines()));
            var output = AttributeFirstJsonTransformer.Transform(input);
            Console.WriteLine("Output:");
            Console.WriteLine(new StreamReader(output).ReadToEnd());
        }

        static string ReadLines()
        {
            IEnumerable<string> InfiniteReadLines()
            {
                while (true) yield return Console.ReadLine();
            }

            return string.Join(Environment.NewLine, InfiniteReadLines().TakeWhile(line => !string.IsNullOrEmpty(line)));
        }
    }
}