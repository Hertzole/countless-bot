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
        public static string WarningsPath { get { return BotHelpers.ProcessPath + "/countlessbot/data/warnings/"; } }

        public WarningService()
        {
            CountlessBot.MinuteTick += CheckWarnings;
        }

        public GuildWarnings GetGuildWarning(IGuild guild)
        {
            GuildWarnings warnings = new GuildWarnings();
            if (File.Exists(WarningsPath + guild.Id + ".json"))
                warnings = JsonConvert.DeserializeObject<GuildWarnings>(File.ReadAllText(WarningsPath + guild.Id + ".json"));

            return warnings;
        }

        public void AddUser(IGuildUser user, int level, string reason = "")
        {
            if (!Directory.Exists(WarningsPath))
                Directory.CreateDirectory(WarningsPath);

            GuildWarnings warnings = GetGuildWarning(user.Guild);

            UserWarning userWarning = warnings.GetWarning(user);
            GuildConfig guildConfig = CountlessBot.Instance.ConfigService.GetGuildConfig(user.Guild);
            UserWarning newWarning = new UserWarning(user, level, reason, guildConfig);
            if (userWarning == null)
            {
                warnings.WarnedUsers.Add(newWarning);
            }
            else
            {
                userWarning = newWarning;
                warnings.UpdateWarning(userWarning);
            }

            WriteWarningToFile(warnings, user.Guild);
        }

        public async void RemoveUser(IGuildUser user)
        {
            GuildConfig guildConfig = CountlessBot.Instance.ConfigService.GetGuildConfig(user.Guild);
            IRole[] roles = new IRole[] { user.Guild.GetRole(guildConfig.Warning1Role), user.Guild.GetRole(guildConfig.Warning2Role), user.Guild.GetRole(guildConfig.Warning3Role) };
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null)
                    await user.RemoveRoleAsync(roles[i]);
            }

            GuildWarnings warnings = GetGuildWarning(user.Guild);
            UserWarning userWarning = warnings.GetWarning(user);
            warnings.WarnedUsers.Remove(userWarning);

            WriteWarningToFile(warnings, user.Guild);
        }

        private void WriteWarningToFile(GuildWarnings warnings, IGuild guild)
        {
            File.WriteAllText(WarningsPath + guild.Id + ".json", JsonConvert.SerializeObject(warnings));
        }

        public Task CheckWarnings(DateTime time)
        {
            foreach (var guild in CountlessBot.Instance.Client.Guilds)
            {
                if (guild == null)
                    continue;

                GuildWarnings warnings = GetGuildWarning(guild);
                for (int i = 0; i < warnings.WarnedUsers.Count; i++)
                {
                    UserWarning userWarning = warnings.WarnedUsers[i];
                    if (DateTime.UtcNow >= userWarning.ExpireTime)
                    {
                        IGuildUser guildUser = guild.GetUser(userWarning.UserID);
                        if (guildUser != null)
                            RemoveUser(guildUser);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
