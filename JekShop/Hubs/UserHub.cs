using Microsoft.AspNetCore.SignalR;

namespace JekShop.Hubs
{
    public class UserHub : Hub
    {
        public static int TotalViews { get; set; } = 0;

        public async Task NewWindowLoaded()
        {
            TotalViews++;
            await Clients.All.SendAsync("UpdateTotalViews", TotalViews);
        }

        // метод для чата
        public async Task SendMessage(string subject, string body)
        {
            await Clients.All.SendAsync("ReceiveMessage", subject, body);
        }
    }
}
