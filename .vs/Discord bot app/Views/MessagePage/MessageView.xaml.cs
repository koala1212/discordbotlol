using Discord.WebSocket;
using Discord_bot_app.GuildManager.Messages;
using Discord_bot_app.Startup;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Discord_bot_app.Views
{ //TODO: CONCEPT GET MESSAGES ONCE THEN AWAIT RESPONSES
    public sealed partial class MessageView : Page
    {
        public ObservableCollection<GuildManager.Users.MainPage.GuildUsers> UserCollection = new ObservableCollection<GuildManager.Users.MainPage.GuildUsers>();
        
        public ObservableCollection<MessagePage.MainPage.Messages> MessageCollection = new ObservableCollection<MessagePage.MainPage.Messages>();

        public MessageView()
        {
            InitializeDiscord.Client.UserJoined += UserJoined;
            InitializeDiscord.Client.MessageReceived += MessageRecieved;
            this.InitializeComponent();
        }

        private async Task UserJoined(SocketGuildUser newUser)
        {
            var newUserClass = new GuildManager.Users.MainPage.GuildUsers()
            {
                Name = newUser.Username,
                Description = newUser,
                UniqueId = newUser.Discriminator,
                ProfileImage = newUser.GetDisplayAvatarUrl()
            };
                
            UserCollection.Add(newUserClass);
        }

        public void PageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UserCollection.Clear();

            foreach (var user in MainPage.Guildinfo.GetGuildMembers().Result.usersList)
            {
                UserCollection.Add(user);
            }
        }

        private async void SendMessage(object sender, TappedRoutedEventArgs e)
        {
            var guildUser = GetMessages.GuildUser;
            var textToSend = MessageText.Text;
            await GetMessages.Send(guildUser, textToSend);
            await DisplayMessageAsync();
        }

        private void Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }
    }
}