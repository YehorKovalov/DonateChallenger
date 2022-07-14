namespace Identity.API.Models.DTOs;

public class SearchedStreamerByNicknameDto
{
    public string StreamerId { get; set; }
    public string StreamerNickname { get; set; }
    public string MerchantId { get; set; }
    public double MinDonatePrice { get; set; }
}