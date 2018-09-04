using CommandLine;

namespace WTStats
{
	class CommandLineOptions
	{
		[Value(0, MetaName ="Input file", HelpText = "Input file to be processed", Required = true)]
		public string FilePath { get; set; }

		[Option('o', "output", HelpText = "Output directory", Default = "")]
		public string OutputDirectory { get; set; }

		[Option("gentxt", HelpText = "Generates txt file")]
		public bool GenerateTxt { get; set; }

		[Option("genmd", HelpText = "Generates markdown")]
		public bool GenerateMarkdown { get; set; }

		[Option("genhtml", HelpText = "Generates HTML")]
		public bool GenerateHTML { get; set; }
	}
}
