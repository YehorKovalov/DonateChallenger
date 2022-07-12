namespace ChallengeOrder.API.Models.Responses;

public class AddChallengeOrderResponse<TId>
{
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }
    public TId? OrderId { get; set; }
}