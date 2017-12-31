using Discord;
using System;
using System.Collections.Generic;

namespace CountlessBot.Models
{
    /// <summary>
    /// Class for poll handeling.
    /// </summary>
    public class Poll
    {
        /// <summary>
        /// Class for poll option.
        /// </summary>
        public class PollOption
        {
            // The name of the option.
            public string Name { get; set; }
            // How many votes the option has.
            public int Votes { get; set; }

            public PollOption(string name)
            {
                Name = name;
                Votes = 0;
            }
        }

        // The name of the poll.
        public string PollName { get; set; } = "";
        // The poll description.
        public string PollDescription { get; set; } = "";
        // All the available options.
        public List<PollOption> Options { get; set; } = new List<PollOption>();
        // All the users that voted.
        public List<ulong> VotingUsers { get; set; } = new List<ulong>();
        // When the poll ends.
        public DateTime EndsAt { get; set; }

        public Poll() { }

        public Poll(string name, string description, List<string> options, DateTime endsAt)
        {
            PollName = name;
            PollDescription = description;
            options.ForEach(o => Options.Add(new PollOption(o)));
            EndsAt = endsAt;
        }

        /// <summary>
        /// Votes for an option.
        /// </summary>
        /// <param name="index"></param>
        public void Vote(int index)
        {
            Options[index].Votes++;
        }

        /// <summary>
        /// Easily checks if a user has voted.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool HasVoted(IUser user)
        {
            return VotingUsers.Contains(user.Id);
        }
    }
}
