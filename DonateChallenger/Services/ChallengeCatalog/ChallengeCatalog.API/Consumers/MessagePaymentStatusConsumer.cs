using ChallengeCatalog.API.Models.DTOs;
using ChallengeCatalog.API.Services.Abstractions;
using Infrastructure.MessageBus.Messages;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.MessageBus.Messages.Responses;
using Infrastructure.Services.Abstractions;
using MassTransit;

namespace ChallengeCatalog.API.Consumers;

public class MessagePaymentStatusConsumer : IConsumer<MessagePaymentStatus>
{
    private readonly IChallengeCatalogService _challengeCatalogService;
    private readonly ILogger<MessagePaymentStatusConsumer> _logger;
    private readonly IJsonSerializerWrapper _jsonSerializer;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRequestClient<MessageGetChallengesFromStorageRequest> _getChallengesFromStorageRequestClient;

    public MessagePaymentStatusConsumer(
        IChallengeCatalogService challengeCatalogService,
        ILogger<MessagePaymentStatusConsumer> logger,
        IRequestClient<MessageGetChallengesFromStorageRequest> getChallengesFromStorageRequestClient,
        IJsonSerializerWrapper jsonSerializer,
        IPublishEndpoint publishEndpoint)
    {
        _challengeCatalogService = challengeCatalogService;
        _logger = logger;
        _jsonSerializer = jsonSerializer;
        _publishEndpoint = publishEndpoint;
        _getChallengesFromStorageRequestClient = getChallengesFromStorageRequestClient;
    }

    public async Task Consume(ConsumeContext<MessagePaymentStatus> context)
    {
        _logger.LogInformation($"{nameof(MessagePaymentStatus)} arrived with {context.Message.Succeeded}");

        if (!context.Message.Succeeded)
        {
            _logger.LogError($"{nameof(MessagePaymentStatus)} ---> Method stopped, status is false");
            return;
        }

        var response = await _getChallengesFromStorageRequestClient.GetResponse<MessageGetChallengesFromStorageResponse>(new
        {
            GetChallenges = context.Message.Succeeded
        });
        _logger.LogInformation($"{nameof(MessageGetChallengesFromStorageResponse)} ---> Data: {response.Message.Data}");

        var challengesForAdding = _jsonSerializer.Deserialize<IEnumerable<ChallengeForAddingDto>>(response.Message.Data).ToList();
        _logger.LogInformation($"Deserialized challenges ---> Arrived Storage Challenges amount: {challengesForAdding.Count}");

        var result = await _challengeCatalogService.AddChallengeRangeForStreamerAsync(challengesForAdding);

        await _publishEndpoint.Publish<MessageAddingChallengesStatusAndPaymentId>(new
        {
            AddingIsSucceeded = result.Succeeded,
            PaymentId = context.Message.PaymentId,
            ChallengesAmount = result.ChallengesAmount,
            ResultDonationPrice = result.ResultDonationPrice
        });
        _logger.LogInformation($"{nameof(MessageAddingChallengesStatusAndPaymentId)} is sent with data: {nameof(result.Succeeded)}: {result.Succeeded}; {nameof(context.Message.PaymentId)}: {context.Message.PaymentId};");
    }
}