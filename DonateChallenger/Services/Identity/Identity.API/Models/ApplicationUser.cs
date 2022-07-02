using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(40)]
        public string Nickname { get; set; }

        [Range(0.1, Double.MaxValue)]
        public double MinDonatePriceInDollars { get; set; }
    }
}
