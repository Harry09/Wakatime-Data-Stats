using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WTStats.Core.Generators
{
	public class TxtGenerator : IGenerator 
	{
		StringBuilder sb = null;

		public TxtGenerator()
		{

		}

		#region Public methods
		public GeneratorData Generate(DataAnalyzer dataAnalyzer, ILogger logger)
		{
			sb = new StringBuilder();

			GenerateBestDays(dataAnalyzer);
			GenerateTotalTime(dataAnalyzer);
			GenerateCommonData(DataAnalyzer.DataType.Editors, "Editors:", dataAnalyzer);
			GenerateCommonData(DataAnalyzer.DataType.OperatingSystems, "Operating systems:", dataAnalyzer);
			GenerateCommonData(DataAnalyzer.DataType.Languages, "Languages:", dataAnalyzer);
			GenerateProjectList(dataAnalyzer);

			var date = DateTime.Now;

			var dataAction = new GeneratorData
			{
				DataName = $"data_{date:yyyyMMddHHmmss}",
				FileExtension = "txt",
				Data = sb.ToString()
			};

			sb = null;

			return dataAction;
		}
		#endregion

		#region Private methods
		void GenerateBestDays(DataAnalyzer dataAnalyzer)
		{
			var startDate = dataAnalyzer.GetStartDate();
			var endDate = dataAnalyzer.GetEndDate();

			AppendSeparator();

			sb.AppendLine($"Best day ever: {dataAnalyzer.GetBestDay().GrandTotal.ToString()}");

			sb.AppendLine();

			sb.AppendLine("Best day of the year:");

			for (int year = startDate.Year; year <= endDate.Year; year++)
			{
				var bestDay = dataAnalyzer.GetBestDay(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

				sb.AppendLine($"{year}: {bestDay.GrandTotal}");

				startDate.AddYears(1);
			}

			sb.AppendLine();
		}

		void GenerateTotalTime(DataAnalyzer dataAnalyzer)
		{
			var startDate = dataAnalyzer.GetStartDate();
			var endDate = dataAnalyzer.GetEndDate();

			AppendSeparator();

			sb.AppendLine($"Total coding activity time: {dataAnalyzer.GetTotalTimeCoding().ToCustomString()}");

			sb.AppendLine();

			sb.AppendLine("Total coding for year:");

			for (int year = startDate.Year; year <= endDate.Year; year++)
			{
				var total = dataAnalyzer.GetTotalTimeCoding(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

				sb.AppendLine($"{year}: {total.ToCustomString()}");
			}

			sb.AppendLine();
		}

		void GenerateCommonData(DataAnalyzer.DataType dataType, string name, DataAnalyzer dataAnalyzer)
		{
			AppendSeparator();

			sb.AppendLine(name);
			PrintProjectDataList(dataAnalyzer.Get(dataType).OrderByDescending(x => x.TotalTime));

			sb.AppendLine();

		}

		void GenerateProjectList(DataAnalyzer dataAnalyzer)
		{
			AppendSeparator();

			sb.AppendLine("Projects:");

			var data = dataAnalyzer.GetProjects().OrderByDescending(m => m.TotalTime).Select(x => new ProjectData { Name = x.Name, TotalTime = x.TotalTime });

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
