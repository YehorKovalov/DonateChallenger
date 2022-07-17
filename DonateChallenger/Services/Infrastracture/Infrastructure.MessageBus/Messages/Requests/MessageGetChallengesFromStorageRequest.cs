namespace Infrastructure.MessageBus.Messages.Requests;

public interface MessageGetChallengesFromStorageRequest
{
    public bool GetChallenges { get; set; }
}