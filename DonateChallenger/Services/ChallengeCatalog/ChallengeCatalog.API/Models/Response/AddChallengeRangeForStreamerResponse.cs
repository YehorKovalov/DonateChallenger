namespace ChallengeCatalog.API.Models.Response;

public class AddChallengeRangeForStreamerResponse
{
    public bool Succeeded { get; set; }
    public double ResultDonationPrice { get; set; }
    public int ChallengesAmount { get; set; }
}