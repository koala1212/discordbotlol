using Discord_bot_app.GuildManager.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Discord_bot_app.Views
{ //TODO: CONCEPT GET MESSAGES ONCE THEN AWAIT RESPONSES
    public sealed partial class MessageView : Page
    {
        private ObservableCollection<GuildManager.Users.MainPage.GuildUsers> _UserCollection =
            new ObservableCollection<GuildManager.Users.MainPage.GuildUsers>();

        public ObservableCollection<GuildManager.Users.MainPage.GuildUsers> UserCollection =
            new ObservableCollection<GuildManager.Users.MainPage.GuildUsers>();

        public Thread th { get; set; }

        public MessageView()
        {
            this.InitializeComponent();
            th = new Thread(Start) { IsBackground = true };

            _UserCollection.CollectionChanged +=
                new System.Collections.Specialized.NotifyCollectionChangedEventHandler(UserCollectionChanged);
        }

        private void Refresh_User_list(object sender, TappedRoutedEventArgs e)
        {
            //_UserCollection.Clear();
            //var ServerList = new MainPage();

            //foreach (var server in MainPage.Guildinfo.GetGuildMembers().Result.usersList)
            //{
            //    _UserCollection.Add(server);
            //}
        }

        private void UserCollectionChanged(
            object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            if (_UserCollection.Count <= 0)
            {
                return;
            }

            if (args.OldItems != null)
            {
                foreach (var itemremove in args.OldItems)
                {
                    UserCollection.Remove(itemremove as GuildManager.Users.MainPage.GuildUsers);
                }
            }

            if (args.NewItems == null)
            {
                return;
            }

            List<GuildManager.Users.MainPage.GuildUsers> listUsers = new List<GuildManager.Users.MainPage.GuildUsers>();

            foreach (var item in args.NewItems)
            {
                listUsers.Add(item as GuildManager.Users.MainPage.GuildUsers);
            }

            var items = args.NewItems as GuildManager.Users.MainPage.GuildUsers;
            var enumerator = args.NewItems.GetEnumerator();

            foreach (IEnumerable UserToAdd in from newItemList in listUsers
                                              let Crossref = UserCollection
                                                  .OfType<GuildManager.Users.MainPage.GuildUsers>()
                                                  .Select(id => id.Name)
                                                  .ToList()
                                              where !Crossref.Contains(newItemList.Name)
                                              select newItemList)
            {
                UserCollection.Add(UserToAdd as GuildManager.Users.MainPage.GuildUsers);
            }
        }

        async void Start()
        {
            while (true)
            {
                Thread.Sleep(1000);

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _UserCollection.Clear();

                        foreach (var user in MainPage.Guildinfo.GetGuildMembers().Result.usersList)
                        {
                            _UserCollection.Add(user);
                        }
                    });
            }
        }

        public async void PageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            th.Start();
        }

        private async void SendMessage(object sender, TappedRoutedEventArgs e)
        {
            var guildUser = GetMessages.GuildUser;
            var textToSend = MessageText.Text;
            await GetMessages.Send(guildUser, textToSend);
        }

        private void MemberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }
    }
}