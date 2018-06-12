using System;

namespace WTParser
{
    class Program
    {
        static void Main(string[] args)
        {
			var wtDataAnalyzer = new WTDataAnalyzer("data.json");

			Console.ReadKey();
        }
    }
}
