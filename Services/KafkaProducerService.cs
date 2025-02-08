using Confluent.Kafka;

namespace ChatV1.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerService()
        {
            //var bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKER") ?? "kafka-setup:9092";

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendMessageAsync(string topic, string jsonMessage)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });

            var jsonMessageDeserialized = System.Text.Json.JsonSerializer.Deserialize<ChatMessage>(jsonMessage);
            var senderName = jsonMessageDeserialized?.SenderName;
            var message = jsonMessageDeserialized?.Message;

            Console.WriteLine($"Mesage from sender {senderName}: {message} sent to topic: {topic}");
        }
    }
}
