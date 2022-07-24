namespace Identity.API.Models.Responses;

public class ManagerUpdateProfileResponse
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = null!;
}