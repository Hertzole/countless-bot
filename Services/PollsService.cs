using CountlessBot.Classes;
using CountlessBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CountlessBot.Services
{
    public class PollsService
    {
        // The path to where all the polls are stored.
        public static string PollsPath { get { return BotHelpers.ProcessPath + "/countlessbot/polls/"; } }

        // All the polls.
        public Dictionary<ulong, Poll> Polls { get; set; } = new Dictionary<ulong, Poll>();

        public PollsService()
        {
            Polls = GetPollsFromFiles();
        }

        public Dictionary<ulong, Poll> GetPollsFromFiles()
        {
            try
            {
                // Make sure the polls path exists.
                if (!Directory.Exists(PollsPath))
                    Directory.CreateDirectory(PollsPath);

                // Create the polls dictionary.
                Dictionary<ulong, Poll> polls = new Dictionary<ulong, Poll>();

                // Get the polls from the polls path.
                string[] pollFiles = Directory.GetFiles(PollsPath, "*.poll");
                // Go through every poll file.
                for (int i = 0; i < pollFiles.Length; i++)
                {
                    // Get the file name, in this case, the ID of the poll.
                    string fileName = Path.GetFileNameWithoutExtension(pollFiles[i]);
                    // Try to convert the file name to the ID.
                    bool success = ulong.TryParse(fileName, out ulong messageChannelID);
                    // If the convert did not succeed, send a warning and continue to the next poll file.
                    if (!success)
                    {
                        Logger.LogWarning($"Could not load poll '{fileName}' due to it having a invalid name.");
                        continue;
                    }

                    // Deserialize the JSON and convert it to a Poll class.
                    Poll poll = (Poll)JsonConvert.DeserializeObject(File.ReadAllText(pollFiles[i]));
                    // If the poll class is null (the JSON deseralize failed), send a warning and continue to the next poll file.
                    if (poll == null)
                    {
                        Logger.LogWarning($"Could not load poll '{fileName}' due to it having invalid JSON format.");
                        continue;
                    }

                    // Lastly add the poll to the polls dictionary.
                    polls.Add(messageChannelID, poll);
                }

                // Return the dictionary.
                return polls;
            }
            catch (Exception ex)
            {
                // If something went wrong, show it in the console.
                Logger.LogError(ex);
                // Return nothing.
                return null;
            }
        }
    }
}
