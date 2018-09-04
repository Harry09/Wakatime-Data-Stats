using System;
using System.Collections.Generic;
using System.Text;

namespace WTStats.Core.Generators
{
	public interface IGenerator
	{
		GeneratorData Generate(DataAnalyzer wtDataAnalyzer, ILogger logger);
	}
}
