namespace ChallengeCatalog.API.Models.DTOs;

public class ChallengeForAddingDto
{
    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public double DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;

    public string StreamerId { get; set; } = null!;
}