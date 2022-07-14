using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IJsonSerializerWrapper _jsonSerializer;
    private readonly IRequestClient<MessageGetChallengesFromStorageRequest> _getChallengesFromStorageRequestClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public MessagePaymentStatusConsumer(
        IChallengeCatalogService challengeCatalogService,
        ILogger<MessagePaymentStatusConsumer> logger,
        IRequestClient<MessageGetChallengesFromStorageRequest> getChallengesFromStorageRequestClient,
        IJsonSerializerWrapper jsonSerializer,
        IPublishEndpoint publishEndpoint,
        IMapper mapper)
    {
        _challengeCatalogService = challengeCatalogService;
        _logger = logger;
        _getChallengesFromStorageRequestClient = getChallengesFromStorageRequestClient;
        _jsonSerializer = jsonSerializer;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<MessagePaymentStatus> context)
    {
        _logger.LogInformation($"MessagePaymentStatus arrived with {context.Message.Succeeded}");

        var response = await _getChallengesFromStorageRequestClient.GetResponse<MessageGetChallengesFromStorageResponse>(new { });
        _logger.LogInformation($"MessageGetChallengesFromStorageResponse ---> Data: {response.Message.Data}");

        var challengesForAdding = _jsonSerializer.Deserialize<IEnumerable<ChallengeForAddingDto>>(response.Message.Data).ToList();
        _logger.LogInformation($"Deserialized challenges ---> Arrived Storage Challenges amount: {challengesForAdding.Count}");

        var result = await _challengeCatalogService.AddChallengeRangeForStreamerAsync(challengesForAdding);

        await _publishEndpoint.Publish<MessageAddingChallengesAndPaymentId>(new
        {
            AddingIsSucceeded = result.Succeeded,
            PaymentId = context.Message.PaymentId,
            ChallengesAmount = result.ChallengesAmount,
            ResultDonationPrice = result.ResultDonationPrice
        });
        _logger.LogInformation($"MessageAddingChallengesWithPaymentId is sent with data: {nameof(result.Succeeded)}: {result.Succeeded}; {nameof(context.Message.PaymentId)}: {context.Message.PaymentId};");
    }
}