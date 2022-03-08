using System;

namespace NodaTime {
    internal static class OffsetExtensions {
        public static string ToFormattedString(
            this Offset offset) {
            var offsetHours = offset.Seconds / 60M / 60M;
            var offsetAbs = Math.Abs(offsetHours);
            var offsetSign = Math.Sign(offsetHours);

            var remainder = offsetAbs % 1;
            var sign = offsetSign switch {
                -1 => "-",
                _ => "+"
            };

            var hours = (int)(offsetAbs - remainder);
            var minutes = (int)(remainder * 60);

            return $"{sign}{hours:D2}:{minutes:D2}";
        }
    }
}