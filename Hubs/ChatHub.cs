using Microsoft.AspNetCore.SignalR;

namespace ChatV1.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string name, string message)
        { 
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}
