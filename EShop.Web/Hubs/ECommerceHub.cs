using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EShop.Web.Hubs
{
    public class ECommerceHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}