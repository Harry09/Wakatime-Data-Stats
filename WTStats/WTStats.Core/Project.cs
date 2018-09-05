using System.Collections.Generic;

namespace WTStats.Core
{
	public class Project : ProjectData
	{
		public List<ProjectData> Languages { get; set; }

		public List<ProjectData> Editors { get; set; }
	}
}
