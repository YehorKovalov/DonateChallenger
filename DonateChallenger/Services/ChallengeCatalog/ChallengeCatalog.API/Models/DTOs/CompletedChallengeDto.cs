namespace ChallengeCatalog.API.Models.DTOs;

public class CompletedChallengeDto
{
    public long ChallengeId { get; set; }

    public string? Title { get; set; }

    public string Description { get; set; } = null!;

    public decimal DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;

    public DateTime CreatedTime { get; set; }
}