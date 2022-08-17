using Discord;
using Discord_bot_app.GuildManager;
using Discord_bot_app.MessageLogger;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Discord_bot_app
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public class Guildinfo : InitializeDiscord
        {
            public static Task<List<string>> GetGuild()
            {
                List<string> Servname = new List<string>();
                var guilds = Client.Guilds;

                foreach (var guild in guilds)
                {
                    Servname.Add(guild.Name);
                }

                return Task.FromResult(Servname);
            }

            public static async Task<(List<IGuildUser> Servname, List<GuildUsers> usersList)> GetGuildMembers()
            {
                var guildUserList = new List<IGuildUser>();
                var guilds = Client.Guilds;
                var usersList = new List<GuildUsers>();

                foreach (var guild in guilds)
                {
                    var x = guild.GetUsersAsync();

                    foreach (var user in x.ToEnumerable())
                    {
                        foreach (var member in user)
                        {
                            var users = new GuildUsers()
                            {
                                Name = member.DisplayName,
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

        private async void RunLoggerProg(object sender, TappedRoutedEventArgs e)
        {
            await new MessageLoggerMain().MainAsync("MessageLogger");
        }

        private async void GetServerinfo(object sender, TappedRoutedEventArgs e)
        {
            await new InitializeDiscord().MainAsync("Initialize Guild");
        }

        private async void NameSelect(object sender, ItemClickEventArgs e)
        {
            var userClass = Guildinfo.GetGuildMembers().Result.usersList;

            foreach (var users in userClass)
            {
                Debug.WriteLine(users.Name);
            }

            var selectedName = userClass.Where(UniqueId2 => UniqueId2.Name == (string)e.ClickedItem)
                .Select(UniqueId2 => UniqueId2.Description);

            await GetMessages.GetUser(selectedName);
        }

        private void Refreshserver(object sender, TappedRoutedEventArgs e)
        {
            var itemList = new List<ListViewItem>();
            ServerList.Items.Clear();

            foreach (var x in Guildinfo.GetGuild().Result)
            {
                ListViewItem item = new ListViewItem();
                TextBlock myText = new TextBlock { Text = x };
                item.Content = x;
                itemList.Add(item);
            }

            foreach (var name in itemList)
            {
                ServerList.Items.Add(name);
            }
        }

        private void Refresh_User_list(object sender, TappedRoutedEventArgs e)
        {
            var userList = new List<ListViewItem>();
            var userBlock = new TextBlock();

            foreach (var x in Guildinfo.GetGuildMembers().Result.Servname)
            {
                var userItem = new ListViewItem();
                userBlock.Text = x.Discriminator;
                userItem.Content = x.DisplayName;
                userList.Add(userItem);
                userItem.Name = x.Discriminator;
            }

            foreach (var username in userList)
            {
                MemberList.Items.Add(username);
            }
        }

        public class GuildUsers
        {
            public string Name { get; set; }

            public IGuildUser Description { get; set; }

            public string UniqueId { get; set; }
        }

        private async void SendMessage(object sender, TappedRoutedEventArgs e)
        {
            var guildUser = GetMessages.GuildUser;
            var textToSend = MessageText.Text;
            await GetMessages.Send(guildUser, textToSend);
        }

        private void MessageToSend(object sender, TextChangedEventArgs e)
        {
            //  TODO: get messages from discord and upload to list 
        }
    }
}