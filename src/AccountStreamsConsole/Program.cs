// See https://aka.ms/new-console-template for more information
using System.IO;
using AccountStreamsConsole;
using Newtonsoft.Json.Linq;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.State;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.Table;

var config = new StreamConfig<StringSerDes, StringSerDes>();

config.ApplicationId = "test-app";
config.BootstrapServers = "localhost:9092";
//Console.WriteLine("offset--------------------------------------- " + config.AutoOffsetReset.Value);

StreamBuilder builder = new StreamBuilder();

//builder.Table("QUERYABLE_USERS", new StringSerDes(), new JsonSerDes<Test>())
//    .ToStream().Print(Printed<string, Test>.ToOut());

//builder.Table("QUERYABLE_USERS", new StringSerDes(), new JsonSerDes<Test>())    .ToStream().Print(Printed<string, Test>.ToOut());

//var list = builder.Stream("register-user", new StringSerDes(), new JsonSerDes<Test>());

builder.Stream("register-user", new StringSerDes(), new JsonSerDes<Test>()).Print(Printed<string, Test>.ToOut());


//// Apply the foreach transformation to print each record to the console
//usersStream.foreach ((key, value)->System.out.printf("Key: %s, Value: %s\n", key, value));

//KafkaStreams streams = new KafkaStreams(topology, props);

// How a k/v store is done in java
//IReadOnlyKeyValueStore<Object, Object> store = streams.store("storeName", QueryableStoreTypes.keyValueStore());

//KeyValueIterator<Object, Object> iterator = store.all();

Topology t = builder.Build();
KafkaStream stream = new KafkaStream(t, config);

Console.CancelKeyPress += (o, e) => {
    stream.Dispose();
};

await stream.StartAsync();