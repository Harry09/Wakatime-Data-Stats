using Newtonsoft.Json;

using System;

using WTStats.Core;

namespace WTStats.Core.Data
{
	public class GrandTotal
	{
		[JsonProperty("digital")]
		public string Digital { get; set; }

		[JsonProperty("hours")]
		public long Hours { get; set; }

		[JsonProperty("minutes")]
		public long Minutes { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("total_seconds")]
		public long TotalSeconds { get; set; }

		[JsonIgnore]
		public TimeSpan TimeSpan { get { return TimeSpan.FromSeconds(TotalSeconds); } }

		public override string ToString()
		{
			return TimeSpan.ToCustomString();
		}
	}
}
