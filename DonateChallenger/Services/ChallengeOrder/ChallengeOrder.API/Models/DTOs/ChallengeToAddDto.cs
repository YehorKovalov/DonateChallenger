namespace ChallengeOrder.API.Models.DTOs;

public class ChallengeToAddDto
{
    public string Description { get; set; } = null!;

    public string StreamId { get; set; } = null!;

    public decimal DonatePrice { get; set; }

    public string DonateFrom { get; set; } = null!;

    public string? Title { get; set; }
}