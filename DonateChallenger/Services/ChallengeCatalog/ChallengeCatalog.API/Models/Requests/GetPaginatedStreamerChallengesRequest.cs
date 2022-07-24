namespace ChallengeCatalog.API.Models.Requests;

public class GetPaginatedStreamerChallengesRequest<TFilter, TSortBy>
    where TFilter : notnull
    where TSortBy : notnull
{
    public int CurrentPage { get; set; }

    public int ChallengesPerPage { get; set; }

    public string StreamerId { get; set; } = null!;

    public IDictionary<TFilter, int>? Filters { get; set; }
    public IDictionary<TSortBy, bool>? SortBy { get; set; }
}