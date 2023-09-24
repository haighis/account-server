using System;
using Dapper;
using KafkaFlow.Producers;
using Npgsql;

namespace AccountApi.Features;
public static class ActivateUserHandler
{
    public static async Task<IResult> HandleAsync(
        IProducerAccessor producerAccessor,
        ActivateUserRequest request, CancellationToken cancellationToken)
    {
        // Activate if user account is not enabled 
        
        var isEnabled = await UserAccountEnabled(request.Username);
        if (isEnabled) {
            return Results.BadRequest();
        }

        // todo activate if the activation code is valid and not expired
        var producer = producerAccessor.GetProducer("active-user");
        await producer.ProduceAsync(
            request.Username,
            request
        );
        return Results.Accepted();
    }

    private async static Task<bool> UserAccountEnabled(string username)
    {
        string cs = @"User ID=postgres;Password=postgres;Host=localhost;Port=5433;Database=MangoAccountDB";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        string commandText = $"SELECT username FROM public.app_user WHERE username = @username AND enabled is true";
        var queryArgs = new { Username = username };
        var user = await con.QuerySingleAsync<string>(commandText, queryArgs);
        if (user.Any())
        {
            return true;
        }
        return false;
    }
}

