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
	}
}
