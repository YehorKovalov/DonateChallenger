namespace Infrastructure.MessageBus.Messages.Responses;

public interface MessageFindUsernamesByIdsResponse
{
    public IDictionary<string, string> Data { get; set; }
}