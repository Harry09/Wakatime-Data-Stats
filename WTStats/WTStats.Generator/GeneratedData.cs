using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WTStats.Generator
{
	class GeneratedData
	{
		public string FileName { get; set; }

		public string Data { get; set; }

		public void Save(string directoryPath)
		{
			if (!string.IsNullOrWhiteSpace(directoryPath))
			{
				File.WriteAllText(directoryPath + "/" + FileName, Data);
			}
			else
			{
				File.WriteAllText(FileName, Data);
			}
		}
	}
}
