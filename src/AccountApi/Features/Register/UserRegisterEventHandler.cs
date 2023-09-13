using KafkaFlow.Producers;

namespace AccountApi.Features.Register;

public static class UserRegisterEventHandler
{
    public static async Task<IResult> HandleAsync(
        IProducerAccessor producerAccessor,
        UserRegisterEventRequest request, CancellationToken cancellationToken)
    {
        var producer = producerAccessor.GetProducer("register-user-event");
        
        await producer.ProduceAsync(
            Guid.NewGuid().ToString(), // tenant key for Multi tenant system or Guid
            request
        );

        return Results.Accepted();
    }
}