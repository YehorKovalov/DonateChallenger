namespace ChallengeOrder.API.Models.Responses;

public class AddChallengeOrderResponse<TId>
{
    public bool Succeeded { get; set; }
    public IEnumerable<string>? ErrorMessages { get; set; }
    public TId? OrderId { get; set; }
}