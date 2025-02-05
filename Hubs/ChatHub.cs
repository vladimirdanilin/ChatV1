using ChatV1.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatV1.Hubs
{
    public class ChatHub : Hub
    {
        private readonly KafkaProducerService _producerService;

        public ChatHub(KafkaProducerService producerService)
        {
            _producerService = producerService;
        }

        public async Task Send(string name, string message)
        {
            await _producerService.SendMessageAsync("test_topic", $"{name}: {message}");

            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}
