using System;

using CommandLine;

using WTStats.Core;
using WTStats.Core.Generators;

namespace WTStats.Generator
{
	class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(op =>
			{
				var mainGenerator = new MainGenerator(op.FilePath, new Logger());

				if (string.IsNullOrWhiteSpace(op.OutputDirectory))
					mainGenerator.Directory = op.OutputDirectory;

				if (op.GenerateTxt)
					mainGenerator.AddGenerator<TxtGenerator>();

				mainGenerator.Generate();
			});
		}
	}
}
