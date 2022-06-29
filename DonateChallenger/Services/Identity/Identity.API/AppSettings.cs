namespace Identity.API;

public class AppSettings
{
    public string? ChallengeCatalogUrl { get; set; }
    public string? ReactClientUrl { get; set; }
    public string? GlobalUrl { get; set; }
    public int? PermanentTokenLifetimeDays { get; set; }
    public int? TokenLifetimeMinutes { get; set; }
}