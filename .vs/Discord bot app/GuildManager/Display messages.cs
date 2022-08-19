//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Windows.UI.Core;
//using Windows.UI.Xaml.Controls;

//namespace Discord_bot_app.GuildManager;

//public class Displaymessages
//{
//    public static List<Message> MessagesList { get; set; }





//    internal static async Task<List<ListViewItem>> DisplayMessage()
//    {

//        var mestodisplaylist = new List<ListViewItem>();
//        var messlist = MessagesList;
//        var messageItemList = new List<ListViewItem>();

//        foreach (var message in messlist)
//        {
//            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
//                CoreDispatcherPriority.Normal,
//                () =>
//                {
//                    var main = new MainPage();
//                    var userItem = new ListViewItem();
//                    var MessageBlock = new TextBlock();
//                    MessageBlock.Text = message.Author.Username;
//                    userItem.Content = message.Content;
//                    messageItemList.Add(userItem);
//                    var guid = Guid.NewGuid().ToString();
//                    userItem.Name = guid;

//                    foreach (var username in messageItemList)
//                    {
//                        mestodisplaylist.Add(username);
//                    }

//                    foreach (var message in mestodisplaylist)
//                    {
//                        main.MessageListView.Items.Add(message);
//                    }

//                });


//        }

//        return mestodisplaylist;

//    }
//}
