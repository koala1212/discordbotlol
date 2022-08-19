using Discord;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Discord_bot_app.GuildManager;

public class GetMessages : InitializeDiscord
{
    internal static IGuildUser GuildUser;

    public static IDMChannel dmChannel;

    public static List<Message> MessagesList { get; set; }

    internal static async Task GetUser(IEnumerable<IGuildUser> userData)
    {
        foreach (var guild in userData)
        {
            GuildUser = guild;
        }

    }

    public static async Task Send(IGuildUser userData, string text = null)
    {
        var userId = userData.Id;
        var userInfo = Client.GetUserAsync(userId).Result;
        dmChannel = await userInfo.CreateDMChannelAsync();

        if (text != null)
        {
            await dmChannel.SendMessageAsync(text);
        }
    }

    internal static async Task<List<Message>> GetMessage()
    {
        try
        {
            var messageQueue = new List<Message>();
            var messagesAsync = dmChannel.GetMessagesAsync(mode: CacheMode.AllowDownload);

            await foreach (var x in messagesAsync)
            {
                foreach (var message in x)
                {
                    var newMessage = new Message
                    {
                        Author = message.Author,
                        Content = $"{message.Author.Username}: {message.Content} {message.Timestamp.DateTime.ToShortDateString()} {message.Timestamp.DateTime.ToShortTimeString()} ",
                        Timestamp = message.Timestamp.DateTime.ToLongDateString() + message.Timestamp.DateTime.ToLongTimeString(),
                    };

                    messageQueue.Add(newMessage);
                }
            }

            MessagesList = messageQueue;

            return messageQueue;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }

    }

    //public async List<Message> dowork()
    //{
    //    var x = Task.Run(GetMessage).ConfigureAwait(false);
    //}
}