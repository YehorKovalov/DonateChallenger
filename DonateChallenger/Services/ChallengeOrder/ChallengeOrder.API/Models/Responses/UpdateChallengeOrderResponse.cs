namespace ChallengeOrder.API.Models.Responses;

public class UpdateChallengeOrderResponse<TResult>
{
    public TResult Data { get; set; } = default!;
}