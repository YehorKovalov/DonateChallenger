using ChallengesTemporaryStorage.API.Services.Abstractions;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.MessageBus.Messages.Responses;
using MassTransit;

namespace ChallengesTemporaryStorage.API.Consumers;

public class GetChallengesFromStorageConsumer : IConsumer<MessageGetChallengesFromStorageRequest>
{
    private readonly IChallengesTemporaryStorageService _temporaryStorage;
    private readonly ILogger<MessageChallengeOrderStatusConsumer> _logger;

    public GetChallengesFromStorageConsumer(IChallengesTemporaryStorageService temporaryStorage, ILogger<MessageChallengeOrderStatusConsumer> logger)
    {
        _temporaryStorage = temporaryStorage;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MessageGetChallengesFromStorageRequest> context)
    {
        var data = _temporaryStorage.GetAsync<string>();
        await context.RespondAsync<MessageGetChallengesFromStorageResponse>(data);
    }
}