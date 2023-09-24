using Dapper;
using KafkaFlow.Producers;
using Npgsql;

namespace AccountApi.Features;
public static class RegisterUserHandler
{
    public static async Task<IResult> HandleAsync(
        IProducerAccessor producerAccessor,
        RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var canRegister = await HasUserAccount(request.Username, request.Email);
        // Check if the user already has an account
        if (canRegister) {
            return Results.BadRequest();
        }
        var producer = producerAccessor.GetProducer("register-user");

        await producer.ProduceAsync(
            request.Username,
            request
        );
        return Results.Accepted();
    }

    private async static Task<bool> HasUserAccount(string username, string email)
    {
        string cs = @"User ID=postgres;Password=postgres;Host=localhost;Port=5433;Database=MangoAccountDB";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        string commandText = $"SELECT username FROM public.app_user WHERE username = @username OR email = @email";
        var queryArgs = new { Username = username, Email = email };
        var user = await con.QueryAsync<List<string>>(commandText, queryArgs);
        if (user.Any()) {
            return true;
        }
        return false;
    }
}

