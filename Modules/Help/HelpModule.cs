using CountlessBot.Classes;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountlessBot.Modules.Help
{
    [Name("Help"), Summary("Commands to help you understand the bot."), ModuleColor(Colors.HELP_R, Colors.HELP_G, Colors.HELP_B)]
    public class HelpModule : DiscordModule
    {
        // List of users who can see hidden items.
        private List<ulong> m_CanSeeHiddenStuff = new List<ulong>() { 112769364129792000, 219871624545959936, 188721628476080129,
                                                                    292445991565524992, 103621610900062208, 170935620061888512,
                                                                    170991730840174592, 230168166913802241, 230164111172960257,
                                                                    293849528224972810, 160760585296936962, 112733889323077632,
                                                                    112769364129792000 };

        [Command("help"), Summary("Sends you a message with all the commands for a certain module."), Alias("h")]
        public async Task CommandsAsync([Summary("The module you want to get a list of commands from.")] string module = "")
        {
            try
            {
                // Make the input module name lower case.
                module = module.Trim().ToLower();
                // Get the module from the commands service.
                ModuleInfo baseModule = CountlessBot.Instance.CommandsService.Modules.Where(m => m.Name.ToLower() == module.ToLower()).FirstOrDefault();
                // If there's no module with that name, stop here.
                if (baseModule == null)
                {
                    await Context.Channel.SendErrorMessage($"No module with that name! Do `{CurrentGuildConfig.CommandPrefix}modules` for list all the modules.");
                    return;
                }

                // Create a temporary variable for the module color.
                Color moduleColor = Color.Default;

                // Get the module color from the attributes.
                baseModule.Attributes.ForEach(a =>
                {
                    if (a.GetType() == typeof(ModuleColorAttribute))
                        moduleColor = ((ModuleColorAttribute)a).Color;
                });

                // Get all the commands from the module.
                var cmds = baseModule.Commands.AsEnumerable();
                // Convert the commands into an array.
                var cmdsArray = cmds as CommandInfo[] ?? cmds.ToArray();

                // Create the help embed.
                EmbedBuilder helpEmbedBuilder = new EmbedBuilder()
                {
                    Title = "Commands for " + module + "\n",
                    Color = moduleColor,
                    Footer = new EmbedFooterBuilder() { Text = "<> = Required parameter | () = Optional parameter" }
                };

                // Sort the commands alphabetically.
                Array.Sort(cmdsArray, (x, y) => String.Compare(x.Name, y.Name));

                // Loop through every command in the command array.
                foreach (CommandInfo cmd in cmdsArray)
                {
                    // Create a temporary stop variable, used for skipping over hidden stuff..
                    bool stop = false;

                    // Go through the attributes of the command.
                    for (int i = 0; i < cmd.Attributes.Count; i++)
                    {
                        // If the command has the Hidden attribute and the user is not in the "canSeeHiddenStuff" list, skip this.
                        if (cmd.Attributes.ElementAt(i).GetType() == typeof(HiddenAttribute))
                            if (!m_CanSeeHiddenStuff.Contains(Context.User.Id))
                                stop = true;
                    }

                    // If stop is true, go to the next command in the list.
                    if (stop)
                        continue;

                    // Create aliases variable.
                    string aliases = "";
                    // Create usage variable.
                    string usage = cmd.Aliases[0];
                    // Go through every alias that the command has.
                    for (int i = 0; i < cmd.Aliases.Count; i++)
                    {
                        // If the aliases are more than 1, add them to the string.
                        // Else just write None.
                        if (cmd.Aliases.Count > 1)
                        {
                            aliases += "`" + cmd.Aliases[i] + "`";

                            if (i < cmd.Aliases.Count - 1)
                                aliases += ", ";
                        }
                        else
                            aliases += "None";
                    }

                    // Go through every parameter.
                    for (int i = 0; i < cmd.Parameters.Count; i++)
                    {
                        // If the parameter is optinal, do (paramater)
                        // Else do <paramater>
                        if (cmd.Parameters[i].IsOptional)
                            usage += " (" + cmd.Parameters[i].Name + ")";
                        else
                            usage += " <" + cmd.Parameters[i].Name + ">";
                    }

                    // Add a single ` character at the end.
                    usage += "`";

                    // Add the fields to the embed builder.
                    helpEmbedBuilder.AddField(x =>
                    {
                        string value = "";
                        value += cmd.Summary + "\n";
                        value += "**Aliases:** ";
                        value += aliases + "\n";
                        value += "**Usage:** `" + CurrentGuildConfig.CommandPrefix + "";
                        value += usage;
                        x.Name = cmd.Name;
                        x.Value = value;
                    });
                }

                // Build the embed.
                Embed helpEmbed = helpEmbedBuilder.Build();

                // Create a private channel to the user who asked for help.
                var userChannel = await Context.User.GetOrCreateDMChannelAsync();
                // Send the message in a private channel.
                await userChannel.SendMessageAsync("", false, helpEmbed);
            }
            catch (Exception e)
            {
                await ReportError(e);
            }
        }

        [Command("info"), Summary("Displays info about the bot, like version, author, etc."), Alias("i")]
        public async Task BotInfoAsync()
        {
            try
            {
                // Create the embed builder.
                EmbedBuilder infoEmbedBuilder = new EmbedBuilder()
                {
                    ThumbnailUrl = CountlessBot.Instance.Client.CurrentUser.GetAvatarUrl(ImageFormat.Png, 256),
                    Title = CountlessBot.Instance.Client.CurrentUser.Username,
                    Color = Colors.MainColor,
                    Description = "Developed by Hertzole for Fish Jesus.",
                    Footer = new EmbedFooterBuilder() { Text = "Version " + CountlessBot.Version }
                };
                // Build the embed.
                Embed infoEmbed = infoEmbedBuilder.Build();
                // Send the embed.
                await ReplyAsync("", false, infoEmbed);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("modules"), Summary("Sends you a list of all the modules."), Alias("module")]
        public async Task ModulesAsync()
        {
            try
            {
                // Get all the modules from the command service.
                var modules = CountlessBot.Instance.CommandsService.Modules;
                // Convert the modules to an array.
                var modulesArray = modules as ModuleInfo[] ?? modules.ToArray();

                // Create the embed builder.
                EmbedBuilder modulesEmbedBuilder = new EmbedBuilder()
                {
                    Title = "List of modules",
                    Color = Colors.MainColor
                };

                // Sort the modules alphabetically.
                Array.Sort(modulesArray, (x, y) => String.Compare(x.Name, y.Name));

                // Loop through all the modules-
                for (int i = 0; i < modulesArray.Length; i++)
                {
                    // If the module name is 'Bot', only show it if the user is an owner. Else just skip.
                    if (modulesArray[i].Name == "Bot")
                        if (!CountlessBot.Instance.Credentials.OwnerIDs.Contains(Context.User.Id))
                            continue;

                    // Add a field with the name and description.
                    modulesEmbedBuilder.AddField(x => { x.Name = modulesArray[i].Name; x.Value = modulesArray[i].Summary; });
                }

                // Create a private channel.
                var userChannel = await Context.User.GetOrCreateDMChannelAsync();
                // Send the embed.
                await userChannel.SendMessageAsync("", false, modulesEmbedBuilder);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }
    }
}
