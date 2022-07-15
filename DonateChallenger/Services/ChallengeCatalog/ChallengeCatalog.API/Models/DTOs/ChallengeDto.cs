namespace ChallengeCatalog.API.Models.DTOs;

public class ChallengeDto
{
    public long ChallengeId { get; set; }

    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public double DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;

    public DateTime CreatedTime { get; set; }
}