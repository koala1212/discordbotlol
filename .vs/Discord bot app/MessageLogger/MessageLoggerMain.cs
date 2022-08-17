using CsvHelper;
using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Discord_bot_app.MessageLogger;

public class MessageLoggerMain : InitializeDiscord
{
    private class Message
    {
        public string _author { get; set; }

        public string _userID { get; set; }

        public DateTimeOffset _timestamp { get; set; }

        public string _attachment { get; set; }

        public string _content { get; set; }
    }

    public static async Task _StartMessageLog()
    {
        var storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;

        var logfile =
            await storageFolder.CreateFileAsync(
                "DiscordLoungeLogs.csv",
                Windows.Storage.CreationCollisionOption.ReplaceExisting);

        var channel = Client.GetChannel(380719004962258944) as IMessageChannel;
        ulong fromMessageId = 0;

        var frommessage = channel.GetMessagesAsync(1).FlattenAsync().Result;

        var records = new List<Message> { };

        foreach (var message in frommessage)
        {
            fromMessageId = message.Id;

            break;
        }

        int i = 0;
        bool go = true;

        while (go)
        {
            try
            {
                var messages = await channel.GetMessagesAsync(fromMessageId, dir: Direction.Before)
                    .FlattenAsync();

                await Task.Delay(500);

                foreach (var message in messages)
                {
                    fromMessageId = message.Id;
                    var author = message.Author;
                    var timestamp = message.Timestamp;
                    var attachments = message.Attachments;
                    var attachmentlist = "";
                    List<string> atList = new List<string>();

                    foreach (var attachment in attachments)
                    {
                        atList.Add(attachment.ProxyUrl);
                    }

                    foreach (var attachment in atList)
                    {
                        attachmentlist = attachmentlist + "\n" + attachment + ",";
                    }

                    records.Add(
                        new Message
                        {
                            _userID = author.Discriminator,
                            _author = author.Username,
                            _timestamp = timestamp,
                            _content = message.Content,
                            _attachment = attachmentlist
                        });

                    Debug.WriteLine(
                        $"User:{records[i]._author} Timestamp:{records[i]._timestamp} Message: {records[i]._content} ");

                    i++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace, Console.BackgroundColor == ConsoleColor.DarkRed);
                go = false;

                break;
            }

            var stream = await logfile.OpenStreamForWriteAsync();
            using var writer = new StreamWriter(stream);

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(records);
            }
        }
    }
}