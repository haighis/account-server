using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(configure => configure.AddConsole());
services.AddKafkaFlowHostedService(
    kafka => kafka
        .UseMicrosoftLog()
        .AddCluster(cluster =>
        {
            cluster
                // todo env var hostname
                .WithBrokers(new[] { "localhost:9092" })
             
                .AddConsumer(consumer =>
                    consumer
                        // todo env var topics
                        .Topic("register-user")
                        .WithGroupId("notif-register-user")
                        .WithBufferSize(100)
                        .WithWorkersCount(3)
                        .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                        .AddMiddlewares(middlewares => middlewares
                            .AddSerializer<JsonCoreSerializer>()
                            .AddTypedHandlers(handlers =>
                                handlers.AddHandler<RegisterUserHandler>()
                            )
                        )
                )
                .AddConsumer(consumer =>
                    consumer
                        .Topic("active-user")
                        .WithGroupId("notif-active-user")
                        .WithBufferSize(100)
                        .WithWorkersCount(3)
                        .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                        .AddMiddlewares(middlewares => middlewares
                            .AddSerializer<JsonCoreSerializer>()
                            .AddTypedHandlers(handlers =>
                                handlers.AddHandler<ActivateUserHandler>()
                            )
                        )
                )
                ;
        })
);

var provider = services.BuildServiceProvider();

var bus = provider.CreateKafkaBus();
await bus.StartAsync();

Console.WriteLine("Press key to exit");
Console.ReadKey();