namespace Identity.API.Models.Requests;

public class ManagerUpdateStreamerProfileRequest
{
    public string UserId { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public double MinDonatePrice { get; set; }
    public string MerchantId { get; set; } = null!;
}