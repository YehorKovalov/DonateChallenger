namespace Comment.API.Models.Requests;

public class UpdateCommentRequest
{
    public long CommentId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Date { get; set; }

    public long ChallengeId { get; set; }

    public string UserId { get; set; } = null!;
}