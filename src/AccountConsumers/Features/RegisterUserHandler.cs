using AccountApi.Features;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.Logging;

/// <summary>
/// Register User Handler Event - This event will consume register-user and send activation email.
/// </summary>
public class RegisterUserHandler : IMessageHandler<RegisterUserRequest>
{
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(ILogger<RegisterUserHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IMessageContext context, RegisterUserRequest message)
    {
        // Send activation email
        //if (message.DueDate.HasValue)
            _logger.LogInformation("Register new user {Username}",
                message.Username);
        return Task.CompletedTask;
    }
}