using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure.MessageBus.Enums;
using Infrastructure.MessageBus.Messages;
using MassTransit;

namespace ChallengesTemporaryStorage.API.Consumers;

public class MessageChallengeOrderStatusConsumer : IConsumer<MessageChallengeOrderStatus>
{
    private readonly IChallengesTemporaryStorageService _temporaryStorage;
    private readonly ILogger<MessageChallengeOrderStatusConsumer> _logger;

    public MessageChallengeOrderStatusConsumer(IChallengesTemporaryStorageService temporaryStorage, ILogger<MessageChallengeOrderStatusConsumer> logger)
    {
        _temporaryStorage = temporaryStorage;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MessageChallengeOrderStatus> context)
    {
        var status = context.Message.ChallengeOrderStatus;
        _logger.LogInformation($"---> MessageChallengeOrderStatusConsumer's received message with OrderStatus: {status}");
        if (status == ChallengeOrderStatus.Created)
        {
            var basketIsDeleted = await _temporaryStorage.DeleteAsync();
            _logger.LogInformation(basketIsDeleted
                ? "MessageChallengeOrderStatusConsumer ---> Basket is deleted while consuming"
                : "MessageChallengeOrderStatusConsumer ---> Basket is not deleted consuming");
        }
        else if (status == ChallengeOrderStatus.Canceled)
        {
            _logger.LogError("MessageChallengeOrderStatusConsumer ---> OrderStatus = status");
        }
    }
}