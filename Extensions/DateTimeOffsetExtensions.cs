namespace System {
    public static class DateTimeOffsetExtensions {
        public static DateTimeOffset? AtTimeZone(
            this DateTimeOffset? value,
            Arex388.TimeZones.TimeZone timeZone) {
            if (!value.HasValue) {
                return null;
            }

            return value.Value.AtTimeZone(timeZone);
        }

        public static DateTimeOffset AtTimeZone(
            this DateTimeOffset value,
            Arex388.TimeZones.TimeZone timeZone) {
            var utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
            var timeSpan = TimeSpan.Parse(timeZone.UtcOffset.Replace("+", null));
            var minutes = (utcOffset - timeSpan).TotalMinutes;

            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(value.AddMinutes(minutes), timeZone.WindowsId);
        }
    }
}