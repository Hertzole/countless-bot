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
        public const int MAIN_R = 253;
        public const int MAIN_G = 93;
        public const int MAIN_B = 0;
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

        // Color used for Fun module.
        public const int FUN_R = 0;
        public const int FUN_G = 255;
        public const int FUN_B = 0;
        public static Color FunModule { get; } = new Color(FUN_R, FUN_G, FUN_B);

        // Color used for Help module.
        public const int HELP_R = 255;
        public const int HELP_G = 255;
        public const int HELP_B = 255;
        public static Color HelpModule { get; } = new Color(HELP_R, HELP_G, HELP_B);

        // Color used for Stupid module.
        public const int STUPID_R = 255;
        public const int STUPID_G = 0;
        public const int STUPID_B = 255;
        public static Color StupidModule { get; } = new Color(STUPID_R, STUPID_G, STUPID_B);
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
