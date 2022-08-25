using Discord;
using Discord.WebSocket;
using Discord_bot_app.MessageLogger;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Discord_bot_app.Startup;


public class InitializeDiscord
{
    
    public static DiscordSocketClient Client;

    public async Task MainAsync(string taskname)
    {
        Client = new DiscordSocketClient();
        Client.Log += Log;
        var token = "MTAwNDg4NjQ1MjY4Mjg5MTMzNQ.GbhrDF.zu43LNAmrLGHqqg-M_F9Tc2-PRMkNIWE7fT0Fo";
        
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

        Debug.WriteLine(Client.ConnectionState);

        await Task.Delay(-1);
    }

    //private async Task UserJoined(SocketGuildUser user)
    //{
    //    new MessageView().UserJoined();
    //}

    

    private Task Log(LogMessage msg)
    {
        Debug.WriteLine(msg.ToString());

        return Task.CompletedTask;
    }
}