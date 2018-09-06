using CommandLine;

namespace WTStats
{
	class CommandLineOptions
	{
		[Value(0, MetaName ="Input file", HelpText = "Input file to be processed", Required = true)]
		public string FilePath { get; set; }

		[Option('o', "output", HelpText = "Output directory", Default = "")]
		public string OutputDirectory { get; set; }

		[Option("gen-txt", HelpText = "Generates txt file")]
		public bool GenerateTxt { get; set; }

		[Option("gen-html", HelpText = "Generates HTML", Default = true)]
		public bool GenerateHTML { get; set; }
	}
}
