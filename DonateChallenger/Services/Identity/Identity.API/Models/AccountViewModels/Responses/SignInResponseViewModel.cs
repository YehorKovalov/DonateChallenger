namespace Identity.API.Models.AccountViewModels;

public class SignInResponseViewModel
{
    public string ReturnUrl { get; set; } = null!;
    public bool SignInIsSuccessful { get; set; }
}