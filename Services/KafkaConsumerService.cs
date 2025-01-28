using Confluent.Kafka;

namespace ChatV1.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _topic = "test_topic";

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Null, string>(config).Build();
            consumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);

                Console.WriteLine($"Message received from Kafka: {consumeResult.Message.Value}");
            }
        }
    } 
}
