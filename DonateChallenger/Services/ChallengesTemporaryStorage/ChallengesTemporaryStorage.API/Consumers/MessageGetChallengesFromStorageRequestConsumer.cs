using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure.MessageBus.Messages;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.MessageBus.Messages.Responses;
using MassTransit;

namespace ChallengesTemporaryStorage.API.Consumers;

public class MessageGetChallengesFromStorageRequestConsumer : IConsumer<MessageGetChallengesFromStorageRequest>
{
    private readonly IChallengesTemporaryStorageService _temporaryStorage;
    private readonly ILogger<MessageChallengeOrderStatusConsumer> _logger;

    public MessageGetChallengesFromStorageRequestConsumer(IChallengesTemporaryStorageService temporaryStorage, ILogger<MessageChallengeOrderStatusConsumer> logger)
    {
        _temporaryStorage = temporaryStorage;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MessageGetChallengesFromStorageRequest> context)
    {
        _logger.LogInformation($"{nameof(MessageGetChallengesFromStorageRequest)} ---> is arrived");
        var data = await _temporaryStorage.GetAsync<string>();

        _logger.LogInformation($"{nameof(MessageGetChallengesFromStorageResponse)} ---> Data: {data.Data}");
        var response = new { Data = data.Data };

        await context.RespondAsync<MessageGetChallengesFromStorageResponse>(response);
    }
}