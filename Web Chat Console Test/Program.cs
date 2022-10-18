using Microsoft.AspNetCore.SignalR.Client;
using Models;

const string SEND = "Send";

var connection = new HubConnectionBuilder().WithUrl("https://localhost:7044/chat").Build();

Console.ForegroundColor = ConsoleColor.Gray;

connection.On<Message>("Receive", message => {
    Console.ForegroundColor = message.TextColor;

    Console.WriteLine($"\n{message}\n");

    Console.ForegroundColor = ConsoleColor.Gray;
});

Console.Write("Введите ваш никнейм >> ");
var username = Console.ReadLine();

await connection.StartAsync();

Console.WriteLine("Вы вошли в чат. Введите '/exit' чтобы выйти");

var personalColor = (ConsoleColor)Random.Shared.Next(1, 15);

Console.ForegroundColor = personalColor;
Console.WriteLine("Так выглядит цвет ваших сообщений у других пользователей");
Console.ForegroundColor = ConsoleColor.Gray;

await SendServerMessageAsync($"{username} вошёл в чат");

while (true)
{
    var msg = Console.ReadLine();

    if (msg == "/exit")
        break;

    await connection.InvokeAsync(SEND, new Message(msg, username, personalColor));
}

Console.WriteLine("Вы вышли из чата");

await SendServerMessageAsync($"{username} вышел из чата");

await connection.StopAsync();

async Task SendServerMessageAsync(string text)
{
    await connection.InvokeAsync(SEND, new Message(text, "SERVER", ConsoleColor.White));
}