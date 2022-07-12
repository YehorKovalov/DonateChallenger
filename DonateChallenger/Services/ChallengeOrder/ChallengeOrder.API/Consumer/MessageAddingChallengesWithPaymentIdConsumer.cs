using ChallengeOrder.API.Services.Abstractions;
using Infrastructure.MessageBus.Enums;
using Infrastructure.MessageBus.Messages;

namespace ChallengeOrder.API.Consumer;

public class MessageAddingChallengesAndPaymentIdConsumer : IConsumer<MessageAddingChallengesAndPaymentId>
{
    private readonly IChallengeOrderService _challengeOrderService;
    private readonly ILogger<MessageAddingChallengesAndPaymentIdConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageAddingChallengesAndPaymentIdConsumer(
        IChallengeOrderService challengeOrderService,
        ILogger<MessageAddingChallengesAndPaymentIdConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _challengeOrderService = challengeOrderService;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<MessageAddingChallengesAndPaymentId> context)
    {
        var message = context.Message;
        _logger.LogInformation($"MessageAddingChallengesAndPaymentId ---> {nameof(message.PaymentId)}: {message.PaymentId}; {nameof(message.AddingIsSucceeded)}: {message.AddingIsSucceeded};");
        var result = await _challengeOrderService.AddChallengeOrderAsync(message.PaymentId, message.ChallengesAmount, message.ResultDonationPrice);
        if (result.Succeeded)
        {
            _logger.LogInformation("Order is succeeded");
        }
        else
        {
            _logger.LogInformation("Order is failed");
        }

        await _publishEndpoint.Publish<MessageChallengeOrderStatus>(new
        {
            ChallengeOrderStatus = result.Succeeded ? ChallengeOrderStatus.Created : ChallengeOrderStatus.Canceled
        });
    }
}