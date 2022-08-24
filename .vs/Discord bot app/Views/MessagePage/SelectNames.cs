using Discord.WebSocket;
using Discord_bot_app.GuildManager.Messages;
using Discord_bot_app.GuildManager.Messages.MessageClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Discord_bot_app.Views;

public partial class MessageView : Page
{
    private List<Message> MessagesList { get; set; }

    private async Task MessageRecieved(SocketMessage message) => await DisplayMessage();

    public async void NameSelect(object sender, ItemClickEventArgs e)
    {
        var userClass = (await MainPage.Guildinfo.GetGuildMembers().ConfigureAwait(false)).usersList;
        ListOfMessages.Items.Clear();

        var selectedName = userClass
            .Where(UniqueId2 => UniqueId2.Name == ((GuildManager.Users.MainPage.GuildUsers)e.ClickedItem).Name)
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
            ListOfMessages.Items.Clear();
        }

        await DisplayMessage();
    }

    

public async Task DisplayMessage()    
    {
        if (GetMessages.dmChannel != null)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>

                {
                    MessagesList = await GetMessages.GetMessage();

                    var mestodisplaylist = new List<ListViewItem>();
                    var messlist = MessagesList;
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
                                                let Crossref = ListOfMessages.Items
                                                    .OfType<ListViewItem>()
                                                    .Select(id => id.Name)
                                                    .ToList()
                                                where !Crossref.Contains(messageitem.Name)
                                                select messageitem)
                    {
                        ListOfMessages.Items.Add(messageitem);
                    }
                });
        }
    }
}

