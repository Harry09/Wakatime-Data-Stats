using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WTStats.Core.Generators
{
	public class TxtGenerator : IGenerator 
	{
		StringBuilder sb = null;
		DataAnalyzer dataAnalyzer = null;

		#region Public methods
		public GeneratorData Generate(DataAnalyzer dataAnalyzer, ILogger logger)
		{
			sb = new StringBuilder();
			this.dataAnalyzer = dataAnalyzer;

			GenerateBestDays();
			GenerateTotalTime();
			GenerateCommonData(DataAnalyzer.DataType.Editors, "Editors:");
			GenerateCommonData(DataAnalyzer.DataType.OperatingSystems, "Operating systems:");
			GenerateCommonData(DataAnalyzer.DataType.Languages, "Languages:");
			GenerateProjectList();

			var dataAction = new GeneratorData
			{
				DataName = $"data_{DateTime.Now:yyyyMMddHHmmss}",
				FileExtension = "txt",
				Data = sb.ToString()
			};

			sb = null;

			return dataAction;
		}
		#endregion

		#region Private methods
		void GenerateBestDays()
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
			}

			sb.AppendLine();
		}

		void GenerateTotalTime()
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

		void GenerateCommonData(DataAnalyzer.DataType dataType, string name)
		{
			AppendSeparator();

			sb.AppendLine(name);
			PrintProjectDataList(dataAnalyzer.Get(dataType).OrderByDescending(x => x.TotalSeconds));

			sb.AppendLine();

		}

		void GenerateProjectList()
		{
			AppendSeparator();

			sb.AppendLine("Projects:");

			var data = dataAnalyzer.GetProjects().OrderByDescending(m => m.TotalSeconds).Cast<ProjectData>();

			PrintProjectDataList(data);

			sb.AppendLine();
		}
		#endregion

		#region Helpers
		void PrintProjectDataList(IEnumerable<ProjectData> datas)
		{
			foreach (var data in datas)
			{
				sb.AppendLine($"{data.Name}: {TimeSpan.FromSeconds(data.TotalSeconds).ToCustomString()}");
			}
		}

		void AppendSeparator()
		{
			sb.AppendLine("---------------------------------");
		}
		#endregion

	}
}
