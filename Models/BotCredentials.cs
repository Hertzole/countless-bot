using System.Collections.Generic;

namespace CountlessBot.Models
{
    public class BotCredentials
    {
        // The token the bot uses to login.
        public string Token { get; set; } = "";

        // All the users that owns the bot.
        public List<ulong> OwnerIDs { get; set; } = new List<ulong>();

        public BotCredentials() { }

        public BotCredentials(string token, string clientId)
        {
            Token = token;
        }
    }
}
