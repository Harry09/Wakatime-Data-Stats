using Newtonsoft.Json;

namespace WTParser.Data
{
	public class Range
	{
		[JsonProperty("end")]
		public long End { get; set; }

		[JsonProperty("start")]
		public long Start { get; set; }
	}
}
