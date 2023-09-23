using System;
using KafkaFlow.Producers;

namespace AccountApi.Features;
public static class RegisterUserHandler
{
    public static async Task<IResult> HandleAsync(
        IProducerAccessor producerAccessor,
        RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var producer = producerAccessor.GetProducer("register-user");

        await producer.ProduceAsync(
            "key",
            request
        );

        return Results.Accepted();
    }
}

