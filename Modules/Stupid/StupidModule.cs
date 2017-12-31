using CountlessBot.Classes;
using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CountlessBot.Modules.Stupid
{
    [Name("Stupid"), Summary("Stupid stuff."), ModuleColor(Colors.UTILITY_R, Colors.UTILITY_G, Colors.UTILITY_B)]
    public class StupidModule : DiscordModule
    {
        private static string BleachPath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/bleach.jpg"; } }
        private static string NeeyonPath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/neeyon.png"; } }
        private static string OrgasmPath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/orgasm.png"; } }
        private static string PotatoPath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/potato.png"; } }
        private static string RopePath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/rope.jpg"; } }
        private static string ShootPath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/shoot.png"; } }
        private static string SuccySuccyPath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/succysuccy.png"; } }
        private static string SuicidePath { get { return BotHelpers.ProcessPath + "/countlessbot/resources/suicide.jpg"; } }

        [Command("bleach"), Summary("Don't drink this."), Hidden]
        public async Task Bleach()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(BleachPath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(BleachPath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("neeyon"), Summary("Dunno"), Hidden]
        public async Task Neeyon()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(NeeyonPath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(NeeyonPath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("orgasm"), Summary("Uhhh...")]
        public async Task Orgasm()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(OrgasmPath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(OrgasmPath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("potato"), Summary("Sends a picture of a potato.")]
        public async Task PotatoAsync()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(PotatoPath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(PotatoPath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("rope"), Summary("Don't do this."), Hidden]
        public async Task Rope()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(RopePath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(RopePath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("shoot"), Summary("Very bad taste"), Hidden]
        public async Task Shoot()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(ShootPath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(ShootPath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("succysuccy"), Summary("It's soo big"), Hidden]
        public async Task SuccySuccy()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(SuccySuccyPath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(SuccySuccyPath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("suicide"), Summary("Don't do this."), Hidden]
        public async Task Suicide()
        {
            try
            {
                // Make sure the file exists.
                if (!await CheckIfFileExists(SuicidePath))
                    return;

                // Send the file.
                await Context.Channel.SendFileAsync(SuicidePath);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        /// <summary>
        /// Does a simple check if the file at the path exists, and if not, tells the bot owner(s).
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<bool> CheckIfFileExists(string path)
        {
            if (!File.Exists(path))
            {
                await CountlessBot.SendMessageToOwner("There's no file at " + path);
                return false;
            }

            return true;
        }
    }
}
