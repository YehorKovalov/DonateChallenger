namespace Comment.API.Models.Responses;

public class DeleteCommentResponse<TData>
{
    public TData Data { get; set; } = default!;
}