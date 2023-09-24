using AccountApi.Features;
using Confluent.Kafka;
using Dapper;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.Logging;
using Npgsql;

/// <summary>
/// Register User Handler Event - This event will
/// - consume register-user,
/// - insert activation request (app_activation_requests username, email, activation code, expiry date, inserted date)
/// - send activation email.
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
        var code = GenerateActivationCode(message.Username, message.Email);
        _logger.LogInformation("Register new user {Username}, Activation code: {code}",
                message.Username, code);
        return Task.CompletedTask;
    }

    public string GenerateActivationCode(string username, string email) {
        var guid = Guid.NewGuid();
        string cs = @"User ID=postgres;Password=postgres;Host=localhost;Port=5433;Database=MangoAccountDB";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        var stm = "INSERT INTO public.app_activation_requests(id,username,activation_code,email,inserted_at) VALUES(@id,@username,@code,@email,current_timestamp)";
        var anonymousObject = new
        {
            id = Guid.NewGuid(),
            Username = username,
            Code = guid.ToString(),
            Email = email
        };
         con.Execute(stm, anonymousObject);
        return guid.ToString();
    }
}