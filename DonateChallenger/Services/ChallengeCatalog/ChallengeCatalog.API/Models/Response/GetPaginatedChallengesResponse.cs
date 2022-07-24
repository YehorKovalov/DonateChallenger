namespace ChallengeCatalog.API.Models.Response;

public class GetPaginatedChallengesResponse<TData>
{
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int ChallengesPerPage { get; set; }
    public int CurrentPage { get; set; }

    public IEnumerable<TData> Data { get; set; } = null!;
}