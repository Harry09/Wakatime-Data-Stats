using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace WTParser.Data
{
    class Project
    {
		[JsonProperty("branches")]
		public List<Data> Branches { get; set; }

		[JsonProperty("categories")]
		public List<Data> Categories { get; set; }

		[JsonProperty("dependencies")]
		public List<Data> Dependencies { get; set; }

		[JsonProperty("editors")]
		public List<Data> Editors { get; set; }

		[JsonProperty("entities")]
		public List<Data> Entities { get; set; }

		[JsonProperty("grand_total")]
		public Data GrandTotal { get; set; }

		[JsonProperty("languages")]
		public List<Data> Languages { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("operating_systems")]
		public List<Data> OperatingSystems { get; set; }
	}
}
