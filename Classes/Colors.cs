using Discord;

namespace CountlessBot.Classes
{
    /// <summary>
    /// A collection of colors used all around the bot.
    /// </summary>
    public static class Colors
    {
        #region Module Colors
        // Color used for the main CountlessBot.
        public const int MAIN_R = 162;
        public const int MAIN_G = 254;
        public const int MAIN_B = 249;
        public static Color MainColor { get; } = new Color(MAIN_R, MAIN_G, MAIN_B);

        // Color used for Admin module.
        public const int ADMIN_R = 255;
        public const int ADMIN_G = 0;
        public const int ADMIN_B = 0;
        public static Color AdminModule { get; } = new Color(ADMIN_R, ADMIN_G, ADMIN_B);

        // Color used for Bot module.
        public const int BOT_R = 58;
        public const int BOT_G = 115;
        public const int BOT_B = 239;
        public static Color BotModule { get; } = new Color(BOT_R, BOT_G, BOT_B);

        // Color used for Config module.
        public const int CONFIG_R = 128;
        public const int CONFIG_G = 128;
        public const int CONFIG_B = 128;
        public static Color ConfigModule { get; } = new Color(CONFIG_R, CONFIG_G, CONFIG_B);

        // Color used for Conversation module.
        public const int CONVERSATION_R = 255;
        public const int CONVERSATION_G = 255;
        public const int CONVERSATION_B = 0;
        public static Color ConversationModule { get; } = new Color(CONVERSATION_R, CONVERSATION_G, CONVERSATION_B);

        // Color used for Games module.
        public const int GAMES_R = 0;
        public const int GAMES_G = 255;
        public const int GAMES_B = 0;
        public static Color GamesModule { get; } = new Color(GAMES_R, GAMES_G, GAMES_B);

        // Color used for Help module.
        public const int HELP_R = 255;
        public const int HELP_G = 255;
        public const int HELP_B = 255;
        public static Color HelpModule { get; } = new Color(HELP_R, HELP_G, HELP_B);

        // Color used for Music module.
        public const int MUSIC_R = 61;
        public const int MUSIC_G = 64;
        public const int MUSIC_B = 255;
        public static Color MusicModule { get; } = new Color(MUSIC_R, MUSIC_G, MUSIC_B);

        // Color used for NSFW module.
        public const int NSFW_R = 255;
        public const int NSFW_G = 128;
        public const int NSFW_B = 0;
        public static Color NSFWModule { get; } = new Color(NSFW_R, NSFW_G, NSFW_B);

        // Color used for Random module.
        public const int RANDOM_R = 255;
        public const int RANDOM_G = 0;
        public const int RANDOM_B = 255;
        public static Color RandomModule { get; } = new Color(RANDOM_R, RANDOM_G, RANDOM_B);

        // Color used for Utility module.
        public const int UTILITY_R = 0;
        public const int UTILITY_B = 124;
        public const int UTILITY_G = 72;
        public static Color UtilityModule { get; } = new Color(UTILITY_R, UTILITY_G, UTILITY_B);
        #endregion

        // Color used for Success messages.
        public const int SUCCESS_R = 0;
        public const int SUCCESS_G = 255;
        public const int SUCCESS_B = 0;
        public static Color Success { get; } = new Color(SUCCESS_R, SUCCESS_G, SUCCESS_B);

        // Color used for Error messages.
        public const int ERROR_R = 255;
        public const int ERROR_G = 0;
        public const int ERROR_B = 0;
        public static Color Error { get; } = new Color(ERROR_R, ERROR_G, ERROR_B);
    }
}
