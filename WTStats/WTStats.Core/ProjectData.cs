using System;

namespace WTStats.Core
{
	public class ProjectData
	{
		public string Name { get; set; }

		public long TotalSeconds { get; set; }

		public TimeSpan TimeSpan { get { return TimeSpan.FromSeconds(TotalSeconds); } }
	}
}
