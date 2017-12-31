using CountlessBot.Classes;
using CountlessBot.Models;
using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CountlessBot.Services
{
    public class ConfigService
    {
        /// <summary>The credentials of the bot. Like token, owner IDs, etc.</summary> 
        public BotCredentials Credentials { get; private set; }
        /// <summary>The bot config. Allows manipulation of the bot without editing code.</summary>
        public BotConfiguration Configuration { get; private set; }
        /// <summary>All the guild configs loaded.</summary>
        public Dictionary<ulong, GuildConfig> GuildConfigs { get; private set; }

        /// <summary>
        /// Creates the service and automatically sets up required fields.
        /// </summary>
        public ConfigService()
        {
            // Create a new dictionary, ready to be filled with info.
            GuildConfigs = new Dictionary<ulong, GuildConfig>();

            ReloadCredentials();
            ReloadConfiguration();
        }

        /// <summary>
        /// Reloads the credentials file.
        /// </summary>
        public void ReloadCredentials()
        {
            Credentials = GetCredentials();
        }

        /// <summary>
        /// Reloads the configuration file.
        /// </summary>
        public void ReloadConfiguration()
        {
            Configuration = GetConfiguration();
        }

        /// <summary>
        /// Gets the bot credentials.
        /// </summary>
        public BotCredentials GetCredentials()
        {
            return GetConfigFile<BotCredentials>(CountlessBot.DataPath, "credentials", true);
        }

        /// <summary>
        /// Gets the bot credentials.
        /// </summary>
        public BotConfiguration GetConfiguration()
        {
            return GetConfigFile<BotConfiguration>(CountlessBot.DataPath, "bot-config", true);
        }

        /// <summary>
        /// Updates the bot config.
        /// </summary>
        /// <param name="newConfig">The config to be written.</param>
        public void UpdateConfig(BotConfiguration newConfig)
        {
            try
            {
                // Make sure the path exists.
                if (!Directory.Exists(CountlessBot.DataPath))
                    Directory.CreateDirectory(CountlessBot.DataPath);

                // Write the new config.
                File.WriteAllText(CountlessBot.DataPath + "bot-config.json", JsonConvert.SerializeObject(newConfig, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets a guild configuration.
        /// </summary>
        /// <param name="guild">The guild you want to get a config from.</param>
        /// <returns></returns>
        public GuildConfig GetGuildConfig(IGuild guild)
        {
            return GetGuildConfig(guild.Id);
        }

        /// <summary>
        /// Gets a guild configuration.
        /// </summary>
        /// <param name="guild">The guild you want to get a config from.</param>
        /// <returns></returns>
        public GuildConfig GetGuildConfig(ulong guildId)
        {
            // Make sure the server data path exists.
            if (!Directory.Exists(CountlessBot.ServerDataPath))
                Directory.CreateDirectory(CountlessBot.ServerDataPath);

            // If the guild ID already exists, return the guild.
            if (GuildConfigs.ContainsKey(guildId))
            {
                return GuildConfigs[guildId];
            }
            else
            {
                // If there's no guild ID loaded, get the guild ID from a file and add it to the list.
                GuildConfig config = GetConfigFile(CountlessBot.ServerDataPath, guildId.ToString(), false, new GuildConfig() { CommandPrefix = Configuration.DefaultCommandPrefix });
                GuildConfigs.Add(guildId, config);
                return config;
            }
        }

        /// <summary>
        /// Updates a guild config.
        /// </summary>
        /// <param name="guildId">The id of the guild to update.</param>
        /// <param name="newConfig">The new config to write.</param>
        public void UpdateGuildConfig(ulong guildId, GuildConfig newConfig)
        {
            try
            {
                // Make sure the server data path exists.
                if (!Directory.Exists(CountlessBot.ServerDataPath))
                    Directory.CreateDirectory(CountlessBot.ServerDataPath);

                // Write the new config.
                File.WriteAllText(CountlessBot.ServerDataPath + guildId + ".json", JsonConvert.SerializeObject(newConfig, Formatting.None));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }
        }

        private T GetConfigFile<T>(string dataPath, string fileName, bool createExample = false, T defaultValue = default(T))
        {
            // Get the path to the actual config file.
            string configPath = dataPath + "/" + fileName + ".json";

            // Make sure the data path exists.
            BotHelpers.CheckPath(dataPath);

            // If createExample is true, try to create an example file.
            if (createExample)
            {
                try
                {
                    File.WriteAllText($"{dataPath}/{fileName}_example.json", JsonConvert.SerializeObject((T)Activator.CreateInstance(typeof(T)), Formatting.Indented));
                }
                catch (Exception ex)
                {
                    // Something went wrong with writing the file. Log what happened to the console.
                    Logger.LogCritical($"Error writring {fileName}_example!\n{ex}");
                }
            }

            // Create the config object that will be returned.
            T config = defaultValue;

            // If the file already exists, load it.
            if (File.Exists(configPath))
                config = JsonConvert.DeserializeObject<T>(File.ReadAllText(configPath));

            // If no credentials were loaded, create them.
            if (config == null)
            {
                try
                {
                    // Create a new object of the generic type.
                    config = (T)Activator.CreateInstance(typeof(T));
                    // Write the object to a file.
                    File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    // Something went horribly wrong.
                    Logger.LogCritical($"Error writing {typeof(T).Name}!\n{ex}");
                }
            }

            // Finally, return the config.
            return config;
        }
    }
}
