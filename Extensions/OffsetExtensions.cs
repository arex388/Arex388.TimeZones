using System;
using System.Collections.Generic;

namespace NodaTime {
	public static class OffsetExtensions {
		private static readonly IDictionary<int, string> Signs = new Dictionary<int, string> {
			{ -1, "-" },
			{ 0, "+" },
			{ 1, "+" }
		};

		public static string ToFormattedString(
			this Offset offset) {
			var offsetHours = offset.Seconds / 60M / 60M;
			var offsetAbs = Math.Abs(offsetHours);
			var offsetSign = Math.Sign(offsetHours);

			var remainder = offsetAbs % 1;
			var sign = Signs[offsetSign];

			var hours = (int)(offsetAbs - remainder);
			var minutes = (int)(remainder * 60);

			return $"{sign}{hours:D2}:{minutes:D2}";
		}
	}
}