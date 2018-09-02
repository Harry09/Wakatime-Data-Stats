using System;
using System.Collections.Generic;
using System.Text;

namespace WTStats.Generator
{
	class MainGenerator
	{
		List<Generators.IGenerator> generators = new List<Generators.IGenerator>();

		readonly ILogger logger;
		readonly string filePath;

		public string Directory { get; set; }

		public MainGenerator(string filePath, ILogger logger)
		{
			this.logger = logger;
			this.filePath = filePath;
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

		public void Generate()
		{
			logger.Info("Loading Wakatime Data...");

			if (string.IsNullOrWhiteSpace(filePath))
			{
				logger.Error("You passed empty file path!");
				return;
			}

			logger.Info("Parsing data...");

			var dataAnalyzer = new WTDataAnalyzer(filePath);

			logger.Info("Started generating...");

			foreach (var generator in generators)
			{
				logger.Info($"Invoking {generator.GetType().Name}...");

				generator.Generate(dataAnalyzer, logger);
			}
		}
	}
}
