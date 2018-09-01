using System;
using System.Collections.Generic;
using System.Text;

namespace WTStats.Generator
{
	interface IGeneratedData
	{
		string GetData();

		void SaveData(string path);
	}
}
