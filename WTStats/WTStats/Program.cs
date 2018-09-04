using System;
using System.IO;

using CommandLine;

using WTStats.Core;
using WTStats.Core.Generators;

namespace WTStats
{
	class Program
	{
		static void Main(string[] args)
		{
			var logger = new Logger();

			Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(op =>
			{
				var mainGenerator = new MainGenerator(op.FilePath, logger);

				if (op.GenerateTxt)
					mainGenerator.AddGenerator<TxtGenerator>();

				var datas = mainGenerator.Generate();

				if (datas != null)
				{
					foreach (var data in datas)
					{
						string filePath = string.Empty;

						if (!string.IsNullOrWhiteSpace(op.OutputDirectory))
						{
							filePath = op.OutputDirectory + "/";
						}

						filePath += $"{data.DataName}.{data.FileExtension}";


						logger.Info($"Saving {filePath}...");

						File.WriteAllText(filePath, data.Data);
					}
				}
			});
		}
	}
}
