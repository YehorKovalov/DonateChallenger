namespace Identity.API.Models.DTOs;

public class UserProfileDto
{
    public string Email { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string UserNickname { get; set; } = null!;
}