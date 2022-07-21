namespace Identity.API.Models.DTOs;

public class StreamerProfileDto
{
    public string StreamerId { get; set; } = null!;
    public string StreamerNickname { get; set; } = null!;
    public double MinDonatePrice { get; set; }
    public string MerchantId { get; set; } = null!;
}