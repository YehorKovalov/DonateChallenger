namespace Comment.API.Models.DTOs;

public class CommentDto
{
    public long CommentId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Date { get; set; }

    public long ChallengeId { get; set; }

    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
}