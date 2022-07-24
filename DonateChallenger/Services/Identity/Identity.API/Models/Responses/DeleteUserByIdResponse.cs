namespace Identity.API.Models.Responses;

public class DeleteUserByIdResponse<TResult>
{
    public TResult Data { get; set; } = default!;
}