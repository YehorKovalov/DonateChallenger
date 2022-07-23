namespace ChallengeCatalog.API.Models.Response;

public class UpdateChallengeResponse<TData>
{
    public TData Data { get; set; } = default!;
}