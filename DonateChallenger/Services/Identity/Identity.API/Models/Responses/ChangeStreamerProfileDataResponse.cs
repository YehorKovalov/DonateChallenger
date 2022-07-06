namespace Identity.API.Models.Responses;

public class ChangeStreamerProfileDataResponse<TData>
{
    public TData ChangedData { get; set; } = default!;
    
    public IEnumerable<string> ValidationErrors { get; set; } = null!;

    public bool Succeeded { get; set; }
}