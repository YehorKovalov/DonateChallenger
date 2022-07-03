namespace Infrastructure.SeedingConventions;

public static class StreamerIdsProvider
{
    private const int Amount = 3;

    static StreamerIdsProvider()
    {
        Response = new GetIdsResponse()
        {
            IdsAmount = Amount,
            Ids = GetIds()
        };
    }

    public static GetIdsResponse Response { get; }

    private static string[] GetIds()
    {
        var ids = new string[Amount];
        ids[0] = "8eb28a0c-6a10-44d8-b1d0-95a08ccef348";
        ids[1] = "c3a03cbd-adc4-44c1-9cee-acd544842e07";
        ids[2] = "df185d57-a09b-4b09-92d8-29fba10d103f";

        return ids;
    }
}