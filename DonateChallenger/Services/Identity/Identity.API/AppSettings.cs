namespace Identity.API;

public class AppSettings
{
    public string? ChallengeCatalogUrl { get; set; } = null!;
    public string? ReactClientUrl { get; set; } = null!;
    public int? PermanentTokenLifetimeDays { get; set; }
    public int? TokenLifetimeMinutes { get; set; }
}