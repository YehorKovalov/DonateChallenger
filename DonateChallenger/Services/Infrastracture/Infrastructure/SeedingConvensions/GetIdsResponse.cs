namespace Infrastructure.SeedingConvensions;

public class GetIdsResponse
{
    public int IdsAmount { get; set; }
    public string[] Ids { get; set; } = null!;
}