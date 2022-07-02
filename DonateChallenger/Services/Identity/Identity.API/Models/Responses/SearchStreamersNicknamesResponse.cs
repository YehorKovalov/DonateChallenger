namespace Identity.API.Models.Responses;

public class SearchStreamersNicknamesResponse<TData>
{
    public IEnumerable<TData> Data { get; set; } = null!;
}