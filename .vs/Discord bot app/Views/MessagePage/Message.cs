

using Discord;
using System.Collections.Generic;

namespace Discord_bot_app.Views.MessagePage;

public partial class MainPage
{
    public class Messages
    {
        public string Content { get; set; }

        public string Author { get; set; }

        public string Timestamp { get; set; }

        public string UserProfile { get; set; }
        
        public IReadOnlyCollection<IAttachment> Attachment { get; set; }
    }
}