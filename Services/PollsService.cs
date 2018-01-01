using CountlessBot.Classes;
using CountlessBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        /// <summary>
        /// Adds a poll to the database.
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="poll"></param>
        /// <returns></returns>
        public bool AddPoll(ulong channelID, Poll poll)
        {
            try
            {
                // Make sure the polls path exists.
                if (!Directory.Exists(PollsPath))
                    Directory.CreateDirectory(PollsPath);

                // If there are polls and the polls already contains the channel id, stop here.
                if (Polls.Any() && Polls.ContainsKey(channelID))
                    return false;

                // Add the poll.
                Polls.Add(channelID, poll);
                // Write the poll file.
                File.WriteAllText(PollsPath + channelID + ".poll", JsonConvert.SerializeObject(poll));
                // Return a result of true.
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Removes a poll from the database.
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool RemovePoll(ulong channelID)
        {
            try
            {
                // Make sure it doesn't delete anything while being offline.
                if (CountlessBot.Instance.Client.ConnectionState != Discord.ConnectionState.Connected)
                    return false;

                // Make sure the polls path exists.
                if (!Directory.Exists(PollsPath))
                    Directory.CreateDirectory(PollsPath);

                // If there aren't any polls or the poll with the channel id doesn't exist, stop here.
                if (!Polls.Any() || !Polls.ContainsKey(channelID))
                    return false;

                // Remove the poll.
                Polls.Remove(channelID);
                // Check if the file exists and if it does, delete it.
                if (File.Exists(PollsPath + channelID + ".poll"))
                    File.Delete(PollsPath + channelID + ".poll");

                // Return a result of true.
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Updates an existing poll.
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="poll"></param>
        public void UpdatePoll(ulong channelID, Poll poll)
        {
            try
            {
                // Make sure the polls path exists.
                if (!Directory.Exists(PollsPath))
                    Directory.CreateDirectory(PollsPath);

                // If there aren't any polls or the poll with the channel id doesn't exist, stop here.
                if (!Polls.Any() || !Polls.ContainsKey(channelID))
                    return;

                // Update the poll at the channel id.
                Polls[channelID] = poll;
                // Write the new file.
                File.WriteAllText(PollsPath + channelID + ".poll", JsonConvert.SerializeObject(poll));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all the polls from files.
        /// </summary>
        /// <returns></returns>
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
