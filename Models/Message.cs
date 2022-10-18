using System.Text.Json.Serialization;

namespace Models;

public class Message
{
    [JsonConstructor]
    public Message(string text, string username, ConsoleColor textColor)
    {
        Text = text;
        Username = username;
        TextColor = textColor;        
    }

    public string Username { get; private init; }

    public string Text { get; private init; }    

    public ConsoleColor TextColor { get; private init; }

    public override string ToString()
    {
        return $"{Username} [{DateTime.Now.ToShortTimeString()}] - {Text}";
    }
}