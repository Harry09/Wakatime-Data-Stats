using System;
using System.Collections.Generic;
using System.Text;

namespace WTStats.Generator.Generators
{
	interface IGenerator
	{
		GeneratedData Generate(WTDataAnalyzer wtDataAnalyzer, ILogger logger);
	}
}
