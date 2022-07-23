namespace ChallengeOrder.API.Models.Responses;

public class GetPaginatedChallengeOrdersResponse<TData>
{
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int ChallengeOrdersPerPage { get; set; }
    public int CurrentPage { get; set; }

    public IEnumerable<TData> Data { get; set; } = null!;
}