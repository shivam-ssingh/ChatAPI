using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ChatAPI
{
    public class UsernameIdentifierProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            if (connection != null && connection.User != null && connection.User.Identity != null)
            {
                return ((System.Security.Claims.ClaimsIdentity)connection.User.Identity).Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            }
            return "";
            
        }
    }
}
