namespace ChallengeOrder.API.Models.Responses;

public class GetChallengeOrderByIdResponse<TData>
{
    public TData? Data { get; set; }
}