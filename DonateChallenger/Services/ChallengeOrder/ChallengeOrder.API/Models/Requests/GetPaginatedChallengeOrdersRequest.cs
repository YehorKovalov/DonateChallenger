namespace ChallengeOrder.API.Models.Requests;

public class GetPaginatedChallengeOrdersRequest
{
    public int CurrentPage { get; set; }
    public int OrdersPerPage { get; set; }
}