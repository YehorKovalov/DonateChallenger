namespace Identity.API.Models.AccountViewModels.Responses;

public class SignUpResponseViewModel
{
    public string? RedirectUrl { get; set; }
    public bool SignUpIsSuccessful { get; set; }
}