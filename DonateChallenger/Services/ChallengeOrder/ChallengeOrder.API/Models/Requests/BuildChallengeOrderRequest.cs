using ChallengeOrder.API.Models.DTOs;

namespace ChallengeOrder.API.Models.Requests;

public class BuildChallengeOrderRequest
{
    public ChallengeToAddDto ChallengeToAdd { get; set; } = null!;
}