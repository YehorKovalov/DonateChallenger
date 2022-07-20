namespace Identity.API.Models.Responses;

public class GetUserProfileResponse<TUserProfile>
{
    public TUserProfile Data { get; set; } = default!;
}