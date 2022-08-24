using Discord;
using Discord.WebSocket;
using Discord_bot_app.MessageLogger;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Discord_bot_app.Startup;

public class InitializeDiscord
{
    protected static DiscordSocketClient Client;

    public async Task MainAsync(string taskname)
    {
        Client = new DiscordSocketClient();
        Client.Log += Log;
        var token = "MTAwNDg4NjQ1MjY4Mjg5MTMzNQ.GdOiSJ.M0LSdZo4v8OnXUjDQWstjLVCuJnoNKb9VYhGRQ";
        
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        Client.MessageReceived += MessageReceived;



        if (taskname == "MessageLogger")
        {
            Client.Ready += MessageLoggerMain._StartMessageLog;
        }

        if (taskname == "Initialize Guild")
        {
            Client.Ready += MainPage.Guildinfo.GetGuild;
        }

        if (taskname == "SendMessage")
        {
            Client.Ready += MainPage.Guildinfo.GetGuild;
        }


        Debug.WriteLine(Client.ConnectionState);

        await Task.Delay(-1);
    }

    private async Task MessageReceived(SocketMessage message)
    {
        Debug.WriteLine(message.Content);
    }

    private Task Log(LogMessage msg)
    {
        Debug.WriteLine(msg.ToString());

        return Task.CompletedTask;
    }
}