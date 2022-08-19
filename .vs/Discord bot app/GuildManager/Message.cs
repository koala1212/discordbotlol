using Discord;

namespace Discord_bot_app.GuildManager;

public class Message
{
    internal string Content { get; set; }

    internal IUser Author { get; set; }

    internal string Timestamp { get; set; }
}