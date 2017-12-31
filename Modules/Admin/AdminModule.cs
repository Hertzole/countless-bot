using CountlessBot.Classes;
using CountlessBot.Models;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CountlessBot.Modules.Admin
{
    [Name("Admin"), Summary("Commands for the server admins and moderators"), Group("admin"), NoPrivateChat, ModuleColor(Colors.ADMIN_R, Colors.ADMIN_G, Colors.ADMIN_B)]
    public class AdminModule : DiscordModule
    {
        [Command("purge"), Summary("Removes the bot's messages in the last 100 messages or a set amount. A user can also be specified to remove their messages."), Alias("clear", "clean"), RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int amount = 100, IUser user = null)
        {
            try
            {
                // If the amount to remove is less than 1, stop here.
                if (amount < 1)
                    return;

                // If the user is not null and the user is the user who sent the message, add one extra to the amount.
                if (user != null)
                    if (user.Id == Context.User.Id)
                        amount += 1;

                // If the user is null, delete the message that started the command.
                if (user == null)
                    await Context.Message.DeleteAsync().ConfigureAwait(false);
                // Set the limit. The limit will always be 100.
                int limit = (amount < 100) ? amount : 100;
                // Make a temporary variable to hold all the messages.
                IEnumerable<IMessage> allMessages = null;
                // If the user is null, get all the messages from the bot in the last 100 messages. Else get the messages from the user specified.
                if (user == null)
                    allMessages = (await Context.Channel.GetMessagesAsync(limit, CacheMode.AllowDownload).Flatten().ConfigureAwait(false)).Where(m => m.Author.Id == CountlessBot.Instance.Client.CurrentUser.Id);
                else
                    allMessages = (await Context.Channel.GetMessagesAsync(limit, CacheMode.AllowDownload).Flatten().ConfigureAwait(false)).Where(m => m.Author.Id == user.Id);

                // Make a temporary variable to hold the messages that will be removed.
                IEnumerable<IMessage> messages = Enumerable.Empty<IMessage>();

                // Loop through all the messages.
                for (int i = 0; i < allMessages.Count(); i++)
                {
                    // Make sure the message is less than 14 days old (Discord limitation) and if it is, add it to the messages list.
                    if ((allMessages.ElementAt(i).Timestamp.UtcDateTime - DateTime.UtcNow).TotalDays < 15)
                        messages.Append(allMessages.ElementAt(i));
                }

                // Lastly delete all the messages.
                await Context.Channel.DeleteMessagesAsync(messages).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("set default role"), Summary("Sets the role that every new user will automatically be set to."), RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetDefaultRoleAsync(IRole role)
        {
            try
            {
                // Set the default role to the the provided role.
                CurrentGuildConfig.DefaultRole = role.Id;
                // Update the config.
                CountlessBot.Instance.ConfigService.UpdateGuildConfig(Context.Guild.Id, CurrentGuildConfig);
                // Say that the role was updated.
                await Context.Channel.SendSuccessMessage("Default role updated.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("set poll channel"), Summary("Sets the channel where normal users can create polls."), RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPollChannel(IGuildChannel channel)
        {
            try
            {
                // Set the polls channel ID.
                CurrentGuildConfig.PollsChannelID = channel.Id;
                // Update the config.
                CountlessBot.Instance.ConfigService.UpdateGuildConfig(channel.GuildId, CurrentGuildConfig);
                // Say that the channel was updated.
                await Context.Channel.SendSuccessMessage("Poll Channel updated.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("set prefix"), Summary("Sets the command prefix for this server."), RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPrefixAsync([Remainder] string prefix)
        {
            try
            {
                // Trim the prefix.
                prefix = prefix?.Trim();

                // Make sure a new prefix was entered.
                if (string.IsNullOrWhiteSpace(prefix))
                {
                    await Context.Channel.SendErrorMessage("You need to enter a new prefix.");
                    return;
                }

                // Make sure so the prefix isn't longer than 10 characters.
                if (prefix.Length > 10)
                {
                    await Context.Channel.SendErrorMessage("The prefix can't be longer than 10 characters.").ConfigureAwait(false);
                    return;
                }

                // Set the prefix.
                CurrentGuildConfig.CommandPrefix = prefix;
                // Update the config.
                CountlessBot.Instance.ConfigService.UpdateGuildConfig(Context.Guild.Id, CurrentGuildConfig);
                // Say that the prefix was updated.
                await Context.Channel.SendSuccessMessage($"Prefix updated to `{prefix}`!");
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("set warn duration"), Summary("Sets the duration of each warning levels."), RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetWarnDuration(int level, [Remainder] string duration)
        {
            try
            {
                // Make sure the level is within range.
                if (level < 1 || level > 3)
                {
                    await Context.Channel.SendErrorMessage("The supported warn levels are just 1-3.");
                    return;
                }

                // Make the duration lower case.
                duration = duration.ToLower();

                // Create a new regex.
                Regex regex = new Regex("([0-9]+)(m|h|d|w)");
                // Find matches.
                MatchCollection matches = regex.Matches(duration);
                // Create a new empty duration.
                TimeDuration timeDuration = new TimeDuration();
                // Run if there was any matches.
                // Else the time format was incorrect.
                if (matches.Count > 0)
                {
                    // Loop through the matches.
                    for (int i = 0; i < matches.Count; i++)
                    {
                        // If the groups was above 0 in the match.
                        if (matches[i].Groups.Count > 0)
                        {
                            // Get the time.
                            int time = 0;
                            // Set the time type.
                            TimeType timeType = TimeType.Seconds;

                            // Go through the groups in the match.
                            for (int j = 0; j < matches[i].Groups.Count; j++)
                            {
                                // Take the group in second place.
                                if (j == 1)
                                {
                                    // Parse the time.
                                    time = int.Parse(matches[i].Groups[j].Value);
                                    // Time can't be negative.
                                    if (time < 0)
                                    {
                                        await Context.Channel.SendErrorMessage("Time can't be less than 0.");
                                        return;
                                    }
                                }
                                else if (j == 2)
                                {
                                    // Get the matching time type from the group.
                                    timeType = GetTimeType(matches[i].Groups[j].Value[0]);
                                }
                            }

                            // Add time in the right slot depending on the time type.
                            switch (timeType)
                            {
                                case TimeType.Seconds:
                                    timeDuration.Seconds += time;
                                    break;
                                case TimeType.Minutes:
                                    timeDuration.Minutes += time;
                                    break;
                                case TimeType.Hours:
                                    timeDuration.Hours += time;
                                    break;
                                case TimeType.Days:
                                    timeDuration.Days += time;
                                    break;
                                case TimeType.Weeks:
                                    timeDuration.Weeks += time;
                                    break;
                            }
                        }
                    }

                    // Create the time string.
                    string timeString = GetTimeString(timeDuration);
                    // Get the current config.
                    GuildConfig config = CurrentGuildConfig;
                    // Set the time duration to the right warning duration field.
                    if (level == 1)
                        config.Warning1Duration = timeDuration;
                    else if (level == 2)
                        config.Warning2Duration = timeDuration;
                    else if (level == 3)
                        config.Warning3Duration = timeDuration;

                    // Update the config.
                    CountlessBot.Instance.ConfigService.UpdateGuildConfig(Context.Guild.Id, config);
                    // Say that the change was successful.
                    await Context.Channel.SendSuccessMessage("The duration for warning level " + level + " will now be " + timeString + ".");
                }
                else
                {
                    // The format was wrong.
                    await Context.Channel.SendErrorMessage("I did not understand your time format.");
                    return;
                }
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        /// <summary>
        /// Gets the right time type based on character.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private TimeType GetTimeType(char value)
        {
            switch (value)
            {
                case 's':
                    return TimeType.Seconds;
                case 'm':
                    return TimeType.Minutes;
                case 'h':
                    return TimeType.Hours;
                case 'd':
                    return TimeType.Days;
                case 'w':
                    return TimeType.Weeks;
                default:
                    return TimeType.Seconds;
            }
        }

        /// <summary>
        /// Create the time string.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private string GetTimeString(TimeDuration duration)
        {
            StringBuilder sb = new StringBuilder();
            if (duration.Weeks > 0)
                sb.Append($"{duration.Weeks} weeks ");
            if (duration.Days > 0)
                sb.Append($"{duration.Days} days ");
            if (duration.Hours > 0)
                sb.Append($"{duration.Hours} hours ");
            if (duration.Minutes > 0)
                sb.Append($"{duration.Minutes} minutes ");
            if (duration.Seconds > 0)
                sb.Append($"{duration.Seconds} seconds");

            return sb.ToString().Trim();
        }

        [Command("set warn role"), Summary("Sets the correct roles for warning levels."), NoPrivateChat, RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetWarnRole(int level, IRole role)
        {
            try
            {
                // Go through each level to see if it matches.
                // If it matches a number, set the role id.
                // If not, just stop.
                switch (level)
                {
                    case 1:
                        CurrentGuildConfig.Warning1Role = role.Id;
                        break;
                    case 2:
                        CurrentGuildConfig.Warning2Role = role.Id;
                        break;
                    case 3:
                        CurrentGuildConfig.Warning3Role = role.Id;
                        break;
                    default:
                        // No matching level number. Stop here and say it was wrong.
                        await Context.Channel.SendErrorMessage("The supported warn levels are just 1-3.");
                        return;
                }

                // Update the config.
                CountlessBot.Instance.ConfigService.UpdateGuildConfig(Context.Guild.Id, CurrentGuildConfig);
                // Send a success message.
                await Context.Channel.SendSuccessMessage($"Role updated for level {level}!");
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("warn"), Summary("Warns a user and gives them a specific role. Has 4 different levels, 4 being banning and 0 being pardon."), RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task WarnAsync(int level, IGuildUser user, [Remainder] string reason = "")
        {
            try
            {
                // Remove any unnecessary spaces from the reason.
                reason = reason.Trim();

                // Check if the user is missing Manage Roles permission and if so, deny them to give warnings above level 2.
                if (!((IGuildUser)Context.User).GuildPermissions.Has(GuildPermission.ManageRoles)) //Is helper
                {
                    if (level > 2)
                    {
                        await ReplyAsync(Context.User.Mention + " You don't have permission to issue warnings above level 2.").ConfigureAwait(false);
                        return;
                    }
                }

                // Make sure you can't warn a bot.
                if (user.IsBot)
                {
                    await ReplyAsync("You can't use this on a bot.");
                    return;
                }

                // Make sure you can't warn/pardon yourself.
                if (user.Id == Context.User.Id)
                {
                    if (level == 0)
                        await ReplyAsync("You can't pardon yourself.");
                    else
                        await ReplyAsync("You can't warn yourself.");
                    return;
                }

                // Make sure you can't warn the owner of the server.
                if (user.Id == Context.Guild.OwnerId)
                {
                    await ReplyAsync("You can't warn the owner of the server.");
                    return;
                }

                // Create the private user channel to the user being warned.
                IDMChannel userChannel = await user.GetOrCreateDMChannelAsync();
                // Create the temporary role variable.
                IRole roleToSet = null;
                // Get the correct role based on the level input.
                if (level == 1)
                    roleToSet = Context.Guild.GetRole(CurrentGuildConfig.Warning1Role);
                else if (level == 2)
                    roleToSet = Context.Guild.GetRole(CurrentGuildConfig.Warning2Role);
                else if (level == 3)
                    roleToSet = Context.Guild.GetRole(CurrentGuildConfig.Warning3Role);
                else if (level == 4)
                {
                    // If the level is 4, ban the user, if the user doing the warning can ban users.

                    // If the user can't ban users, stop here.
                    if (!((IGuildUser)Context.User).GuildPermissions.Has(GuildPermission.BanMembers))
                    {
                        await ReplyAsync("You don't have permission to ban users.").ConfigureAwait(false);
                        return;
                    }

                    // Say that a user is being banned.
                    await ReplyAsync("Banning user " + user.Username).ConfigureAwait(false);

                    // Create the fancy embed.
                    EmbedBuilder bannedEmbedBuilder = new EmbedBuilder()
                    {
                        Color = Color.Red,
                        Title = "The ban hammer has been used!",
                        Description = "You've been banned from '" + Context.Guild.Name + "'!"
                    };

                    // If there's a reason, add it as a field.
                    if (!string.IsNullOrWhiteSpace(reason))
                        bannedEmbedBuilder.AddField("Reason:", reason);
                    // Build the embed.
                    Embed bannedEmbed = bannedEmbedBuilder.Build();

                    // Send the embed.
                    await userChannel.SendMessageAsync("", false, bannedEmbed);
                    // Ban the user.
                    await Context.Guild.AddBanAsync(user);
                    return;
                }
                else if (level < 0 || level > 4)
                {
                    // If warn level is out of range, say so and stop.
                    await ReplyAsync("The only warning levels are 1-3.");
                    return;
                }

                // If the role is null and the level is not 0 (pardon), say that the role is not present.
                if (roleToSet == null && level != 0)
                {
                    await ReplyAsync($"I can't find a role that is associated with that level. Add it using the `{CurrentGuildConfig.CommandPrefix}set warn role` command");
                    return;
                }

                // Create an array of all the roles to remove from the user (old warning roles).
                IRole[] roles = new IRole[] { Context.Guild.GetRole(CurrentGuildConfig.Warning1Role), Context.Guild.GetRole(CurrentGuildConfig.Warning2Role), Context.Guild.GetRole(CurrentGuildConfig.Warning3Role) };
                // Loop through the roles.
                for (int i = 0; i < roles.Length; i++)
                {
                    // If the role is present, remove it from the user.
                    if (roles[i] != null)
                        await user.RemoveRoleAsync(roles[i]).ConfigureAwait(false);
                }

                // If the level is above 0, they are being warned.
                // Else they are being pardoned.
                if (level > 0)
                {
                    // Say that a user is being warned.
                    await ReplyAsync("Warning user " + user.Username);
                    // Add the warning role.
                    await user.AddRoleAsync(roleToSet);
                    // Build the warning embed.
                    EmbedBuilder warningEmbedBuilder = new EmbedBuilder()
                    {
                        Color = Color.Red,
                        Title = "Oh no!",
                        Description = "You've received a warning on '" + Context.Guild.Name + "'!"
                    };
                    // If there's a reason, add it as a field.
                    if (!string.IsNullOrWhiteSpace(reason))
                        warningEmbedBuilder.AddField("Reason:", reason);
                    // Build the embed.
                    Embed warningEmbed = warningEmbedBuilder.Build();

                    // Send the embed to the user.
                    await userChannel.SendMessageAsync("", false, warningEmbed);
                    // Add the user to the warnings list.
                    CountlessBot.Instance.WarningService.AddUser(user, level, reason);
                }
                else
                {
                    // Say that a user is being pardoned.
                    await ReplyAsync("Pardoning user " + user.Username);
                    // Build the pardon embed.
                    EmbedBuilder pardonEmbedBuilder = new EmbedBuilder()
                    {
                        Color = Color.Green,
                        Title = "You've been pardoned!",
                        Description = "You no longer have any warnings on '" + Context.Guild.Name + "'! Good for you!"
                    };
                    // If there's a reason, add it as a field.
                    if (!string.IsNullOrWhiteSpace(reason))
                        pardonEmbedBuilder.AddField("Reason:", reason);
                    // Build the embed.
                    Embed pardonEmbed = pardonEmbedBuilder.Build();
                    // Send the embed to the user.
                    await userChannel.SendMessageAsync("", false, pardonEmbed);
                    // Remove the user from the warnings list.
                    CountlessBot.Instance.WarningService.RemoveUser(user);
                }
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }
    }
}
