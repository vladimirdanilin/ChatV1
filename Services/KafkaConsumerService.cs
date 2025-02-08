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
            //var bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKER") ?? "kafka-setup:9092";

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "consumer-group-A",
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
                        var receivedData = System.Text.Json.JsonSerializer.Deserialize<ChatMessage>(consumeResult.Message.Value);
                        
                        string senderId = receivedData?.SenderId;
                        string senderName = receivedData?.SenderName;
                        string message = receivedData?.Message;

                        Console.WriteLine($"Message received from {senderName}: {message}");

                        await _hubContext.Clients
                        .AllExcept(senderId)
                        .SendAsync("broadcastMessage", senderName, message);
                    }

                }
            }, cancellationToken);
        }
    } 
}
