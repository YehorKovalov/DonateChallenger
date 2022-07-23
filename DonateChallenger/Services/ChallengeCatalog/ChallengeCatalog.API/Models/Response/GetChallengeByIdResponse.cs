namespace ChallengeCatalog.API.Models.Response;

public class GetChallengeByIdResponse<TData>
{
    public TData Data { get; set; } = default!;
}