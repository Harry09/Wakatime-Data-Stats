using System;

namespace WTStats
{
    class Program
    {
        static void Main(string[] args)
        {
			var wtDataAnalyzer = new WTDataAnalyzer("data.json");
			//wtDataAnalyzer.StartDate = new DateTime(2018, 01, 01);

			var total = wtDataAnalyzer.GetTotalTimeCoding();

			Console.WriteLine($"{total.Days * 24 + total.Hours}");

			Console.ReadKey();
        }
    }
}
