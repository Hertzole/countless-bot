namespace CountlessBot.Models
{
    public class GuildConfig
    {
        public const string NEWEST_CONFIG_VERSION = "1";

        // The version of the config. Used for updating.
        public string ConfigVersion { get; set; } = NEWEST_CONFIG_VERSION;
        // The guild ID.
        public ulong GuildID { get; set; }
        // The ID of the owner of the current guild.
        public ulong GuildOwnerID { get; set; }

        // The command prefix for this guild.
        public string CommandPrefix { get; set; } = ".";

        // The channel where polls can be done.
        public ulong PollsChannelID { get; set; } = 0;

        // Determines if the bot should greet new users.
        public bool GreetNewUsers { get; set; } = true;
        // The message that the bot will greet users with.
        public string GreetMessage { get; set; } = "Welcome to my awesome server!";

        // The role the bot should put new users in.
        public ulong DefaultRole { get; set; } = 0;
        // The role the bot should put warning 1 users in.
        public ulong Warning1Role { get; set; } = 0;
        // The role the bot should put warning 2 users in.
        public ulong Warning2Role { get; set; } = 0;
        // The role the bot should put warning 3 users in.
        public ulong Warning3Role { get; set; } = 0;

        // The duration of warning 1.
        public TimeDuration Warning1Duration { get; set; } = new TimeDuration() { Days = 14 };
        // The duration of warning 2.
        public TimeDuration Warning2Duration { get; set; } = new TimeDuration() { Days = 21 };
        // The duration of warning 3.
        public TimeDuration Warning3Duration { get; set; } = new TimeDuration() { Days = 28 };
    }
}
