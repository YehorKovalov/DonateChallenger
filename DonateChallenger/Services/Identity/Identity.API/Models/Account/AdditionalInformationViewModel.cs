using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Account;

public class AdditionalInformationViewModel
{
    public string ReturnUrl { get; set; }

    public string UserId { get; set; } = null!;

    public string? MerchantId { get; set; }

    [Required]
    public string Nickname { get; set; } = null!;

    public string Role { get; set; } = null!;

    public double? MinDonatePriceInDollars { get; set; }
}