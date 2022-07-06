using System.ComponentModel.DataAnnotations;
using Identity.API.Helpers;

namespace Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(ValidationConstants.NicknameMaxLength)]
        public string Nickname { get; set; }

        [Range(ValidationConstants.LogicalMinimumDonatePrice, ValidationConstants.LogicalMaximumMinimumDonatePrice)]
        public double MinDonatePriceInDollars { get; set; }
    }
}
