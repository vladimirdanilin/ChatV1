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

        public async Task Send(string senderName, string message)
        {
            var senderId = Context.ConnectionId;

            var chatMessage = new ChatMessage(senderId, senderName, message);

            var jsonMessage = System.Text.Json.JsonSerializer.Serialize(chatMessage);

            await _producerService.SendMessageAsync("test_topic", jsonMessage);

            await Clients.All.SendAsync("broadcastMessage", senderName, message);
        }
    }
}
