using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

using Newtonsoft.Json;

namespace WTStats
{
	public class WTDataAnalyzer
    {
		Data.DataCore _dataCore;

		#region Public fields
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

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
		private WTDataAnalyzer()
		{
			StartDate = DateTime.MinValue;
			EndDate = DateTime.MaxValue;
		}

		public WTDataAnalyzer(string path) : this()
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(path);

			var json = File.ReadAllText(path);

			_dataCore = JsonConvert.DeserializeObject<Data.DataCore>(json);
		}

		public WTDataAnalyzer(Data.DataCore wtData) : this()
		{
			_dataCore = wtData;
		}
		#endregion

		#region Public methods
		public DateTime GetStartDate()
		{
			return DateTimeOffset.FromUnixTimeSeconds(_dataCore.Range.Start).DateTime;
		}

		public DateTime GetEndDate()
		{
			return DateTimeOffset.FromUnixTimeSeconds(_dataCore.Range.End).DateTime;
		}

		public TimeSpan GetTotalTimeCoding()
		{
			var total = GetDaysInTimeRange().Sum((Data.Day day) =>
			{
				return day.GrandTotal.TotalSeconds;
			});

			return TimeSpan.FromSeconds(total);
		}

		public Data.Day GetBestDay()
		{
			var bestDay = GetDaysInTimeRange().OrderByDescending(x => x.GrandTotal.TotalSeconds).First();

			return bestDay;
		}

		public List<Project> GetProjects()
		{
			var projects = new List<Project>();

			foreach (var eDay in GetDaysInTimeRange())
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

					lProject.TotalTime += eProject.GrandTotal.TotalSeconds;

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

		public List<ProjectData> Get(DataType dataType)
		{
			var data = new List<ProjectData>();

			foreach (var eDay in GetDaysInTimeRange())
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

				lData.TotalTime += eData.TotalSeconds;

				if (newData)
				{
					localData.Add(lData);
				}
			});

			return localData;
		}

		IEnumerable<Data.Day> GetDaysInTimeRange()
		{
			return _dataCore.Days.Where(x => x.Date.DateTime > StartDate && x.Date.DateTime < EndDate);
		}
		#endregion
	}
}
