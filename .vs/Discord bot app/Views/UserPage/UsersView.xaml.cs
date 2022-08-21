
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Discord_bot_app.Views.UserPage
{

    public partial class UsersView : Page
    {
        public readonly ObservableCollection<Discord_bot_app.Views.UserPage.Userdata.Classes.ServerClass>
            ServerCollection = new ObservableCollection<Discord_bot_app.Views.UserPage.Userdata.Classes.ServerClass>();

        public UsersView()
        {
            this.InitializeComponent();
        }

        private void Refresh_User_list(object sender, TappedRoutedEventArgs e)
        {

        }

        private async void Refreshserver(object sender, TappedRoutedEventArgs e)
        {
            ServerCollection.Clear();
            var ServerList = new MainPage();

            foreach (var server in MainPage.Guildinfo.GetGuild().Result)
            {
                ServerCollection.Add(server);
            }
        }

        private void NameSelect(object sender, ItemClickEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}