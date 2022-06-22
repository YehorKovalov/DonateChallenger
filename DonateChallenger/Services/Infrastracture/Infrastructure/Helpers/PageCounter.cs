namespace Infrastructure.Helpers;

public static class PageCounter
{
    public static int Count(long totalCount, int entitiesPerPage)
    {
        var result = (double)totalCount / entitiesPerPage;
        var ceiledResult = Math.Ceiling(result);
        return Convert.ToInt32(ceiledResult);
    }
}