using Discord;
using System.Collections;

namespace Discord_bot_app.GuildManager.Users;

public partial class MainPage
{
    public class GuildUsers : IEnumerable
    {
        public string Name { get; set; }

        internal IGuildUser Description { get; set; }

        internal string UniqueId { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}