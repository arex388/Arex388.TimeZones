namespace System {
    public static class DateTimeExtensions {
        public static DateTime? AtTimeZone(
            this DateTime? value,
            Arex388.TimeZones.TimeZone timeZone) {
            if (!value.HasValue) {
                return null;
            }

            return value.Value.AtTimeZone(timeZone);
        }

        public static DateTime AtTimeZone(
            this DateTime value,
            Arex388.TimeZones.TimeZone timeZone) => new DateTimeOffset(value).AtTimeZone(timeZone).DateTime;
    }
}