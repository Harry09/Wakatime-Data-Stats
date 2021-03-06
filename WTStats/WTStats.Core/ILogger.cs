using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace WTStats.Core
{
	public interface ILogger
	{
		void Info(string txt, [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);

		void Warning(string txt, [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);

		void Error(string txt, [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);
	}
}
