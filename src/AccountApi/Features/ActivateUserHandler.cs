using System;
using KafkaFlow.Producers;

namespace AccountApi.Features;
public static class ActivateUserHandler
{
    public static async Task<IResult> HandleAsync(
        IProducerAccessor producerAccessor,
        ActivateUserRequest request, CancellationToken cancellationToken)
    {
        var producer = producerAccessor.GetProducer("active-user");
        await producer.ProduceAsync(
            "key",
            request
        );
        return Results.Accepted();
    }
}

