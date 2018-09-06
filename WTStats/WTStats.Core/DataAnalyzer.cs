using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

using Newtonsoft.Json;

namespace WTStats.Core
{
	public class DataAnalyzer
    {
		Data.DataCore _dataCore;

		#region Public fields
		public enum DataType
		{
			Dependencies,
			Editors,
			Languages,
			OperatingSystems,
			Categories
		}
		#endregion

		#region Constructors
		private DataAnalyzer()
		{
		}

		public DataAnalyzer(string path) : this()
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(path);

			var json = File.ReadAllText(path);

			_dataCore = JsonConvert.DeserializeObject<Data.DataCore>(json);
		}

		public DataAnalyzer(Data.DataCore wtData) : this()
		{
			_dataCore = wtData;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Return date of first entry in data
		/// </summary>
		/// <returns>DataTime</returns>
		public DateTime GetStartDate()
		{
			return DateTimeOffset.FromUnixTimeSeconds(_dataCore.Range.Start).DateTime;
		}

		/// <summary>
		/// Returns date of last entry in data
		/// </summary>
		/// <returns>DateTime</returns>
		public DateTime GetEndDate()
		{
			return DateTimeOffset.FromUnixTimeSeconds(_dataCore.Range.End).DateTime;
		}

		/// <summary>
		/// Returns total time coding
		/// </summary>
		/// <returns>Total time coding</returns>
		public TimeSpan GetTotalTimeCoding()
		{
			return GetTotalTimeCoding(DateTime.MinValue, DateTime.MaxValue);
		}

		/// <summary>
		/// Returns total time coding in date range
		/// </summary>
		/// <param name="startDate">Start of range</param>
		/// <param name="endDate">End of range</param>
		/// <returns>Total time coding in data range</returns>
		public TimeSpan GetTotalTimeCoding(DateTime startDate, DateTime endDate)
		{
			var total = GetDaysInTimeRange(startDate, endDate).Sum((Data.Day day) =>
			{
				return day.GrandTotal.TotalSeconds;
			});

			return TimeSpan.FromSeconds(total);
		}

		/// <summary>
		/// Returns total daily average
		/// </summary>
		/// <returns>Daily average time</returns>
		public TimeSpan GetDailyAverage()
		{
			return GetDailyAverage(DateTime.MinValue, DateTime.MaxValue);
		}

		/// <summary>
		/// Returns daily average time in date range
		/// </summary>
		/// <param name="startDate">Start of range</param>
		/// <param name="endDate">End of range</param>
		/// <returns>Daily average time</returns>
		public TimeSpan GetDailyAverage(DateTime startDate, DateTime endDate)
		{
			var days = GetDaysInTimeRange(startDate, endDate);

			var total = days.Sum((Data.Day day) =>
			{
				return day.GrandTotal.TotalSeconds;
			});

			total /= days.Count();

			return TimeSpan.FromSeconds(total);
		}

		/// <summary>
		/// Returns day with the greatest coding activity time
		/// </summary>
		/// <returns>Day data</returns>
		public Data.Day GetBestDay()
		{
			return GetBestDay(DateTime.MinValue, DateTime.MaxValue);
		}

		/// <summary>
		/// Returns day with the greatest coding activity time in date range
		/// </summary>
		/// <param name="startDate">Start of range</param>
		/// <param name="endDate">End of range</param>
		/// <returns>Day data</returns>
		public Data.Day GetBestDay(DateTime startDate, DateTime endDate)
		{
			var bestDay = GetDaysInTimeRange(startDate, endDate).OrderByDescending(x => x.GrandTotal.TotalSeconds).First();

			return bestDay;
		}

		/// <summary>
		/// Returns list of projects and data all time
		/// </summary>
		/// <returns>List of projects</returns>
		public List<Project> GetProjects()
		{
			return GetProjects(DateTime.MinValue, DateTime.MaxValue);
		}

		/// <summary>
		/// Returns list of projects and data in specified date range
		/// </summary>
		/// <param name="startDate">Start of range</param>
		/// <param name="endDate">End of range</param>
		/// <returns>List of projects</returns>
		public List<Project> GetProjects(DateTime startDate, DateTime endDate)
		{
			var projects = new List<Project>();

			foreach (var eDay in GetDaysInTimeRange(startDate, endDate))
			{
				eDay.Projects.ForEach(eProject =>
				{
					bool newProject = false;

					var lProject = projects.SingleOrDefault(p => p.Name == eProject.Name);

					if (lProject == null)
					{
						lProject = new Project();
						lProject.Name = eProject.Name;
						newProject = true;
					}

					lProject.TotalSeconds += eProject.GrandTotal.TotalSeconds;

					lProject.Languages = MergeProjectData(eProject.Languages, lProject.Languages);
					lProject.Editors = MergeProjectData(eProject.Editors, lProject.Editors);

					if (newProject)
					{
						projects.Add(lProject);
					}
				});
			}

			return projects;
		}

		/// <summary>
		/// Returns specified data
		/// </summary>
		/// <param name="dataType">Data type</param>
		/// <returns>List of data</returns>
		public List<ProjectData> Get(DataType dataType)
		{
			return Get(dataType, DateTime.MinValue, DateTime.MaxValue);
		}

		/// <summary>
		/// Returns specified data in date range
		/// </summary>
		/// <param name="dataType">Data type</param>
		/// <param name="startDate">Start of range</param>
		/// <param name="endDate">End of range</param>
		/// <returns>List of data</returns>
		public List<ProjectData> Get(DataType dataType, DateTime startDate, DateTime endDate)
		{
			var data = new List<ProjectData>();

			foreach (var eDay in GetDaysInTimeRange(startDate, endDate))
			{
				var property = eDay.GetType().GetProperty(dataType.ToString());

				if (property != null)
				{
					data = MergeProjectData(property.GetValue(eDay) as List<Data.Data>, data);
				}
			}

			return data;
		}
		#endregion

		#region Helpers
		List<ProjectData> MergeProjectData(List<Data.Data> exportData, List<ProjectData> localData)
		{
			localData = localData ?? new List<ProjectData>();

			exportData.ForEach(eData => 
			{
				bool newData = false;

				var lData = localData.SingleOrDefault(x => x.Name == eData.Name);

				if (lData == null)
				{
					lData = new ProjectData();
					lData.Name = eData.Name;
					newData = true;
				}

				lData.TotalSeconds += eData.TotalSeconds;

				if (newData)
				{
					localData.Add(lData);
				}
			});

			return localData;
		}

		IEnumerable<Data.Day> GetDaysInTimeRange(DateTime startDate, DateTime endDate)
		{
			return _dataCore.Days.Where(x => x.Date.DateTime > startDate && x.Date.DateTime < endDate);
		}
		#endregion
	}
}
