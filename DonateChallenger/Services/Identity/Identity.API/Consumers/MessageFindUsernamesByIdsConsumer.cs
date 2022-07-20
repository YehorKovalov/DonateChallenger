using Identity.API.Services.Abstractions;
using Infrastructure.MessageBus.Messages.Requests;
using Infrastructure.MessageBus.Messages.Responses;
using MassTransit;

namespace Identity.API.Consumers;

public class MessageFindUsernamesByIdsConsumer : IConsumer<MessageFindUsernamesByIdsRequest>
{
    private readonly ILogger<MessageFindUsernamesByIdsConsumer> _logger;
    private readonly IUserService _userService;

    public MessageFindUsernamesByIdsConsumer(ILogger<MessageFindUsernamesByIdsConsumer> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<MessageFindUsernamesByIdsRequest> context)
    {
        var message = context.Message;
        _logger.LogInformation($"{nameof(MessageFindUsernamesByIdsRequest)} ---> message is received");

        if (context.Message == null)
        {
            _logger.LogInformation($"{nameof(MessageFindUsernamesByIdsRequest)} ---> message data is null. Method stopped");
            return;
        }

        var usernamesWithIds = await _userService.GetUsernamesByUsersIdsAsync(message.Data);

        await context.RespondAsync<MessageFindUsernamesByIdsResponse>(new
        {
            Data = usernamesWithIds.Data
        });

        _logger.LogInformation($"{nameof(MessageFindUsernamesByIdsRequest)} ---> Usernames with ids message has been sent");
    }
}