using Newtonsoft.Json;

using System;

namespace WTStats.Data
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

		public string ToString()
		{
			var ts = TimeSpan.FromSeconds(TotalSeconds);

			return ts.ToString();
		}
	}
}
