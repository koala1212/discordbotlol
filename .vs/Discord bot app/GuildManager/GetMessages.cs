using Discord;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Discord_bot_app.GuildManager;

internal class GetMessages : InitializeDiscord
{
    internal static IGuildUser GuildUser;

    internal static async Task GetUser(IEnumerable<IGuildUser> userData)
    {
        Debug.WriteLine(userData);

        foreach (var guild in userData)
        {
            GuildUser = guild;
        }
    }

    internal static async Task Send(IGuildUser userData, string text)
    {
        var userId = userData.Id;

        var userInfo = Client.GetUserAsync(userId).Result;
        var dmChannel = await userInfo.CreateDMChannelAsync();
        var messagesAsync = dmChannel.GetMessagesAsync();
        await dmChannel.SendMessageAsync(text);
    }
}