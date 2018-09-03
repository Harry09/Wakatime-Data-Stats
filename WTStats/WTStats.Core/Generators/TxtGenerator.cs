using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WTStats.Core.Generators
{
	class TxtGenerator : IGenerator
	{
		StringBuilder sb = null;

		public TxtGenerator()
		{

		}

		#region Public methods
		public GeneratedData Generate(WTDataAnalyzer wtDataAnalyzer, ILogger logger)
		{
			sb = new StringBuilder();

			GenerateBestDays(wtDataAnalyzer);
			GenerateTotalTime(wtDataAnalyzer);
			GenerateCommonData(WTDataAnalyzer.DataType.Editors, "Editors:", wtDataAnalyzer);
			GenerateCommonData(WTDataAnalyzer.DataType.OperatingSystems, "Operating systems:", wtDataAnalyzer);
			GenerateCommonData(WTDataAnalyzer.DataType.Languages, "Languages:", wtDataAnalyzer);
			GenerateProjectList(wtDataAnalyzer);

			var generatedData = new GeneratedData
			{
				Data = sb.ToString(),
				FileName = "generated_data.txt"
			};

			sb = null;

			return generatedData;
		}
		#endregion

		#region Private methods
		void GenerateBestDays(WTDataAnalyzer wtDataAnalyzer)
		{
			var startDate = wtDataAnalyzer.GetStartDate();
			var endDate = wtDataAnalyzer.GetEndDate();

			AppendSeparator();

			sb.AppendLine($"Best day ever: {wtDataAnalyzer.GetBestDay().GrandTotal.ToString()}");

			sb.AppendLine();

			sb.AppendLine("Best day of the year:");

			for (int year = startDate.Year; year <= endDate.Year; year++)
			{
				var bestDay = wtDataAnalyzer.GetBestDay(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

				sb.AppendLine($"{year}: {bestDay.GrandTotal}");

				startDate.AddYears(1);
			}

			sb.AppendLine();
		}

		void GenerateTotalTime(WTDataAnalyzer wtDataAnalyzer)
		{
			var startDate = wtDataAnalyzer.GetStartDate();
			var endDate = wtDataAnalyzer.GetEndDate();

			AppendSeparator();

			sb.AppendLine($"Total coding activity time: {wtDataAnalyzer.GetTotalTimeCoding().ToCustomString()}");

			sb.AppendLine();

			sb.AppendLine("Total coding for year:");

			for (int year = startDate.Year; year <= endDate.Year; year++)
			{
				var total = wtDataAnalyzer.GetTotalTimeCoding(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

				sb.AppendLine($"{year}: {total.ToCustomString()}");
			}

			sb.AppendLine();
		}

		void GenerateCommonData(WTDataAnalyzer.DataType dataType, string name, WTDataAnalyzer wtDataAnalyzer)
		{
			AppendSeparator();

			sb.AppendLine(name);
			PrintProjectDataList(wtDataAnalyzer.Get(dataType).OrderByDescending(x => x.TotalTime));

			sb.AppendLine();

		}

		void GenerateProjectList(WTDataAnalyzer wtDataAnalyzer)
		{
			AppendSeparator();

			sb.AppendLine("Projects:");

			var data = wtDataAnalyzer.GetProjects().OrderByDescending(m => m.TotalTime).Select(x => new ProjectData { Name = x.Name, TotalTime = x.TotalTime });

			PrintProjectDataList(data);

			sb.AppendLine();
		}
		#endregion

		#region Helpers
		void PrintProjectDataList(IEnumerable<ProjectData> datas)
		{
			foreach (var data in datas)
			{
				sb.AppendLine($"{data.Name}: {TimeSpan.FromSeconds(data.TotalTime).ToCustomString()}");
			}
		}

		void AppendSeparator()
		{
			sb.AppendLine("---------------------------------");
		}
		#endregion

	}
}