using System;

namespace WTStats.Core
{
    public static class Helpers
    {
		public static string ToCustomString(this TimeSpan ts)
		{
			return $"{ts.TotalHours:F0}h {ts.Minutes}m {ts.Seconds}s";
		}
	}
}
