using System.ComponentModel.DataAnnotations;
using Identity.API.Data.Identities;

namespace Identity.API.Models.AccountViewModels;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;

    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; init; } = null!;

    public ApplicationUser User { get; init; } = null!;

    public string? ReturnUrl { get; set; }
}