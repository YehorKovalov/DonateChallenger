namespace Comment.API.Models.Requests;

public class GetPaginatedCommentsRequest
{
    public int CurrentPage { get; set; }

    public int CommentsPerPage { get; set; }

    public int ChallengeId { get; set; }

    public string UserId { get; set; } = null!;
}