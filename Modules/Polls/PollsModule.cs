using CountlessBot.Classes;
using CountlessBot.Models;
using CountlessBot.Services;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountlessBot.Modules.Polls
{
    [Name("Polls"), Summary("Commands related to creating and voting on polls."), NoPrivateChat]
    public class PollsModule : DiscordModule
    {
        // Shortcut to the polls service.
        private static PollsService PollService { get { return CountlessBot.Instance.PollService; } }

        public PollsModule()
        {
            // Subscribe to the minute tick to check the polls every minute.
            CountlessBot.MinuteTick += CheckPolls;
        }

        private async Task CheckPolls(DateTime time)
        {
            // Go through all the loaded polls.
            for (int i = 0; i < PollService.Polls.Count; i++)
            {
                // Get the poll.
                Poll poll = PollService.Polls.ElementAt(i).Value;
                // Check if the time has passed the expire time.
                if (DateTime.UtcNow >= poll.EndsAt)
                {
                    // Create a temporary variable for the channel to check.
                    IMessageChannel channel = null;
                    // Go through all the available guilds.
                    for (int g = 0; g < CountlessBot.Instance.Client.Guilds.Count; g++)
                    {
                        // Check if the guild ID is the same as the one in the poll.
                        // If so, get the channel from that guild.
                        if (CountlessBot.Instance.Client.Guilds.ElementAt(g).Id == poll.GuildID)
                            channel = CountlessBot.Instance.Client.Guilds.ElementAt(g).GetTextChannel(PollService.Polls.ElementAt(i).Key);
                    }

                    // End the poll if the time is bast the expire time.
                    await EndPoll(channel);
                }
            }
        }

        [Command("poll create"), Summary("Creates a poll that exists for a certain duration and it can have multiple options, separated by commas.")]
        public async Task PollCreateAsync(float duration, string pollName, string pollDescription, [Remainder] string options = null)
        {
            try
            {
                // Check if the user who wanted to create the poll has manage channels permission.
                if (!((IGuildUser)Context.User).GetPermissions(Context.Channel as IGuildChannel).ManageChannel)
                {
                    // If there's a polls channel set, continue.
                    // Else say that there is no polls channel.
                    if (CurrentGuildConfig.PollsChannelID != 0)
                    {
                        // Check if the current channel is the same as the polls channel id. If it isn't, tell the user.
                        if (Context.Channel.Id != CurrentGuildConfig.PollsChannelID)
                        {
                            IChannel pollsChannel = await Context.Guild.GetChannelAsync(CurrentGuildConfig.PollsChannelID);
                            await Context.Channel.SendErrorMessage($"You don't have permission to create a poll here. Go to the `#{pollsChannel.Name}` channel to do that.").ConfigureAwait(false);
                            return;
                        }
                    }
                    else
                    {
                        // No poll channel has been set.
                        await Context.Channel.SendErrorMessage($"The admin(s) haven't set a channel where you can create polls. Please contact them about this.");
                        return;
                    }

                    // If the user doesn't have the Manage Channel permission, they can't create polls that last longer than 30 minutres.
                    if (duration > 30)
                    {
                        await Context.Channel.SendErrorMessage("You can't have a poll that runs for longer than 30 minutes.");
                        return;
                    }
                }

                // Make sure the poll doesn't last longer than 24 hours.
                if (duration > 1440)
                {
                    await ReplyAsync("You can't have a poll that runs for longer than 24 hours.");
                    return;
                }

                // Get the choices, split by comma.
                string[] choices = options.Split(',');
                // If the choices are less than 2, stop here. You can't have a poll with less than 2 options.
                if (choices.Length < 2)
                {
                    await Context.Channel.SendErrorMessage("You need to have more than 2 options.");
                    return;
                }

                // Make sure there aren't more than 10 choices.
                if (choices.Length > 10)
                {
                    await Context.Channel.SendErrorMessage("You can't have more than 10 options.");
                    return;
                }

                // Go through each option and make sure they aren't longer than 240 characters.
                for (int i = 0; i < choices.Length; i++)
                {
                    if (choices[i].Length > 240)
                    {
                        await Context.Channel.SendErrorMessage("An option can't be more than 240 characters.");
                        return;
                    }
                }

                // Create the expire time.
                DateTime expireTime = DateTime.UtcNow;
                // Add the duration.
                expireTime = expireTime.AddMinutes(duration);
                // Try to add the poll to the polls service.
                bool result = PollService.AddPoll(Context.Channel.Id, new Poll(Context.Guild.Id, pollName, pollDescription, new List<string>(choices), expireTime));
                // If the add failed (there was already a poll in the channel), stop.
                if (!result)
                {
                    await Context.Channel.SendErrorMessage("There's already an active poll in this channel! Wait for it to finish.");
                    return;
                }

                // Create the embed builder.
                EmbedBuilder pollEmbedBuilder = new EmbedBuilder()
                {
                    Title = pollName,
                    Description = pollDescription,
                    Color = new Color(0, 255, 255),
                    Footer = new EmbedFooterBuilder() { Text = $"Vote using {CurrentGuildConfig.CommandPrefix}vote <number>" }
                };

                // Add the choices to the embed.
                for (int i = 0; i < choices.Length; i++)
                    pollEmbedBuilder.AddInlineField((i + 1).ToString(), choices[i]);

                //Build the embed.
                Embed pollEmbed = pollEmbedBuilder.Build();

                // Send the embed.
                await ReplyAsync("", false, pollEmbed);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("poll stop"), Summary("If there's an active poll in the current channel, it will be stopped."), NoPrivateChat, RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task PollStop()
        {
            try
            {
                // Get the result from ending the poll.
                // True, everything went well.
                // False, it didn't exist.
                bool result = await EndPoll(Context.Channel);
                // If the result is false, it didn't exists. Say so.
                if (!result)
                    await Context.Channel.SendErrorMessage("There's no active poll in this channel!");
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("vote"), Summary("Votes on the currently active poll in the channel."), NoPrivateChat]
        public async Task VoteAsync(int number)
        {
            try
            {
                // Check if there are any polls and if one exists for this channel.
                if (PollService.Polls.Any() && PollService.Polls.ContainsKey(Context.Channel.Id))
                {
                    // Get the poll.
                    Poll poll = PollService.Polls[Context.Channel.Id];
                    // Check if the user who voted has already voted.
                    if (poll.HasVoted(Context.User))
                    {
                        // They have already voted. Stop here.
                        await ReplyAsync(Context.User.Mention + " You've already voted on the current poll.");
                        return;
                    }

                    // Get the vote index.
                    int voteNum = number - 1;
                    // Make sure the vote index is within range.
                    if (voteNum < 0 || voteNum > poll.Options.Count - 1)
                    {
                        await ReplyAsync(Context.User.Mention + " You need to vote with a number between 1 and " + poll.Options.Count);
                        return;
                    }

                    // Vote on the poll.
                    poll.Vote(voteNum);
                    // Add the user to the voted list.
                    poll.VotingUsers.Add(Context.User.Id);
                    // Update the poll file.
                    PollService.UpdatePoll(Context.Channel.Id, poll);
                }
                else
                {
                    // No poll in this channel. Say so.
                    await Context.Channel.SendErrorMessage("There's no pull currently running in this channel that you can vote on.");
                }
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        public async Task<bool> EndPoll(IMessageChannel channel)
        {
            try
            {
                // Create a temporary variable for the poll.
                Poll poll = null;
                // Only get the poll if it exists.
                if (PollService.Polls.ContainsKey(channel.Id))
                    poll = PollService.Polls[channel.Id];

                // Try to remove the poll and get the result.
                bool removalResult = PollService.RemovePoll(channel.Id);
                // If the result failed, just stop here. It didn't exists or the bot is offline.
                if (!removalResult)
                    return removalResult;

                // Create the embed builder.
                EmbedBuilder pollEmbedBuilder = new EmbedBuilder()
                {
                    Title = poll.PollName,
                    Description = poll.PollDescription,
                    Color = new Color(0, 128, 255)
                };
                // Sort the options based on the amount of votes.
                poll.Options.Sort((a, b) => b.Votes.CompareTo(a.Votes));

                // For each poll option, add it to the embed with the results.
                for (int i = 0; i < poll.Options.Count; i++)
                    pollEmbedBuilder.AddInlineField(poll.Options[i].Name, poll.Options[i].Votes);

                // Build the embed.
                Embed pollEmbed = pollEmbedBuilder.Build();

                // Send the result message.
                await channel.SendMessageAsync("A poll has ended! These were the results:", false, pollEmbed);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }
        }
    }
}
