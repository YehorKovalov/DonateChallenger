namespace ChallengeOrder.API.Data.Entities;

public class ChallengeOrderEntity
{
    public Guid ChallengeOrderId { get; set; }

    public long CatalogChallengeId { get; set; }

    public Guid PaymentId { get; set; }
}