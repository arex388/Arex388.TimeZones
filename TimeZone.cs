namespace Arex388.TimeZones;

public sealed class TimeZone {
    public string Abbreviation { get; set; }
    public string IanaId { get; set; }
    public bool IsDaylightSavings { get; set; }
    public string UtcOffset { get; set; }
    public string WindowsId { get; set; }
}