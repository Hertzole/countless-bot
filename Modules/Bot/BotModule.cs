using CountlessBot.Classes;
using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountlessBot.Modules.Bot
{
    [Name("Bot"), Summary("Commands for bot owner(s)."), Group("bot"), BotOwnerRequired, ModuleColor(Colors.BOT_R, Colors.BOT_G, Colors.BOT_B)]
    public class BotModule : DiscordModule
    {
        [Command("avatar"), Summary("Sets a new avatar for the bot."), Alias("setavatar", "setpic", "pic"), BotOwnerRequired]
        public async Task SetAvatarAsync([Summary("The avatar URL."), Remainder] string avatarURL)
        {
            try
            {
                // Make sure there's an actual url.
                if (string.IsNullOrWhiteSpace(avatarURL))
                    return;

                // Create a new http client.
                using (var http = new HttpClient())
                {
                    // Create a new stream to get the avatar from the provided url.
                    using (var sr = await http.GetStreamAsync(avatarURL))
                    {
                        // Create a new stream for the image.
                        var imgStream = new MemoryStream();
                        // Copy the bits and boops into the image stream.
                        await sr.CopyToAsync(imgStream);
                        // ¯\_(ツ)_/¯
                        imgStream.Position = 0;
                        // Create a new image from the image stream.
                        Image image = new Image(imgStream);

                        // Update the picture.
                        await CountlessBot.Instance.Client.CurrentUser.ModifyAsync(u => u.Avatar = image).ConfigureAwait(false);
                    }
                }
                // Say that the picture has been updated.
                await Context.Channel.SendSuccessMessage("Avatar has been updated!");
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("reload"), Summary("Reloads the bot config and credentials."), BotOwnerRequired]
        public async Task ReloadAsync()
        {
            try
            {
                // Reload the bot config.
                CountlessBot.Instance.ConfigService.ReloadConfiguration();
                // Reload the bot credentials.
                CountlessBot.Instance.ConfigService.ReloadCredentials();
                // Say that the action was successful.
                await Context.Channel.SendSuccessMessage("Reload complete.");
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("rename"), Summary("Renames the bot."), Alias("name"), BotOwnerRequired]
        public async Task RenameAsync([Remainder] string name)
        {
            try
            {
                // Make sure there's an actual name.
                if (string.IsNullOrWhiteSpace(name))
                    return;

                // Set the new name.
                await (CountlessBot.Instance.Client.CurrentUser.ModifyAsync(x => x.Username = name)).ConfigureAwait(false);
                // Send a success message.
                await Context.Channel.SendSuccessMessage("Name has been changed!");
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("update"), Summary("Updates the bot."), BotOwnerRequired]
        public async Task Update()
        {
            try
            {
                // Check if there is an update available. If not, say so.
                bool willUpdate = CountlessBot.Update();
                // No update was available. Say so.
                if (!willUpdate)
                    await Context.Channel.SendErrorMessage("No update available!");
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }
    }
}
