using System;
using System.Collections.Generic;
using System.Text;

namespace WTStats.Generator
{
	interface IGenerator
	{
		IGeneratedData GenerateData();
	}
}
