using System.Collections.Generic;

using Newtonsoft.Json;

namespace WTStats.Core.Data
{
    public class DataCore
	{
		[JsonProperty("days")]
		public List<Day> Days { get; set; }

		[JsonProperty("range")]
		public Range Range { get; set; }

		[JsonProperty("user")]
		public User User { get; set; }
	}
}
