using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.MessageBus.Messages.Responses;
using MassTransit;

namespace ChallengesTemporaryStorage.API.Consumers;

public class MessageGetChallengesFromStorageRequestConsumer : IConsumer<MessageGetChallengesFromStorageRequest>
{
    private readonly IChallengesTemporaryStorageService _temporaryStorage;
    private readonly ILogger<MessageGetChallengesFromStorageRequestConsumer> _logger;

    public MessageGetChallengesFromStorageRequestConsumer(IChallengesTemporaryStorageService temporaryStorage, ILogger<MessageGetChallengesFromStorageRequestConsumer> logger)
    {
        _temporaryStorage = temporaryStorage;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MessageGetChallengesFromStorageRequest> context)
    {
        _logger.LogInformation($"{nameof(MessageGetChallengesFromStorageRequest)} ---> is arrived");

        if (!context.Message.GetChallenges)
        {
            _logger.LogInformation($"{nameof(MessageGetChallengesFromStorageRequest)} ---> GetChallenges: {context.Message.GetChallenges}. Consuming is stopped.");
        }

        var data = await _temporaryStorage.GetAsync<string>();

        await context.RespondAsync<MessageGetChallengesFromStorageResponse>(new
        {
            Data = data.Data
        });

        _logger.LogInformation($"{nameof(MessageGetChallengesFromStorageResponse)} ---> Sent Data: {data.Data}");
    }
}