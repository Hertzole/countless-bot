using CountlessBot.Classes;
using CountlessBot.Models;
using Discord;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CountlessBot.Services
{
    public class WarningService
    {
        // The path where all the warnings are stored.
        public static string WarningsPath { get { return BotHelpers.ProcessPath + "/countlessbot/data/warnings/"; } }

        public WarningService()
        {
            // Subscribe to the five minute tick to check warnings every five minutes.
            CountlessBot.FiveMinuteTick += CheckWarnings;
        }

        /// <summary>
        /// Get all the warnings in a guild.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        public GuildWarnings GetGuildWarning(IGuild guild)
        {
            // Create a new temporary variable.
            GuildWarnings warnings = new GuildWarnings();
            // Check if the file exists. If it does, deserialize it.
            if (File.Exists(WarningsPath + guild.Id + ".json"))
                warnings = JsonConvert.DeserializeObject<GuildWarnings>(File.ReadAllText(WarningsPath + guild.Id + ".json"));

            // Return the warning.
            return warnings;
        }

        /// <summary>
        /// Add a user warning.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="level"></param>
        /// <param name="reason"></param>
        public void AddUser(IGuildUser user, int level, string reason = "")
        {
            try
            {
                // Make sure the warnings path exists.
                if (!Directory.Exists(WarningsPath))
                    Directory.CreateDirectory(WarningsPath);

                // Get the warnings for the guild.
                GuildWarnings warnings = GetGuildWarning(user.Guild);

                // Get the user warning.
                UserWarning userWarning = warnings.GetWarning(user);
                // Get the guild config.
                GuildConfig guildConfig = CountlessBot.Instance.ConfigService.GetGuildConfig(user.Guild);
                // Create a new warning.
                UserWarning newWarning = new UserWarning(user, level, reason, guildConfig);
                // If the old user warning is null, add the new warning to the list.
                // Else update the old warning with the new warning.
                if (userWarning == null)
                {
                    // Add the new warning to the database.
                    warnings.WarnedUsers.Add(newWarning);
                }
                else
                {
                    // Set the old warning to the new warning.
                    userWarning = newWarning;
                    // Update the warning.
                    warnings.UpdateWarning(userWarning);
                }

                // Write the warning to a file.
                WriteWarningToFile(warnings, user.Guild);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Removes a user warning.
        /// </summary>
        /// <param name="user"></param>
        public async void RemoveUser(IGuildUser user)
        {
            try
            {
                // Get the current guild config.
                GuildConfig guildConfig = CountlessBot.Instance.ConfigService.GetGuildConfig(user.Guild);
                // Get all the roles.
                IRole[] roles = new IRole[] { user.Guild.GetRole(guildConfig.Warning1Role), user.Guild.GetRole(guildConfig.Warning2Role), user.Guild.GetRole(guildConfig.Warning3Role) };
                // Loop through every role and make sure it exists and if it does, remove it from the user.
                for (int i = 0; i < roles.Length; i++)
                {
                    if (roles[i] != null)
                        await user.RemoveRoleAsync(roles[i]);
                }

                // Get the warnings.
                GuildWarnings warnings = GetGuildWarning(user.Guild);
                // Get the specific user warning.
                UserWarning userWarning = warnings.GetWarning(user);
                // Remove the user from the database.
                warnings.WarnedUsers.Remove(userWarning);

                // Write the updated warnings to a file.
                WriteWarningToFile(warnings, user.Guild);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Simply writes the warnings to a file.
        /// </summary>
        /// <param name="warnings"></param>
        /// <param name="guild"></param>
        private void WriteWarningToFile(GuildWarnings warnings, IGuild guild)
        {
            File.WriteAllText(WarningsPath + guild.Id + ".json", JsonConvert.SerializeObject(warnings));
        }

        /// <summary>
        /// Checks the warnings.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Task CheckWarnings(DateTime time)
        {
            try
            {
                // Go through every available guild.
                foreach (var guild in CountlessBot.Instance.Client.Guilds)
                {
                    // If the guild is null, just skip it.
                    if (guild == null)
                        continue;

                    // Get the warnings.
                    GuildWarnings warnings = GetGuildWarning(guild);
                    // Go through every user in the warnings.
                    for (int i = 0; i < warnings.WarnedUsers.Count; i++)
                    {
                        // Get the user warning.
                        UserWarning userWarning = warnings.WarnedUsers[i];
                        // If the time is passed the expire time, remove the user warning.
                        if (DateTime.UtcNow >= userWarning.ExpireTime)
                        {
                            // Get the user.
                            IGuildUser guildUser = guild.GetUser(userWarning.UserID);
                            // Remove the user if the user exists.
                            if (guildUser != null)
                                RemoveUser(guildUser);
                        }
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
