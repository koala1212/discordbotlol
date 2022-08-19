using Discord;
using Discord_bot_app.GuildManager;
using Discord_bot_app.MessageLogger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Discord_bot_app
{
    public class NewUsers
    {
        public string Name { get; set; }

        public IGuildUser Description { get; set; }

        public string UniqueId { get; set; }
    }

    public partial class MainPage : Page
    {
        ObservableCollection<NewUsers> UserCollection = new ObservableCollection<NewUsers>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        public List<Message> MessagesList { get; set; }

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

            public static async
                Task<(List<IGuildUser> Servname, List<NewUsers> usersList)> GetGuildMembers()
            {
                var guildUserList = new List<IGuildUser>();
                var guilds = Client.Guilds;
                var usersList = new List<NewUsers>();

                foreach (var guild in guilds)
                {
                    var x = guild.GetUsersAsync();

                    foreach (var user in x.ToEnumerable())
                    {
                        foreach (var member in user)
                        {
                            var users = new NewUsers
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

        private async void RunLoggerProg(object sender, TappedRoutedEventArgs e)
        {
            await new MessageLoggerMain().MainAsync("MessageLogger").ConfigureAwait(false);
        }

        private async void GetServerinfo(object sender, TappedRoutedEventArgs e)
        {
            await new InitializeDiscord().MainAsync("Initialize Guild").ConfigureAwait(false);
        }

        public void filldata(List<NewUsers> testList)
        {
            foreach (var user in testList)
            {
                UserCollection.Add(user);
            }
        }

        private async void NameSelect(object sender, ItemClickEventArgs e)
        {
            var userClass = (await Guildinfo.GetGuildMembers().ConfigureAwait(false)).usersList;
            MessageListView.Items.Clear();

            var selectedName = userClass
                .Where(UniqueId2 => UniqueId2.Name == ((Discord_bot_app.NewUsers)e.ClickedItem).Name)
                .Select(UniqueId2 => UniqueId2.Description);

            await GetMessages.GetUser(selectedName).ConfigureAwait(false);

            try // Creates dm channel kinda scuffed but works 
            {
                var guildUser = GetMessages.GuildUser;
                await GetMessages.Send(guildUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageListView.Items.Clear();
            }

            if (GetMessages.dmChannel != null)
            {
                async void Start()
                {
                    while (true)
                    {
                        MessagesList = await Task.Run(GetMessages.GetMessage).ConfigureAwait(false);
                        Thread.Sleep(1000);
                        var mestodisplaylist = new List<ListViewItem>();
                        var messlist = MessagesList;

                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () =>
                            {
                                var MessageBlock = new TextBlock();

                                foreach (var message in messlist)
                                {
                                    var userItem = new ListViewItem();

                                    MessageBlock.Text = message.Author.Username;
                                    userItem.Content = message.Content;
                                    userItem.Name = message.Timestamp;
                                    mestodisplaylist.Add(userItem);
                                }

                                foreach (var messageitem in from messageitem in mestodisplaylist
                                                            let Crossref = MessageListView.Items
                                                                .OfType<ListViewItem>()
                                                                .Select(id => id.Name)
                                                                .ToList()
                                                            where !Crossref.Contains(messageitem.Name)
                                                            select messageitem)
                                {
                                    MessageListView.Items.Add(messageitem);
                                }
                            });
                    }
                }

                var th = new Thread(Start) { IsBackground = true };

                th.Start();
            }
        }

        private void Refreshserver(object sender, TappedRoutedEventArgs e)
        {
            var itemList = new List<ListViewItem>();
            ServerList.Items.Clear();

            foreach (var x in Guildinfo.GetGuild().Result)
            {
                var item = new ListViewItem();
                var myText = new TextBlock { Text = x };
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

            filldata(Guildinfo.GetGuildMembers().Result.usersList);
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
    }
}