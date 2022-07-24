namespace Identity.API.Models.Responses;

public class SearchStreamersByNicknameResponse<TData>
{
    public IEnumerable<TData> Data { get; set; } = null!;
}