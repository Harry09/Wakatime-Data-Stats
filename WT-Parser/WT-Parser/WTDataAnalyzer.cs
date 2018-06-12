using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

using Newtonsoft.Json;

namespace WTParser
{
	class ProjectData
	{
		public string Name { get; set; }

		public long TotalTime { get; set; }
	}

	class Project
	{
		public string Name { get; set; }

		public long TotalTime { get; set; }

		public List<ProjectData> Languages { get; set; }

		public List<ProjectData> Editors { get; set; }
	}

    class WTDataAnalyzer
    {
		Data.WTData _wtData;

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

			_wtData = JsonConvert.DeserializeObject<Data.WTData>(json);
		}

		public WTDataAnalyzer(Data.WTData wtData) : this()
		{
			_wtData = wtData;
		}

		public DateTime GetStartDate()
		{
			return DateTimeOffset.FromUnixTimeSeconds(_wtData.Range.Start).DateTime;
		}

		public DateTime GetEndDate()
		{
			return DateTimeOffset.FromUnixTimeSeconds(_wtData.Range.End).DateTime;
		}

		public TimeSpan GetTotalTimeCoding()
		{
			long total = 0;

			foreach (var day in _wtData.Days.Where(x => x.Date.DateTime > StartDate && x.Date.DateTime < EndDate))
			{
				total += day.GrandTotal.TotalSeconds;
			}

			return TimeSpan.FromSeconds(total);
		}

		public Data.Day GetBestDay()
		{
			Data.Day bestDay = _wtData.Days
				.Where(x => x.Date.DateTime > StartDate && x.Date.DateTime < EndDate)
				.OrderByDescending(x => x.GrandTotal.TotalSeconds).First();

			return bestDay;
		}

		public List<Project> GetProjects()
		{
			var projects = new List<Project>();

			foreach (var eDay in _wtData.Days.Where(x => x.Date.DateTime > StartDate && x.Date.DateTime < EndDate))
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

			foreach (var eDay in _wtData.Days.Where(x => x.Date.DateTime > StartDate && x.Date.DateTime < EndDate))
			{
				var property = eDay.GetType().GetProperty(dataType.ToString());

				if (property != null)
				{
					data = MergeProjectData(property.GetValue(eDay) as List<Data.Data>, data);
				}
			}

			return data;
		}

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
	}
}
