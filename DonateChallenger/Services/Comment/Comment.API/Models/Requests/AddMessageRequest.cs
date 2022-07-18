namespace Comment.API.Models.Requests;

public class AddCommentRequest
{
    public string Message { get; set; } = null!;

    public long ChallengeId { get; set; }

    public string UserId { get; set; } = null!;
}