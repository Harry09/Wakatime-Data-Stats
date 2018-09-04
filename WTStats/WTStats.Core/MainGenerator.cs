using System;
using System.Collections.Generic;
using System.Text;

using WTStats.Core.Generators;

namespace WTStats.Core
{
	public class MainGenerator
	{
		List<IGenerator> generators = new List<IGenerator>();

		readonly ILogger logger;
		readonly string dataFilePath;

		public MainGenerator(string dataFilePath, ILogger logger)
		{
			this.logger = logger;
			this.dataFilePath = dataFilePath;
		}

		public void AddGenerator<Generator>() where Generator : Generators.IGenerator, new()
		{
			logger.Info($"Adding {typeof(Generator).Name}...");

			generators.Add(new Generator());
		}

		public void AddGenerator<Generator>(Func<Generator> ctor) where Generator : Generators.IGenerator
		{
			logger.Info($"Adding {typeof(Generator).Name} with custom constructor...");

			generators.Add(ctor.Invoke());
		}

		public IEnumerable<GeneratorData> Generate()
		{
			logger.Info("Loading Wakatime Data...");

			if (string.IsNullOrWhiteSpace(dataFilePath))
			{
				logger.Error("You passed empty file path!");
				return null;
			}

			logger.Info("Parsing data...");

			var dataAnalyzer = new DataAnalyzer(dataFilePath);

			logger.Info("Started generating...");

			var generatorDatas = new List<GeneratorData>();

			foreach (var generator in generators)
			{
				logger.Info($"Invoking {generator.GetType().Name}...");

				var data = generator.Generate(dataAnalyzer, logger);

				generatorDatas.Add(data);
			}

			logger.Info("Done!");

			return generatorDatas;
		}
	}
}
