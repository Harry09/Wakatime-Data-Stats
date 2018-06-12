using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace WTParser.Data
{
    class WTData
	{
		[JsonProperty("days")]
		public List<Day> Days { get; set; }

		[JsonProperty("range")]
		public Range Range { get; set; }

		[JsonProperty("user")]
		public User User { get; set; }
	}
}
