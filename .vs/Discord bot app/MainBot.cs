using Discord;
using Discord.WebSocket;
using Discord_bot_app.MessageLogger;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Discord_bot_app;

public class InitializeDiscord
{
    protected static DiscordSocketClient Client;

    public async Task MainAsync(string taskname, MainPage.GuildUsers info = null)
    {
        Client = new DiscordSocketClient();
        Client.Log += Log;

        var token = "MTAwNDg4NjQ1MjY4Mjg5MTMzNQ.GikQmJ.Cca4A9cV0kZctRJYbzb8e6dghTADrrlZFrMbeo";

        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

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

        //_client.Ready += MessageLoggerMain._StartMessageLog;

        Debug.WriteLine(Client.ConnectionState);

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Debug.WriteLine(msg.ToString());

        return Task.CompletedTask;
    }
}