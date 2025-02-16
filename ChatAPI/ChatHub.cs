using ChatAPI.Data;
using ChatAPI.Models;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ChatAPI
{
    public class ChatHub : Hub
    {
        private SharedDBDictionary _sharedDB;
        public ChatHub(SharedDBDictionary sharedDB)
        {
            _sharedDB = sharedDB;
        }

        //public override Task OnConnectedAsync()
        //{
        //    var username = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extract username

        //    if (string.IsNullOrEmpty(username))
        //    {
        //        Console.WriteLine($" User is not authenticated! Connection ID: {Context.ConnectionId}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"User {username} connected with ConnectionId {Context.ConnectionId}");
        //    }

        //    return base.OnConnectedAsync();
        //}


        //public async Task JoinChat(UserConnection connection)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage","admin",$"{connection.UserName} has joined");
        //}

        public async Task JoinSpecificChatRoom(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
            await Clients.Group(connection.ChatRoom)
                .SendAsync("ReceiveMessage", "admin", $"{connection.UserName} has joined");
            
            //store the user's details in in-memory db, like a dictionary.

            if (_sharedDB.userPublickKeys.TryGetValue(connection.ChatRoom, out ConcurrentDictionary<string, string> users))
            {
                if (!users.TryGetValue(connection.UserName, out string userPublicKey ))
                {
                    users[connection.UserName] = connection.PublicKey;
                }
            }
            else
            {
                var publicKey = new ConcurrentDictionary<string, string>();
                publicKey[connection.UserName] = connection.PublicKey;
                _sharedDB.userPublickKeys[connection.ChatRoom] = publicKey;
            }

            await Clients.Group(connection.ChatRoom)
                .SendAsync("NewJoinee", _sharedDB.userPublickKeys[connection.ChatRoom]);
        }

        public async void SendMessage(UserConnection connection,  string message)
        {
            await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", connection.UserName, message);
        }

        public async void SendMessageToSpecificUser(UserConnection connection, string encryptedMessage, string userToSend)
        {
            if (connection.UserName != userToSend)
            {
                await Clients.User(userToSend).SendAsync("ReceiveEncryptedMessage", connection.UserName, encryptedMessage);
                //await Clients.User(userToSend).SendAsync("ReceiveMessage", connection.UserName, encryptedMessage);
            }
        }

        //add disconnect

        //check how to see all groups

    }
}
