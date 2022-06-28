using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModels;

public class LoginViewModel
{
    public string ReturnUrl { get; set; } = null!;
    public bool RememberMe { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}