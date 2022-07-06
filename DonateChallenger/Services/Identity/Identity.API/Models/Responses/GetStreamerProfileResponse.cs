namespace Identity.API.Models.Responses;

public class GetStreamerProfileResponse<TData>
{
    public TData Data { get; set; } = default(TData)!;
}