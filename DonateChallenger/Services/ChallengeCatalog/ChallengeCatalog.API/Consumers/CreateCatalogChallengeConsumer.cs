using ChallengeCatalog.API.Services.Abstractions;
using Infrastructure.MessageBus.Messages;
using MassTransit;

namespace ChallengeCatalog.API.Consumers;

public class CreateCatalogChallengeConsumer : IConsumer<CreateCatalogChallengeRequestMessage>
{
    private readonly IChallengeService _challengeService;
    private readonly ILogger<CreateCatalogChallengeConsumer> _logger;
    public CreateCatalogChallengeConsumer(IChallengeService challengeService, ILogger<CreateCatalogChallengeConsumer> logger)
    {
        _challengeService = challengeService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateCatalogChallengeRequestMessage> context)
    {
        var message = context.Message;
        _logger.LogInformation($"{nameof(Consume)} ---> Message: description: {message.Description}; title: {message.Title}; DonateFrom: {message.DonateFrom}; DonatePrice: {message.DonatePrice}; StreamerId: {message.StreamerId};");

        var result = await _challengeService.AddChallengeForStreamerAsync(message.DonateFrom, message.StreamerId, message.DonatePrice, message.Description, message.Title);
        if (result?.ChallengeId == null)
        {
            var errorMessage = $"{nameof(Consume)} ---> Response is null or challengeId is null";
            _logger.LogError(errorMessage);
            await context.RespondAsync<CatalogChallengeCreatingResponseMessage>(new
            {
                Succeeded = false,
                ErrorMessages = new List<string> { errorMessage },
                ChallengeId = (long?)null
            });

            return;
        }

        await context.RespondAsync<CatalogChallengeCreatingResponseMessage>(new
        {
            Succeeded = true,
            ErrorMessages = (IEnumerable<string>?)null,
            ChallengeId = result!.ChallengeId
        });
    }
}