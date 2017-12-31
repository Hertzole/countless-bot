using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountlessBot.Classes
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            for (int i = 0; i < source.Count(); i++)
            {
                action(source.ElementAt(i));
            }
        }

        public static Stream ToStream(this string str)
        {
            var sw = new StreamWriter(new MemoryStream());
            sw.Write(str);
            sw.Flush();
            sw.BaseStream.Position = 0;
            return sw.BaseStream;
        }

        public static void AddFakeHeaders(this HttpClient http)
        {
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1");
            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        }

        public static async Task<IDeletable> SendErrorMessage(this IMessageChannel channel, string description = "", int deleteTime = 10, IUser userMention = null)
        {
            return await SendEmbedMessage(channel, "Error!", description, Colors.Error, deleteTime).ConfigureAwait(false);
        }

        public static async Task<IDeletable> SendErrorMessage(this IMessageChannel channel, Color color, string description = "", int deleteTime = 10, IUser userMention = null)
        {
            return await SendEmbedMessage(channel, "Error!", description, color, deleteTime).ConfigureAwait(false);
        }

        public static async Task<IDeletable> SendSuccessMessage(this IMessageChannel channel, string description = "", int deleteTime = 10, IUser userMention = null)
        {
            return await SendEmbedMessage(channel, "Success!", description, Colors.Success, deleteTime).ConfigureAwait(false);
        }

        public static async Task<IDeletable> SendSuccessMessage(this IMessageChannel channel, Color color, string description = "", int deleteTime = 10, IUser userMention = null)
        {
            return await SendEmbedMessage(channel, "Success!", description, color, deleteTime).ConfigureAwait(false);
        }

        private static async Task<IDeletable> SendEmbedMessage(IMessageChannel channel, string title, string description, Color color, int deleteTime = 10, IUser userMention = null)
        {
            // Build the message.
            EmbedBuilder messageEmbedBuilder = new EmbedBuilder()
            {
                Title = title,
                Description = description,
                Color = color
            };

            // Convert the message to Embed.
            Embed messageEmbed = messageEmbedBuilder.Build();

            // Send the message.
            var msg = await channel.SendMessageAsync(userMention == null ? "" : userMention.Mention, false, messageEmbed).ConfigureAwait(false);
            // Delete the message after a set amount of seconds, if delete time is above 0, that is.
            if (deleteTime > 0)
                msg.DeleteAfter(deleteTime);

            // Lastly, return the message.
            return msg;
        }

        /// <summary>
        /// Deletes a message after a set amount of seconds.
        /// </summary>
        /// <param name="seconds">The amount of seconds to pass before the message gets deleted.</param>
        /// <returns></returns>
        public static IMessage DeleteAfter(this IUserMessage msg, int seconds)
        {
            Task.Run(async () =>
            {
                await Task.Delay(seconds * 1000);
                try { await msg.DeleteAsync().ConfigureAwait(false); }
                catch { }
            });
            return msg;
        }
    }
}
