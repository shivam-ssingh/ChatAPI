using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(UserConnection connection)
        {
            await Clients.All.SendAsync("ReceiveMessage","admin",$"{connection.UserName} has joined");
        }

        public async Task JoinSpecificChatRoom(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
            await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", "admin", $"{connection.UserName} has joined");
        }

        public async void SendMessage(string name, string message)
        {
            Clients.All.SendAsync("ReceiveMessage", name, message);
        }
    }
}
