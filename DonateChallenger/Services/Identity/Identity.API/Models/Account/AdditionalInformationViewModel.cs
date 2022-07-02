using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Account;

public class AdditionalInformationViewModel
{
    public string ReturnUrl { get; set; }
    public string UserId { get; set; } = null!;
    
    [Required]
    public string Nickname { get; set; } = null!;

    [Range(0.1, Double.MaxValue)]
    public double MinDonatePriceInDollars { get; set; }
}