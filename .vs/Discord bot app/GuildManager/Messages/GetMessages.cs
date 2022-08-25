using Discord;
using Discord_bot_app.Startup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord_bot_app.GuildManager.Messages;

public class GetMessages : InitializeDiscord
{
    internal static IGuildUser GuildUser;

    public static IDMChannel dmChannel;

    public static List<Views.MessagePage.MainPage.Messages> MessagesList { get; set; }

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

    internal static async Task<List<Views.MessagePage.MainPage.Messages>> GetMessage()
    {
        try
        {
            var messageQueue = new List<Views.MessagePage.MainPage.Messages>();
            var messagesAsync = dmChannel.GetMessagesAsync(mode: CacheMode.AllowDownload);

            string getTimeStamp(IMessage message)
            {
                if (message.Timestamp.DateTime.ToShortDateString() == DateTime.Today.ToShortDateString())

                {
                    return message.Timestamp.DateTime.ToShortTimeString();
                }
                else
                {
                    return message.Timestamp.DateTime.ToLongDateString();
                }
            }

            await foreach (var x in messagesAsync)
            {
                foreach (var message in x)
                {
                    var newMessage = new Views.MessagePage.MainPage.Messages
                    {
                        Author = message.Author.Username + " ",
                        Content = message.Content + " ",
                        UserProfile = message.Author.GetAvatarUrl(),
                        Timestamp = getTimeStamp(message),
                        Attachment = message.Attachments
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