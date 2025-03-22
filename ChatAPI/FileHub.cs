using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ChatAPI
{
    public class FileHub : Hub
    {
        //in memory 
        private static readonly ConcurrentDictionary<string, HashSet<string>> _sessions = new(); 

        //creating session
        public async Task<string> CreateSession()
        {
            string sessionId = Guid.NewGuid().ToString(); 

            // Create a new session and add the current user to it
            var connectedClients = new HashSet<string> { Context.ConnectionId };
            _sessions[sessionId] = connectedClients;

            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            Console.WriteLine($"New session created: {sessionId} by connection {Context.ConnectionId}");

            return sessionId;
        }

        public async Task<object> JoinSpecificSession(string sessionId, string userName)
        {
            if (!_sessions.ContainsKey(sessionId))
            {
                return new { success = false, message = "Session not found" };
            }

            _sessions[sessionId].Add(Context.ConnectionId);

            // Add the user to the SignalR group and send other members message
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            await Clients.Group(sessionId).SendAsync("PeerJoined", Context.ConnectionId, userName);

            Console.WriteLine($"Connection {Context.UserIdentifier} {Context.ConnectionId} joined session: {sessionId}");
            //Console.WriteLine($"{JsonSerializer.Serialize(Context)}");
            return new { success = true, message = "Successfully joined session" };
        }

 
        //send offer from one candidate to other
        public async Task SendOffer(string sessionId, object offer)
        {
            await Clients.OthersInGroup(sessionId).SendAsync("ReceiveOffer", offer, Context.ConnectionId);
        }
     
        //reply with answer
        public async Task SendAnswer(string sessionId, object answer)
        {
            await Clients.OthersInGroup(sessionId).SendAsync("ReceiveAnswer", answer, Context.ConnectionId);
        }

        //ice candidates 
        public async Task SendIceCandidate(string sessionId, object candidate)
        {
            await Clients.OthersInGroup(sessionId).SendAsync("ReceiveIceCandidate", candidate, Context.ConnectionId);
        }
    }
}
