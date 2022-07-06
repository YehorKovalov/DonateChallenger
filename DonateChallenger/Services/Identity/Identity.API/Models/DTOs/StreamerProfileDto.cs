namespace Identity.API.Models.DTOs;

public class StreamerProfileDto
{
    public string? StreamerId { get; set; }
    public string? StreamerNickname { get; set; }
    public double? MinDonatePrice { get; set; }
}