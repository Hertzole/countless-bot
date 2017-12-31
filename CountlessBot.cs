using CountlessBot.Classes;
using CountlessBot.Models;
using CountlessBot.Modules;
using CountlessBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace CountlessBot
{
    public class CountlessBot
    {
        // Event for when a specific time has been reached.
        public delegate Task TimeEvent(DateTime time);

        // The current bot version.
        public static string Version { get; } = "2.0";

        // The service that handles the commands.
        public CommandService CommandsService { get; private set; }
        // The actual bot client.
        public DiscordSocketClient Client { get; private set; }
        // The credentials of the bot. Like token, owner IDs, etc.
        public BotCredentials Credentials { get { return ConfigService.Credentials; } }
        // The bot config. Allows manipulation of the bot without editing code.
        public BotConfiguration Configuration { get { return ConfigService.Configuration; } }
        // The time when the bot was started.
        public DateTime StartTime { get; private set; }

        // A simple check used to see if the bot has been updated.
        private bool updated = false;
        // A simple check used to see if the bot should announce the update.
        private bool silentUpdate = false;
        // A simple check used to see if the bot has executed the ready function.
        private bool isReady = false;

        // The main path to where most of the data related to the bot is stored.
        public static string DataPath { get { return BotHelpers.ProcessPath + "/countlessbot/data/"; } }
        // The main path to where most of the server related data is stored.
        public static string ServerDataPath { get { return BotHelpers.ProcessPath + "/countlessbot/servers/"; } }
        // The main path to where updates are delivered.
        public static string UpdatePath { get { return BotHelpers.ProcessPath + "/update/"; } }

        // Event called every minute.
        public static event TimeEvent MinuteTick;
        // Event called every five minutes.
        public static event TimeEvent FiveMinuteTick;
        // Event called every hour.
        public static event TimeEvent HourTick;

        // The config service.
        public ConfigService ConfigService { get; private set; }
        // The poll service.
        public PollsService PollService { get; private set; }
        // The warnings service.
        public WarningService WarningService { get; private set; }

        // All the owner chats.
        private static List<IDMChannel> ownerChannels = new List<IDMChannel>();
        // All the games the bot switches between.
        private static List<string> games = new List<string>();

        // The formatting of the JSOn files. Switches between Debug and Release.
#if DEBUG
        private const Newtonsoft.Json.Formatting JSON_FORMATTING = Newtonsoft.Json.Formatting.Indented;
#else
        private const Newtonsoft.Json.Formatting JSON_FORMATTING = Newtonsoft.Json.Formatting.None;
#endif

        /// <summary>The current instance of the main bot.</summary>
        public static CountlessBot Instance { get; private set; }

        public CountlessBot()
        {
            // Set the instance.
            Instance = this;
        }

        /// <summary>
        /// Starts the bot.
        /// </summary>
        public async Task StartAsync(bool updated, bool silentUpdate = false)
        {
            try
            {
                // Log the bot version.
                Logger.LogSuccess("Starting Countlessbot v" + Version);

                // Set the updated bool to the one given.
                this.updated = updated;
                // Set the silent update bool to the one given.
                this.silentUpdate = silentUpdate;
                // Set the start time to the current time.
                StartTime = DateTime.UtcNow;

                // Create services.
                CommandsService = new CommandService(new CommandServiceConfig() { CaseSensitiveCommands = false, DefaultRunMode = RunMode.Async });
                ConfigService = new ConfigService();
                PollService = new PollsService();
                WarningService = new WarningService();

                // Load the games from the config.
                games = Configuration.MotdGames;

                // Make sure credentials are actually loaded.
                if (Credentials == null)
                {
                    // Credentials aren't loaded, send a error message.
                    Logger.LogCritical("Credentials aren't loaded! Make sure to run LoadCredentials()!");
                    Console.Read();
                    return;
                }

                // Make sure the bot doesn't try and run without the required credentials.
                // Check token.
                if (string.IsNullOrWhiteSpace(Credentials.Token))
                {
                    // No token was found, stop the bot from running.
                    Logger.LogCritical("The bot token is blank! Enter a token into the credentials file before starting the bot!");
                    Console.Read();
                    return;
                }

                // Create the bot client configuration.
                DiscordSocketConfig clientConfig = new DiscordSocketConfig()
                {
                    MessageCacheSize = 10,
                    ConnectionTimeout = int.MaxValue,
                    LogLevel = LogSeverity.Info,
                    AlwaysDownloadUsers = false
                };

                // Create the bot client.
                Client = new DiscordSocketClient(clientConfig);

                // Initialize all the commands.
                await InstallCommands();

                // Setup events.
                Client.Log += OnClientLog;
                Client.UserJoined += OnUserJoined;
                Client.Ready += OnClientReady;

                // Log the bot in.
                await Client.LoginAsync(TokenType.Bot, Credentials.Token);
                // Start the bot.
                await Client.StartAsync();

                // Make sure the instance doesn't close.
                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Gathers all the commands from all the modules and initializes them.
        /// </summary>
        /// <returns></returns>
        private async Task InstallCommands()
        {
            // Add an event listener to handle reacting to commnads.
            Client.MessageReceived += OnGetCommand;

            // Gather all the classes in the assembly.
            Assembly assembly = Assembly.GetEntryAssembly();
            // Loop through every class.
            for (int i = 0; i < assembly.GetTypes().Length; i++)
            {
                // Check if the current class is a subclass of the custom made 'DiscordModule' class.
                if (assembly.GetTypes()[i].IsSubclassOf(typeof(DiscordModule)))
                {
                    // Create an instance of the module.
                    DiscordModule instance = (DiscordModule)Activator.CreateInstance(assembly.GetTypes()[i]);
                    // Add the module to the commands service.
                    await CommandsService.AddModuleAsync(instance.GetType());
                    // Lastly, initialize the module.
                    instance.Initialize();
                }
            }
        }

        private async Task OnGetCommand(SocketMessage socketMessage)
        {
            // Put it all in a Try & Catch, just in case something breaks.
            try
            {
                // Convert the message to a user message.
                var message = socketMessage as SocketUserMessage;
                // And if the message doesn't exist, just stop right here.
                if (message == null)
                    return;

                // Make sure the bot doesn't respond to other bots.
                if (message.Author.IsBot)
                    return;

                // Convert the channel that the messages was in into a guild channel.
                IGuildChannel guildChannel = message.Channel as IGuildChannel;
                // Get the default command prefix.
                string prefix = Configuration.DefaultCommandPrefix;
                // If the guild channel exists, set the prefix to the server set prefix.
                if (guildChannel != null)
                    prefix = ConfigService.GetGuildConfig(guildChannel.Guild).CommandPrefix;

                // Where the prefix should be.
                int argPos = 0;
                #region @ bot info - UNCOMMENT IF WANTED
                // If the user just did @Bot, give them some basic info.
                //if (message.Content.Trim() == $"<@{Client.CurrentUser.Id}>")
                //{
                //    // Create the description.
                //    string description = $"To get a list of all the available modules, write `{prefix}modules`." +
                //        $"\n\nTo get all the commands in a module, write `{prefix}help <module>`." +
                //        $"\n\nTo get help about a specific command, write `{prefix}command <command>`. " +
                //        $"\n\nAlternatively, you can go [here](https://www.hertzole.se/hertzbot/commands) for all the modules and commands." +
                //        $"\n\nYou can also @ me instead of using the prefix.";

                //    // Create the embed.
                //    EmbedBuilder infoEmbedBuilder = new EmbedBuilder()
                //    {
                //        Title = $"{Client.CurrentUser.Username} Quickstart",
                //        Color = Colors.MainColor,
                //        Description = description,
                //    };

                //    // Build the embed.
                //    Embed infoEmbed = infoEmbedBuilder.Build();

                //    // Send the info message.
                //    var infoMsg = await message.Channel.SendMessageAsync("", false, infoEmbed);
                //    // Delete the info message after 20 seconds.
                //    infoMsg.DeleteAfter(20);
                //    // Stop here.
                //    return;
                //}
                #endregion

                // If the message doesn't include the prefix or an @Bot, stop here.
                if (!(message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(Client.CurrentUser, ref argPos)))
                    return;

                // Create the command context.
                var context = new CommandContext(Client, message);
                // Create the command search.
                var search = CommandsService.Search(context, argPos);
                // If the search couldn't find the command, send an "unknown command" message.
                if (!search.IsSuccess)
                {
                    await message.Channel.SendErrorMessage($"Unknown command! Type `{prefix}help <moudle>` or `{prefix}modules` to get some help.").ConfigureAwait(false);
                    return;
                }

                // Get command info.
                var commandInfo = search.Commands.FirstOrDefault().Command;

                // Execute the command and get the result.
                var result = await CommandsService.ExecuteAsync(context, argPos);

                // If the command failed, find out why.
                if (!result.IsSuccess)
                {
                    // Go through each error and react to it.
                    switch (result.Error)
                    {
                        case CommandError.BadArgCount:
                            await message.Channel.SendErrorMessage($"Not enough arguments! Do `{prefix}command {commandInfo.Aliases.FirstOrDefault()}` for all the arguments needed.").ConfigureAwait(false);
                            break;
                        case CommandError.Exception:
                            await message.Channel.SendErrorMessage($"Something really bad happened. This has been reported to the developer!").ConfigureAwait(false);
                            await SendMessageToOwner($"**NO ERROR REPORTING!**\nCommand: {context.Message}\n\n```{result.ErrorReason}```").ConfigureAwait(false);
                            break;
                        case CommandError.MultipleMatches:
                            await message.Channel.SendErrorMessage($"Multiple matches").ConfigureAwait(false);
                            break;
                        case CommandError.ObjectNotFound:
                            await message.Channel.SendErrorMessage($"Object not found").ConfigureAwait(false);
                            await SendMessageToOwner($"Object not found on command \"{context.Message}\"!").ConfigureAwait(false);
                            break;
                        case CommandError.ParseFailed:
                            await message.Channel.SendErrorMessage($"I did not fully understand all your arguments. Make sure you use the correct types! Do `{prefix}command {commandInfo.Aliases.FirstOrDefault()}` if you're still confused!").ConfigureAwait(false);
                            break;
                        case CommandError.UnmetPrecondition:
                            await message.Channel.SendErrorMessage(result.ErrorReason).ConfigureAwait(false);
                            break;
                        case CommandError.Unsuccessful:
                            await message.Channel.SendErrorMessage("Unsuccessful command").ConfigureAwait(false);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex);
            }
        }

        /// <summary>
        /// Called whenever the bot client logs something.
        /// </summary>
        private Task OnClientLog(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Logger.LogCritical("[DISCORD] " + message.Message);
                    break;
                case LogSeverity.Error:
                    Logger.LogError("[DISCORD] " + message.Message);
                    Logger.LogError("[DISCORD] " + message.Exception);
                    break;
                case LogSeverity.Warning:
                    Logger.LogWarning("[DISCORD] " + message.Message);
                    break;
                case LogSeverity.Info:
                    Logger.Log("[DISCORD] " + message.Message);
                    break;
                case LogSeverity.Verbose:
                    Logger.Log("[DISCORD VERBOSE] " + message.Message);
                    break;
                case LogSeverity.Debug:
                    Logger.Log("[DISCORD DEBUG] " + message.Message);
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Called whenever a user joins a Guild that the bot is in.
        /// </summary>
        private async Task OnUserJoined(SocketGuildUser user)
        {
            // Make sure there's a guild available.
            if (user.Guild == null)
                return;

            // Get the server config.
            GuildConfig config = ConfigService.GetGuildConfig(user.Guild);
            // If the default role is not 0, add the role to the user.
            if (config.DefaultRole != 0)
            {
                // Get the role.
                var role = user.Guild.GetRole(config.DefaultRole);
                // Add the role.
                await user.AddRoleAsync(role);
            }

            // If greet new users is true, do so in a private chat.
            if (config.GreetNewUsers)
            {
                // Put in an empty catch to make sure nothing happens if something goes wrong.
                try
                {
                    // Get the private user channel.
                    var userChannel = await user.GetOrCreateDMChannelAsync();
                    // Send the private message.
                    await userChannel.SendMessageAsync(config.GreetMessage);
                }
                catch { }
            }
        }

        private async Task OnClientReady()
        {
            // Only execute if the ready function hasn't been executed before.
            if (!isReady)
            {
                // Make sure to say that the ready function has been executed.
                isReady = true;

                // Setup game changing.
                FiveMinuteTick += ChangeGame;

                // Setup all the timers.
                SetupTimers();

                // Set the bot status to online.
                await Client.SetStatusAsync(UserStatus.Online);
                // Change the game.
                await ChangeGame(DateTime.UtcNow);
            }
        }

        private async Task ChangeGame(DateTime time)
        {
            // Set the game to a random game.
            string game = GetRandomGame();

            // Only set the game if the bot is connected.
            if (Client.ConnectionState == ConnectionState.Connected)
                await Client.SetGameAsync(game);
        }

        /// <summary>
        /// Returns a random game that the bot can use in it's "playing" state.
        /// </summary>
        /// <returns></returns>
        private string GetRandomGame()
        {
            // Shuffle the list.
            string game = "";
            int n = new Random().Next(1, games.Count);
            game = games[n];
            games[n] = games[0];
            games[0] = game;

            // Return the game.
            return game;
        }

        /// <summary>
        /// Sets up all the needed timers.
        /// </summary>
        private void SetupTimers()
        {
            // Get the current time to base all the timers on.
            DateTime now = DateTime.UtcNow;

            // Calculate the time where the minute timer should start.
            DateTime nextMinute = now.AddMinutes(1);
            nextMinute = nextMinute.AddSeconds(-nextMinute.Second);

            // Calculate the time where the five minute timer should start.
            DateTime nextFiveMinutes = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            nextFiveMinutes = nextFiveMinutes.AddMinutes(((now.Minute / 5) + 1) * 5);

            // Calculate the time where the hour timer should start.
            DateTime nextHour = now.AddHours(1);
            nextHour = nextHour.AddSeconds(-nextHour.Second);
            nextHour = nextHour.AddMinutes(-nextHour.Minute);

            // Create the minute timer.
            CreateTimer(now, nextMinute, 60, MinuteTick);
            // Create the five minute timer.
            CreateTimer(now, nextFiveMinutes, 60 * 5, FiveMinuteTick);
            // Create the hour timer.
            CreateTimer(now, nextHour, 60 * 60, HourTick);
        }

        private async void CreateTimer(DateTime currentTime, DateTime nextTime, int intervalInSeconds, TimeEvent timeEvent)
        {
            // Calculate the time until the next minute and get the seconds.
            var secondsUntilStart = (int)(nextTime - currentTime).TotalSeconds;

            // Wait the calculated amounnt.
            await Task.Delay(secondsUntilStart * 1000);

            // Create the timer.
            TimeRepeater repeater = new TimeRepeater
            {
                Interval = intervalInSeconds * 1000,
                Event = timeEvent
            }.Start(true);
        }

        /// <summary>
        /// Sends a message to a guild's default channel. If announcement is true, a fancy embed will be made.
        /// </summary>
        /// <param name="guild">The guild you want to post the message in.</param>
        /// <param name="message">The message you want to send.</param>
        /// <param name="announcement">If true, a fancy embed will be made.</param>
        public static async void SendMessageToGuild(IGuild guild, string message, bool announcement = false)
        {
            await Instance.InternalSendMessageToGuild(guild, message, announcement);
        }

        /// <summary>
        /// Sends a message to a guild's default channel. If announcement is true, a fancy embed will be made.
        /// </summary>
        /// <param name="guild">The guild you want to post the message in.</param>
        /// <param name="message">The message you want to send.</param>
        /// <param name="announcement">If true, a fancy embed will be made.</param>
        private async Task InternalSendMessageToGuild(IGuild guild, string message, bool announcement = false)
        {
            // Get the default channel.
            var defaultChannel = await guild.GetDefaultChannelAsync(CacheMode.AllowDownload);
            // If it's an announcement, create a fancy embed message. If not, just send the message.
            if (announcement)
            {
                // Create the embed.
                EmbedBuilder embedBuilder = new EmbedBuilder()
                {
                    Title = Client.CurrentUser.Username + " Announcement",
                    Description = message,
                    Color = Colors.MainColor
                };

                // Build the embed.
                Embed messageEmbed = embedBuilder.Build();
                // Send the message.
                await defaultChannel.SendMessageAsync("", false, messageEmbed).ConfigureAwait(false);
            }
            else
            {
                // Just send the message to the default channel.
                await defaultChannel.SendMessageAsync(message).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Send a message to all the owners of the bot.
        /// </summary>
        /// <param name="message">The message you want to send.</param>
        public static async Task SendMessageToOwner(string message)
        {
            await Instance.InternalSendMessageToOwner(message);
        }

        /// <summary>
        /// Send a message to all the owners of the bot.
        /// </summary>
        /// <param name="message">The message you want to send.</param>
        private async Task InternalSendMessageToOwner(string message)
        {
            // If the amount of owner channels doesn't match the owners in the credentials, create them.
            if (ownerChannels == null || ownerChannels.Count != Credentials.OwnerIDs.Count)
                await CreateOwnerChannels();

            // Go through every owner channel and send the message.
            for (int i = 0; i < ownerChannels.Count; i++)
            {
                // If the message if above 2000 characters (Discord limit), send it as a file instead.
                if (message.Length >= 2000)
                    await ownerChannels[i].SendFileAsync(message.ToStream(), message.Substring(0, 12) + "...");
                else
                    await ownerChannels[i].SendMessageAsync(message);
            }
        }

        /// <summary>
        /// Creates owner owner channels.
        /// </summary>
        /// <returns></returns>
        private async Task CreateOwnerChannels()
        {
            ownerChannels = (await Task.WhenAll(Client.Guilds.SelectMany(g => g.Users).Where(u => Credentials.OwnerIDs.Contains(u.Id))
                .Distinct(new GuildUserComparer()).Select(async u =>
                {
                    try
                    {
                        return await u.GetOrCreateDMChannelAsync();
                    }
                    catch
                    {
                        return null;
                    }
                }))).Where(ch => ch != null).OrderBy(x => Credentials.OwnerIDs.IndexOf(x.Id)).ToList();
        }

        #region Process related functions
        /// <summary>
        /// Shutsdown the bot.
        /// </summary>
        public static async void Shutdown()
        {
            await Instance.InternalShutdown();
        }

        /// <summary>
        /// Shutsdown the bot.
        /// </summary>
        private async Task InternalShutdown()
        {
            // Set the bot's status to invisible.
            await Client.SetStatusAsync(UserStatus.Invisible);
            // Wait 2 seconds.
            await Task.Delay(6000).ConfigureAwait(false);
            // Exit.
            Environment.Exit(0);
        }

        /// <summary>
        /// Restarts the bot.
        /// </summary>
        public static async void Restart()
        {
            await Instance.InternalRestart();
        }

        /// <summary>
        /// Restarts the bot.
        /// </summary>
        private async Task InternalRestart()
        {
            //Set the bot's status to invisible.
            await Client.SetStatusAsync(UserStatus.Invisible);
            // Wait 2 seconds.
            await Task.Delay(6000).ConfigureAwait(false);
            // Start the new instance.
            Process.Start(BotHelpers.FullProcessPath);
            // Close the current instance.
            Environment.Exit(0);
        }

        /// <summary>
        /// Updates the bot.
        /// </summary>
        /// <param name="silent">If true, the update will not be annoucned.</param>
        public static bool Update(bool silent)
        {
            // Check to make sure if the update folder exists.
            if (!Directory.Exists(UpdatePath))
            {
                // If it didn't exist, create it.
                Directory.CreateDirectory(UpdatePath);
                // And if it didn't exist, there will obviously not be any updates.
                return false;
            }

            // Get all the new files in the update folder.
            string[] newFiles = Directory.GetFiles(UpdatePath);

            // If there are no new files, stop here.
            if (newFiles.Length == 0)
                return false;

            // Loop through all the new files.
            for (int i = 0; i < newFiles.Length; i++)
            {
                // Get the path of the current file.
                string currentFilePath = BotHelpers.ProcessPath + "/" + Path.GetFileName(newFiles[i]);
                // If the file exists, mark it as "old".
                if (File.Exists(currentFilePath))
                {
                    // Make sure an old file doesn't exist already. If it does, delete it.
                    if (File.Exists(currentFilePath + ".old"))
                        File.Delete(currentFilePath + ".old");

                    Logger.Log(currentFilePath);
                    // "Move" the current file to the old variant.
                    File.Move(currentFilePath, currentFilePath + ".old");
                }

                // Move the new file into the folder.
                File.Move(newFiles[i], currentFilePath);
            }

            // Generate the silent update arg string.
            string silentUpdate = silent ? " silent-update" : "";
            // Start the new instance.
            Process.Start(BotHelpers.FullProcessPath, $"update{silentUpdate}");
            // Close the current instance.
            Environment.Exit(0);

            // Finally return true, as in everything needed to be updated.
            return true;
        }
        #endregion
    }

    public class TimeRepeater
    {
        // The actual time.
        public Timer Timer { get; set; }
        // The interval of the timer.
        public float Interval { get; set; }
        // The event that shoul be called.
        public CountlessBot.TimeEvent Event { get; set; }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <returns></returns>
        public TimeRepeater Start(bool invokeOnStart)
        {
            Logger.LogSuccess("Timer started!");

            // Creates the timer and sets the interval.
            Timer = new Timer { Interval = Interval };
            // When the timer is elapsed, execute the Invoke method.
            Timer.Elapsed += Invoke;
            // Start the timer.
            Timer.Start();

            // If invokeOnStart is true, invoke the event.
            if (invokeOnStart)
                Event?.Invoke(DateTime.UtcNow);

            return this;
        }

        private void Invoke(object sender, ElapsedEventArgs e)
        {
            // If the event isn't null, invoke it with the current time.
            Event?.Invoke(DateTime.UtcNow);
        }
    }

    public class GuildUserComparer : IEqualityComparer<IGuildUser>
    {
        public bool Equals(IGuildUser x, IGuildUser y) => x.Id == y.Id;
        public int GetHashCode(IGuildUser obj) => obj.Id.GetHashCode();
    }
}
