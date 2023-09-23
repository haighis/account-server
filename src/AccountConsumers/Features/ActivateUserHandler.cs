using AccountApi.Features;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.Logging;
using Dapper;
using Npgsql;

/// <summary>
/// Activate User Handler Event - This event will consume register-user and insert
/// to users postgres database table
/// </summary>
public class ActivateUserHandler : IMessageHandler<ActivateUserRequest>
{
    private readonly ILogger<ActivateUserHandler> _logger;

    public ActivateUserHandler(ILogger<ActivateUserHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IMessageContext context, ActivateUserRequest message)
    {
        string cs = @"User ID=postgres;Password=postgres;Host=localhost;Port=5433;Database=MangoAccountDB";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        var stm = "INSERT INTO public.app_user(id,username,password,enabled,email) VALUES(@id,@username,@password,@enabled,@email)";
        var anonymousObject = new
        {
            id = Guid.NewGuid(),
            Username = message.Username,
            Password = message.Password,
            Email = message.Email,
            Enabled = true
        };
        int n = con.Execute(stm,anonymousObject);
        //con.Close();
        //if (message.DueDate.HasValue)
        _logger.LogInformation("Activate user {Username}",
                message.Username);
        return Task.CompletedTask;
    }
}