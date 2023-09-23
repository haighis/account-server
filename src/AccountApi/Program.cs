using KafkaFlow;
using KafkaFlow.Serializer;
using AccountApi.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafka(
    kafka => kafka
        .AddCluster(cluster=>
        {
            cluster
                .CreateTopicIfNotExists("register-user",3,3)
                .CreateTopicIfNotExists("active-user",3,3)
                .WithBrokers(new[] { "localhost:9092" })
            
                .AddProducer(
                    "register-user",
                    producer => producer
                        .DefaultTopic("register-user")
                        .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSerializer<JsonCoreSerializer>())

                        )
                .AddProducer(
                    "active-user",
                    producer => producer
                        .DefaultTopic("active-user")
                        .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSerializer<JsonCoreSerializer>())

                        )
                ;
        })
);

var app = builder.Build();

app.UseHttpsRedirection();
app.MapPost("/register", RegisterUserHandler.HandleAsync);
app.MapPost("/activate", ActivateUserHandler.HandleAsync);
app.Run();