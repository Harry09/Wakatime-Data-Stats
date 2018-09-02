using System;
using System.Collections.Generic;
using System.Text;

namespace WTStats.Generator.Generators
{
	interface IGenerator
	{
		void Generate(WTDataAnalyzer wtDataAnalyzer, ILogger logger);
	}
}
