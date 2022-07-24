using ChallengeOrder.API.Services.Abstractions;
using Infrastructure.MessageBus.Enums;
using Infrastructure.MessageBus.Messages;
using MassTransit;

namespace ChallengeOrder.API.Consumers;

public class MessageAddingChallengesStatusAndPaymentIdConsumer : IConsumer<MessageAddingChallengesStatusAndPaymentId>
{
    private readonly IChallengeOrderService _challengeOrderService;
    private readonly ILogger<MessageAddingChallengesStatusAndPaymentIdConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageAddingChallengesStatusAndPaymentIdConsumer(
        IChallengeOrderService challengeOrderService,
        ILogger<MessageAddingChallengesStatusAndPaymentIdConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _challengeOrderService = challengeOrderService;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<MessageAddingChallengesStatusAndPaymentId> context)
    {
        var message = context.Message;
        _logger.LogInformation($"MessageAddingChallengesAndPaymentId ---> {nameof(message.PaymentId)}: {message.PaymentId}; {nameof(message.AddingIsSucceeded)}: {message.AddingIsSucceeded};");

        if (!message.AddingIsSucceeded || string.IsNullOrWhiteSpace(message.PaymentId))
        {
            _logger.LogError("MessageAddingChallengesAndPaymentId ---> Message state is not valid");
            return;
        }

        var result = await _challengeOrderService.AddChallengeOrderAsync(message.PaymentId, message.ChallengesAmount, message.ResultDonationPrice);
        if (result.Succeeded)
        {
            _logger.LogInformation("Order is succeeded");
        }
        else
        {
            _logger.LogError("Order is failed");
        }

        var orderStatus = result.Succeeded ? ChallengeOrderStatus.Created : ChallengeOrderStatus.Canceled;
        await _publishEndpoint.Publish<MessageChallengeOrderStatus>(new
        {
            ChallengeOrderStatus = orderStatus
        });

        _logger.LogInformation($"MessageChallengeOrderStatus ---> Status is sent with value: {orderStatus}");
    }
}