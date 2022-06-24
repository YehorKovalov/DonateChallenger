namespace Identity.API.Models.AccountViewModels;

public class LoginViewModel
{
    public string ReturnUrl { get; set; } = null!;
    public bool RememberMe { get; set; }
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}