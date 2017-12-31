using CountlessBot.Classes;
using CountlessBot.Models;
using Discord.Commands;
using Discord.Net;
using System;
using System.Threading.Tasks;

namespace CountlessBot.Modules
{
    /// <summary>
    /// Base class used for all modules. Includes several handy shortcuts.
    /// </summary>
    public abstract class DiscordModule : ModuleBase
    {
        protected GuildConfig CurrentGuildConfig
        {
            get
            {
                if (Context.Guild != null)
                    return CountlessBot.Instance.ConfigService.GetGuildConfig(Context.Guild.Id);
                else
                {
                    GuildConfig config = new GuildConfig() { CommandPrefix = CountlessBot.Instance.Configuration.DefaultCommandPrefix };
                    return config;
                }
            }
        }


        protected static System.Random Random { get; } = new System.Random();

        /// <summary>
        /// Used to initialize everything the module needs.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Automatically sends an error report to all the bot owners.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async Task ReportError(Exception exception)
        {
            // If the error is a Http exception, check if it's simply not being able to send private messages.
            if (exception is HttpException h)
            {
                // Check for the forbidden error code.
                if (h.HttpCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Tell the user that they can't get private messages.
                    await Context.Channel.SendErrorMessage("I can't send a message to you! You need to enable 'Allow direct messages from server members' in your privacy settings!", 30, Context.User);
                    // Stop here.
                    return;
                }
            }

            // Log the error to the console.
            Logger.LogError(exception);
            // Send the error message saying something went wrong.
            await Context.Channel.SendErrorMessage("Ops! Something went wrong here! I will forward this to the developer immediately!").ConfigureAwait(false);
            // Create the owner message.
            string ownerMessage = $"**An error has occured!**\n\nCommand executed: {Context.Message}\n\nException:```\n{exception.ToString()}```";
            // Send the message to owners.
            await CountlessBot.SendMessageToOwner(ownerMessage).ConfigureAwait(false);
        }
    }
}
