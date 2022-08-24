using Discord;
using Discord_bot_app.GuildManager.Messages.MessageClasses;

using Discord_bot_app.MessageLogger;
using Discord_bot_app.Startup;
using Discord_bot_app.Views;
using Discord_bot_app.Views.SettingsPage;
using Discord_bot_app.Views.UserPage;
using Discord_bot_app.Views.UserPage.Userdata.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Discord_bot_app
{
    public partial class MainPage : Page
    {
        //public readonly ObservableCollection<GuildManager.Users.MainPage.GuildUsers> UserCollection = new ObservableCollection<GuildManager.Users.MainPage.GuildUsers>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private List<Message> MessagesList { get; set; }

        public class Guildinfo : InitializeDiscord
        {
            public static Task<List<ServerClass>> GetGuild()
            {
                var servname = new List<ServerClass>();

                var guilds = Client.Guilds;

                foreach (var guild in guilds)
                {
                    var guildinfo = new ServerClass
                    {
                        ServerName = guild.Name,
                        Description = guild.Description,
                    };

                    servname.Add(guildinfo);
                }

                return Task.FromResult(servname);
            }

            public static async
                Task<(List<IGuildUser> Servname, List<GuildManager.Users.MainPage.GuildUsers> usersList)> GetGuildMembers()
            {
                var guildUserList = new List<IGuildUser>();
                var guilds = Client.Guilds;
                var usersList = new List<GuildManager.Users.MainPage.GuildUsers>();

                foreach (var guild in guilds)
                {
                    var x = guild.GetUsersAsync();

                    foreach (var user in x.ToEnumerable())
                    {
                        foreach (var member in user)
                        {
                            var users = new GuildManager.Users.MainPage.GuildUsers
                            {
                                Name = $"{member.Username}#{member.DiscriminatorValue}",
                                UniqueId = member.Discriminator,
                                Description = member
                            };

                            usersList.Add(users);
                            guildUserList.Add(users.Description);
                        }
                    }
                }

                return (guildUserList, usersList);
            }
        }

        private async void RunLoggerProg(object sender, TappedRoutedEventArgs e) => await new MessageLoggerMain().MainAsync("MessageLogger").ConfigureAwait(false);

        public void Refresh_User_list(object sender, TappedRoutedEventArgs e)
        {

            //Filldata(Guildinfo.GetGuildMembers().Result.usersList);
        }



        private void menuItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainPageFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                if (args.InvokedItem is TextBlock ItemContent)
                {
                    switch (ItemContent.Tag)
                    {
                        case "Nav_Home":
                            MainPageFrame.Navigate(typeof(HomeView));

                            break;

                        case "Nav_Users":
                            MainPageFrame.Navigate(typeof(UsersView));

                            break;

                        case "Nav_Messages":
                            MainPageFrame.Navigate(typeof(MessageView));

                            break;
                    }
                }
            }
        }

        private void NavViewLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            foreach (NavigationViewItemBase item in NavMenuMain.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Home_Page")
                {
                    NavMenuMain.SelectedItem = item;

                    break;
                }
            }

            MainPageFrame.Navigate(typeof(HomeView));
        }

        private void NavViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        }
    }
}