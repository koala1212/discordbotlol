using Discord_bot_app.Startup;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Discord_bot_app.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {
            this.InitializeComponent();
        }

        private async void GetServerinfo(object sender, TappedRoutedEventArgs e) => await new InitializeDiscord().MainAsync("Initialize Guild").ConfigureAwait(false);


    }
}
