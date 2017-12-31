using CountlessBot.Classes;
using System;
using System.IO;

namespace CountlessBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Name the window so you can identify the instance.
#if DEBUG
            Console.Title = "CountlessBot - DEBUG";
#else
            Console.CountlessBot = "HertzBot";
#endif

            // Put in try *just in case*.
            try
            {
                Logger.StartNewLogSession();

                // Determines if the bot has been updated.
                bool updated = false;
                // Determines if the bot update should be silent.
                bool silentUpdate = false;

                // Loop through all the arguments given to the program.
                for (int i = 0; i < args.Length; i++)
                {
                    // If one of the arguments is the update argument, set updated to true.
                    if (args[i] == "update-clean" || args[i] == "update")
                        updated = true;
                }

                // Always clean the directory from ".old" files.
                CleanDirectory();

                // Run the start method for the bot.
                new CountlessBot().StartAsync(updated).GetAwaiter().GetResult();
            }
            // If something breaks, post it to the console.
            catch (Exception ex)
            {
                // Use the Logger to log the error.
                Logger.LogCritical("STARTUP FAILED\n" + ex);
                // Read the key so the instance doesn't exit.
                Console.Read();
            }
        }

        /// <summary>
        /// Cleans the current directory from all ".old" files.
        /// </summary>
        private static void CleanDirectory()
        {
            // Get all the old files.
            string[] oldFiles = Directory.GetFiles(BotHelpers.ProcessPath, "*.old");
            // Loop through all the old files and delete them.
            for (int i = 0; i < oldFiles.Length; i++)
                File.Delete(oldFiles[i]);
        }
    }
}
