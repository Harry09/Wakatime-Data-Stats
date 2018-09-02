using System;
using System.Collections.Generic;
using System.Text;

namespace WTStats.Generator.Generators
{
	class TxtGenerator : IGenerator
	{
		public TxtGenerator()
		{

		}

		public void Generate(WTDataAnalyzer wtDataAnalyzer, ILogger logger)
		{
			logger.Info("Ale generuje wooo");
		}
	}
}
