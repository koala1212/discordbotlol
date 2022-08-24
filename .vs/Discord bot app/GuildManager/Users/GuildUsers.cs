using Discord;


namespace Discord_bot_app.GuildManager.Users;

public partial class MainPage
{
    public class GuildUsers
    {
        public string Name { get; set; }

        internal IGuildUser Description { get; set; }

        internal string UniqueId { get; set; }

        public string ProfileImage { get; set; }
        
    }
}