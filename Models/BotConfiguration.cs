using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CountlessBot.Models
{
    public class BotConfiguration
    {
        [JsonIgnore]
        private List<string> m_MotdGames = new List<string>() { };

        // The default prefix the bot uses for it's commands.
        public string DefaultCommandPrefix { get; set; } = ".";
        // All the games the bot will switch between.
        public List<string> MotdGames { get; set; }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            AddMissingGames();
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            AddMissingGames();
        }

        /// <summary>
        /// Adds all the games from the default list to the config.
        /// </summary>
        private void AddMissingGames()
        {
            // If games is just null, create it and fill the list and then stop.
            if (MotdGames == null)
            {
                MotdGames = new List<string>();
                MotdGames.AddRange(m_MotdGames);
                return;
            }

            // Loop through all the default games.
            for (int i = 0; i < m_MotdGames.Count; i++)
            {
                // If the games list doesn't contain the game, add it.
                if (!MotdGames.Contains(m_MotdGames[i]))
                    MotdGames.Add(m_MotdGames[i]);
            }
        }
    }
}
