using Newtonsoft.Json;

namespace WTStats.Data
{
	public class Range
	{
		[JsonProperty("end")]
		public long End { get; set; }

		[JsonProperty("start")]
		public long Start { get; set; }
	}
}
