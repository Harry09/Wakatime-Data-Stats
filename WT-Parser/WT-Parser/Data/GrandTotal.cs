using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace WTParser.Data
{
    class GrandTotal
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
	}
}
