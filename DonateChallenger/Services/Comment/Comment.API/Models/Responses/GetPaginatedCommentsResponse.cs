namespace Comment.API.Models.Responses;

public class GetPaginatedCommentsResponse<TData>
{
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CommentsPerPage { get; set; }
    public int CurrentPage { get; set; }

    public IEnumerable<TData> Data { get; set; } = null!;
}