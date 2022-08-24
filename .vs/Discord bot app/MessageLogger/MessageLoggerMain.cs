using CsvHelper;
using Discord;
using Discord_bot_app.Startup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Discord_bot_app.MessageLogger;

public class MessageLoggerMain : InitializeDiscord
{
    private class Message
    {
        public string Author { get; set; }

        public string UserId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string Attachment { get; set; }

        public string Content { get; set; }
    }

    internal static async Task _StartMessageLog()
    {
        var storageFolder =
            ApplicationData.Current.LocalFolder;

        var logfile =
            await storageFolder.CreateFileAsync(
                "DiscordLoungeLogs.csv",
                CreationCollisionOption.ReplaceExisting);

        var channel = Client.GetChannel(380719004962258944) as IMessageChannel;
        ulong fromMessageId = 0;

        var frommessage = channel.GetMessagesAsync(1).FlattenAsync().Result;

        var records = new List<Message>();

        foreach (var message in frommessage)
        {
            fromMessageId = message.Id;

            break;
        }

        var i = 0;

        while (true)
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
                            UserId = author.Discriminator,
                            Author = author.Username,
                            Timestamp = timestamp,
                            Content = message.Content,
                            Attachment = attachmentlist
                        });

                    Debug.WriteLine(
                        $"User:{records[i].Author} Timestamp:{records[i].Timestamp} Message: {records[i].Content} ");

                    i++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace, Console.BackgroundColor == ConsoleColor.DarkRed);

                break;
            }

            var stream = await logfile.OpenStreamForWriteAsync();
            using var writer = new StreamWriter(stream);

            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(records);
        }
    }
}