using Newtonsoft.Json;

namespace WTParser.Data
{
    class Data
    {
		[JsonProperty("digital")]
		public string Digital { get; set; }

		[JsonProperty("hours")]
		public long Hours { get; set; }

		[JsonProperty("minutes")]
		public long Minutes { get; set; }

		[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty("percent")]
		public double Percent { get; set; }

		[JsonProperty("seconds", NullValueHandling = NullValueHandling.Ignore)]
		public long? Seconds { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("total_seconds")]
		public long TotalSeconds { get; set; }
	}
}
