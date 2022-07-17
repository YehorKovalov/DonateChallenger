using System.ComponentModel.DataAnnotations;
using Identity.API.Helpers;

namespace Identity.API.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(ValidationConstants.NicknameMaxLength)]
        public string Nickname { get; set; }

        [Range(ValidationConstants.LogicalMinimumDonatePrice, ValidationConstants.LogicalMaximumMinimumDonatePrice)]
        public double MinDonatePriceInDollars { get; set; }

        public string MerchantId { get; set; }
    }
}
