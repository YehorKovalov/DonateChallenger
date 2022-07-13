namespace ChallengesTemporaryStorage.API.Models;

public class UpdateChallengesTemporaryStorageRequest<TData>
{
    public TData Data { get; set; } = default!;
}