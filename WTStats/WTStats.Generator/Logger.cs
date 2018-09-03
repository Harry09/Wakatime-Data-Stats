using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using WTStats.Core;

namespace WTStats.Generator
{
	class Logger : ILogger
	{
		private void Log(string txt, string type, string fileName, int lineNumber)
		{
#if DEBUG
			fileName = Path.GetFileName(fileName);
			Console.WriteLine($"[{type}][{fileName}:{lineNumber}] {txt}");
#else
			Console.WriteLine($"[{type}] {txt}");
#endif
		}

		public void Info(string txt, [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
		{
			Log(txt, "Info", fileName, lineNumber);
		}

		public void Warning(string txt, [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
		{
			Log(txt, "Warn", fileName, lineNumber);
		}

		public void Error(string txt, [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
		{
			Log(txt, "Erro", fileName, lineNumber);
		}
	}
}
