namespace Infrastructure.MessageBus.Messages.Requests;

public interface MessageFindUsernamesByIdsRequest
{
    public IEnumerable<string> Data { get; set; }
}