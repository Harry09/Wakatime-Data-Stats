using System.Collections.Generic;

namespace WTStats.Core
{
	public class Project
	{
		public string Name { get; set; }

		public long TotalTime { get; set; }

		public List<ProjectData> Languages { get; set; }

		public List<ProjectData> Editors { get; set; }
	}
}
