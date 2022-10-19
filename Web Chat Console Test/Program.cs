using Microsoft.AspNetCore.SignalR.Client;
using Models;

const string SEND = "Send";

var connection = new HubConnectionBuilder().WithUrl("https://localhost:7044/chat").Build();

Console.ForegroundColor = ConsoleColor.Gray;

connection.On<Message>("Receive", message => {
    WriteConsoleMessage($"\n{message}\n", message.TextColor);
});

Console.Write("Введите ваш никнейм >> ");
var username = Console.ReadLine();

await connection.StartAsync();

Console.WriteLine("Вы вошли в чат. Введите '/exit' чтобы выйти");

var personalColor = (ConsoleColor)Random.Shared.Next(1, 15);

WriteConsoleMessage("Так выглядит цвет ваших сообщений у других пользователей\n", personalColor);

await SendServerMessageAsync($"{username} вошёл в чат");

while (connection.State == HubConnectionState.Connected)
{
    var msg = Console.ReadLine();

    if (msg == "/exit")
        break;

    if (string.IsNullOrWhiteSpace(msg))
    {
        Console.WriteLine("Вы ввели пустое сообщение! Низя так");
        continue;
    }

    await connection.InvokeAsync(SEND, new Message(msg, username, personalColor));
}

Console.WriteLine("Вы вышли из чата");

await SendServerMessageAsync($"{username} вышел из чата");

await connection.StopAsync();

async Task SendServerMessageAsync(string text)
{
    await connection.InvokeAsync(SEND, new Message(text, "SERVER", ConsoleColor.White));
}

static void WriteConsoleMessage(string msg, ConsoleColor foreground)
{
    Console.ForegroundColor = foreground;
    Console.WriteLine(msg);
    Console.ForegroundColor = ConsoleColor.Gray;
}