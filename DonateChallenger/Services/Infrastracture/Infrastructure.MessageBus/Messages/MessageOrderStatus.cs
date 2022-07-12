using Infrastructure.MessageBus.Enums;

namespace Infrastructure.MessageBus.Messages;

public interface MessageChallengeOrderStatus
{
    public ChallengeOrderStatus ChallengeOrderStatus { get; set; }
}