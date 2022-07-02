namespace ChallengeCatalog.API.Models.Requests;

public class GetPaginatedStreamerChallengesRequest<T>
    where T : notnull
{
    public int CurrentPage { get; set; }

    public int ChallengesPerPage { get; set; }

    public string StreamerId { get; set; } = null!;

    public IDictionary<T, int>? Filters { get; set; }
}