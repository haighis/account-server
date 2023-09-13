using AccountApi.Features.Register;
using KafkaFlow;
using KafkaFlow.Serializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafka(
    kafka => kafka
        .AddCluster(cluster=>
        {
            cluster
                .WithBrokers(new[] { "localhost:9092" })
            //    .CreateTopicIfNotExists("sometopic", 3, 3)
                       .AddProducer(
                    "register-user-event",
                    producer => producer
                        .DefaultTopic("register-user-event")
                        .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSerializer<JsonCoreSerializer>()));
        })
);

var app = builder.Build();
app.UseHttpsRedirection();
// register user
app.MapPost("/account/register", UserRegisterEventHandler.HandleAsync);
// todo - once user registers produce message to send new user register email topic. topic name: send-user-active-account-email
// todo - consumer for topic send-user-active-account-email consumes topic and sends activate account email
// Activate Account
// 
// Authenticate
// authenticate against users ktable
app.Run();