namespace Infrastructure.Seeding;

public class GetIdsResponse
{
    public int StreamersAmount { get; set; }
    public string[] Ids { get; set; } = null!;
}