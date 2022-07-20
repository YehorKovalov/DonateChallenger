namespace Comment.API.Models.Responses;

public class GetCommentByCommentIdResponse<TComment>
{
    public TComment? Data { get; set; }
}