using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace WTStats.Core.Data
{
    public class Day
    {
		[JsonProperty("categories")]
		public List<Data> Categories { get; set; }

		[JsonProperty("date")]
		public DateTimeOffset Date { get; set; }

		[JsonProperty("dependencies")]
		public List<Data> Dependencies { get; set; }

		[JsonProperty("editors")]
		public List<Data> Editors { get; set; }

		[JsonProperty("grand_total")]
		public GrandTotal GrandTotal { get; set; }

		[JsonProperty("languages")]
		public List<Data> Languages { get; set; }

		[JsonProperty("operating_systems")]
		public List<Data> OperatingSystems { get; set; }

		[JsonProperty("projects")]
		public List<Project> Projects { get; set; }
	}
}
