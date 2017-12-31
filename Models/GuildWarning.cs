using Discord;
using System;
using System.Collections.Generic;

namespace CountlessBot.Models
{
    /// <summary>
    /// Class for warnings on a guild.
    /// </summary>
    public class GuildWarnings
    {
        // All the warned users on the guild.
        public List<UserWarning> WarnedUsers { get; set; } = new List<UserWarning>();

        /// <summary>
        /// Easily get a warning for a specific user, if one exists.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserWarning GetWarning(IUser user)
        {
            for (int i = 0; i < WarnedUsers.Count; i++)
            {
                if (user.Id == WarnedUsers[i].UserID)
                    return WarnedUsers[i];
            }

            return null;
        }

        /// <summary>
        /// Update the warning for a user.
        /// </summary>
        /// <param name="newWarning"></param>
        public void UpdateWarning(UserWarning newWarning)
        {
            for (int i = 0; i < WarnedUsers.Count; i++)
            {
                if (newWarning.UserID == WarnedUsers[i].UserID)
                {
                    WarnedUsers[i] = newWarning;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Class for user specific warning.
    /// </summary>
    public class UserWarning
    {
        // The user with the warning.
        public ulong UserID { get; set; }
        // The time when the warning expires.
        public DateTime ExpireTime { get; set; }
        // The reason the user was warned.
        public string Reason { get; set; }

        public UserWarning() { }

        public UserWarning(IUser user, int level, string reason, GuildConfig guildConfig)
        {
            UserID = user.Id;
            DateTime time = DateTime.Now;
            if (level == 1)
                time = AddDuration(time, guildConfig.Warning1Duration);
            else if (level == 2)
                time = AddDuration(time, guildConfig.Warning2Duration);
            else if (level == 3)
                time = AddDuration(time, guildConfig.Warning3Duration);

            ExpireTime = time;
            Reason = reason;
        }

        private DateTime AddDuration(DateTime time, TimeDuration duration)
        {
            for (int i = 0; i < duration.Weeks; i++)
                time = time.AddDays(7);
            time = time.AddDays(duration.Days);
            time = time.AddHours(duration.Hours);
            time = time.AddMinutes(duration.Minutes);
            time = time.AddSeconds(duration.Seconds);

            return time;
        }
    }
}
