using ChatV1.Hubs;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;

namespace ChatV1.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _topic = "test_topic";
        private readonly IHubContext<ChatHub> _hubContext;

        public KafkaConsumerService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "consumer-group-B",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Null, string>(config).Build();
            consumer.Subscribe(_topic);

            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    if (consumeResult != null)
                    {
                        Console.WriteLine($"Message received from Kafka: {consumeResult.Message.Value}");

                        await _hubContext.Clients.All.SendAsync("ReceivedMessage", "Kafka", consumeResult.Message.Value);
                    }

                }
            }, cancellationToken);
        }
    } 
}
