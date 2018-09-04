using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTStats.Core.Generators
{
    public class GeneratorData
    {
		/// <summary>
		/// Name of data
		/// </summary>
		public string DataName { get; set; }

		/// <summary>
		/// Extension of data
		/// </summary>
		public string FileExtension { get; set; }

		/// <summary>
		/// Generated data
		/// </summary>
		public string Data { get; set; }
	}
}
