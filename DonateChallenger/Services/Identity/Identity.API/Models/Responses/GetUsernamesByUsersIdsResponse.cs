namespace Identity.API.Models.Responses;

public class GetUsernamesByUsersIdsResponse<TData>
{
    public TData Data { get; set; } = default!;
}