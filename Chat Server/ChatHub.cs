using Microsoft.AspNetCore.SignalR;
using Models;

namespace Chat_Server;

public class ChatHub : Hub
{
    public async Task Send(Message message)
    {
        await Clients.Others.SendAsync("Receive", message);
    }
}