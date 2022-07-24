namespace Identity.API.Models.Requests;

public class ManagerUpdateUserProfileRequest
{
    public string UserId { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
}