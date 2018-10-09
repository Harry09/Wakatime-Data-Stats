using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Globalization;

namespace WTStats.Core.Generators
{
	class ChartJsData
	{
		public string DataName { get; set; }

		public Dictionary<string, double> Data { get; set; }

		public ChartJsData()
		{
			Data = new Dictionary<string, double>();
		}

		public string GenerateChartData()
		{
			var values = Data.Values.Select(x => x.ToString("F2", CultureInfo.InvariantCulture));

			string data = @"
			" + DataName + @" = {

				labels: [ '" + string.Join("', '", Data.Keys) + @"' ],
				datasets: [{
					label: 'Hours',
					data: [ " + string.Join(", ", values) + @" ],
					backgroundColor: randomScalingFactor()
				}]
			};";

			return data;
		}
	}

	public class HTMLGenerator : IGenerator
	{
		static readonly string TemplatePath = "Assets/template.html";
		DataAnalyzer dataAnalyzer = null;

		public GeneratorData Generate(DataAnalyzer dataAnalyzer, ILogger logger)
		{
			if (!File.Exists(TemplatePath))
			{
				throw new FileNotFoundException("Cannot find HTML template!");
			}

			this.dataAnalyzer = dataAnalyzer;

			string htmlCode = File.ReadAllText(TemplatePath);

			var datas = new List<ChartJsData>
			{
				GenerateBestDay(),
				GenerateTotalTime(),
				GenerateDailyAverage(),
				GenerateCommonData(DataAnalyzer.DataType.Editors, "editorsData"),
				GenerateCommonData(DataAnalyzer.DataType.OperatingSystems, "osData"),
				GenerateCommonData(DataAnalyzer.DataType.Languages, "languagesData"),
				GenerateProjectsList()
			};

			PutChartsData(ref htmlCode, datas);

			var data = new GeneratorData
			{
				DataName = $"data_{DateTime.Now:yyyyMMddHHmmss}",
				FileExtension = "html",
				Data = htmlCode
			};

			return data;
		}

		#region Private methods
		#region Generators
		ChartJsData GenerateBestDay()
		{
			var startDate = dataAnalyzer.GetStartDate();
			var endDate = dataAnalyzer.GetEndDate();

			var chartJsData = new ChartJsData();
			chartJsData.DataName = "bestDayData";

			for (int year = startDate.Year; year <= endDate.Year; year++)
			{
				var bestDay = dataAnalyzer.GetBestDay(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

				chartJsData.Data.Add(year.ToString(), bestDay.GrandTotal.TimeSpan.TotalHours);
			}

			return chartJsData;
		}

		ChartJsData GenerateTotalTime()
		{
			var startDate = dataAnalyzer.GetStartDate();
			var endDate = dataAnalyzer.GetEndDate();

			var chartJsData = new ChartJsData();
			chartJsData.DataName = "totalTimeData";

			chartJsData.Data.Add("All time", dataAnalyzer.GetTotalTimeCoding().TotalHours);

			for (int year = startDate.Year; year <= endDate.Year; year++)
			{
				var total = dataAnalyzer.GetTotalTimeCoding(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

				chartJsData.Data.Add(year.ToString(), total.TotalHours);
			}

			return chartJsData;
		}

		ChartJsData GenerateDailyAverage()
		{
			var startDate = dataAnalyzer.GetStartDate();
			var endDate = dataAnalyzer.GetEndDate();

			var chartJsData = new ChartJsData();
			chartJsData.DataName = "averageData";

			chartJsData.Data.Add("All time", dataAnalyzer.GetDailyAverage().TotalHours);

			for (int year = startDate.Year; year <= endDate.Year; year++)
			{
				var total = dataAnalyzer.GetDailyAverage(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

				chartJsData.Data.Add(year.ToString(), total.TotalHours);
			}

			return chartJsData;
		}

		ChartJsData GenerateCommonData(DataAnalyzer.DataType dataType, string name)
		{
			var chartJsData = new ChartJsData();
			chartJsData.DataName = name;

			GenerateProjectData(ref chartJsData, dataAnalyzer.Get(dataType).OrderByDescending(x => x.TotalSeconds));

			return chartJsData;
		}

		ChartJsData GenerateProjectsList()
		{
			var chartJsData = new ChartJsData();
			chartJsData.DataName = "projectsData";

			var data = dataAnalyzer.GetProjects().OrderByDescending(m => m.TotalSeconds).Cast<ProjectData>();

			GenerateProjectData(ref chartJsData, data);

			return chartJsData;
		}

		void GenerateProjectData(ref ChartJsData chartJsData, IEnumerable<ProjectData> datas)
		{
			foreach (var data in datas)
			{
				chartJsData.Data.Add(data.Name, data.TimeSpan.TotalHours);
			}
		}
		#endregion

		#region HTML helpers
		void PutChartsData(ref string htmlCode, List<ChartJsData> chartsData)
		{
			var chartTagIndex = htmlCode.IndexOf("//@Charts.Data");

			if (chartTagIndex == -1)
				return;

			string data = string.Empty;

			chartsData.ForEach(x => data += x.GenerateChartData() + "\n");

			htmlCode = htmlCode.Insert(chartTagIndex, data);
		}
		#endregion
		#endregion
	}
}
