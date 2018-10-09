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

		/// <summary>
		/// Path to data file
		/// </summary>
		public string DataFilePath { get; set; }

		public MainGenerator(string dataFilePath, ILogger logger)
		{
			this.logger = logger;
			DataFilePath = dataFilePath;
		}

		/// <summary>
		/// Adds generator
		/// </summary>
		/// <typeparam name="Generator">Generator class which inheric from <see cref="IGenerator"/></typeparam>
		public void AddGenerator<Generator>() where Generator : IGenerator, new()
		{
			logger.Info($"Adding {typeof(Generator).Name}...");

			generators.Add(new Generator());
		}

		/// <summary>
		/// Adds generator with custom constructor
		/// </summary>
		/// <typeparam name="Generator">Generator class which inheric from <see cref="IGenerator"/></typeparam>
		/// <param name="ctor">Constructor for generator class</param>
		public void AddGenerator<Generator>(Func<Generator> ctor) where Generator : IGenerator
		{
			logger.Info($"Adding {typeof(Generator).Name} with custom constructor...");

			generators.Add(ctor.Invoke());
		}

		/// <summary>
		/// Invokes generators to return list of generated data
		/// </summary>
		/// <returns>Data from generators</returns>
		public IEnumerable<GeneratorData> Generate()
		{
			logger.Info("Loading Wakatime Data...");

			if (string.IsNullOrWhiteSpace(DataFilePath))
			{
				logger.Error("You passed empty file path!");
				return null;
			}

			logger.Info("Parsing data...");

			var dataAnalyzer = new DataAnalyzer(DataFilePath);

			logger.Info("Started generating...");

			var generatorDatas = new List<GeneratorData>();

			foreach (var generator in generators)
			{
				logger.Info($"Invoking {generator.GetType().Name}...");

				try
				{
					var data = generator.Generate(dataAnalyzer, logger);

					if (data is null)
					{
						logger.Warning("Returned null value");

						continue;
					}

					generatorDatas.Add(data);
				}
				catch (Exception ex)
				{
					logger.Error(ex.Message);
				}

			}

			logger.Info("Done!");

			return generatorDatas;
		}
	}
}
