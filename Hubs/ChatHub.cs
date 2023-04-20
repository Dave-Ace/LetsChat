using LetsChat.Models;
using Microsoft.AspNetCore.SignalR;

namespace LetsChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Login(string userId, string token)
        {
            await Clients.All.SendAsync("LoggedIn", userId, token);
        }

        public async Task Fetch(IList<ChatMessages> messages)
        {
            await Clients.All.SendAsync("fetchMessages", messages);
        }
    }
}
