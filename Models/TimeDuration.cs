using System;

namespace CountlessBot.Models
{
    public class TimeDuration
    {
        // The duration in seconds.
        public int Seconds { get; set; } = 0;
        // The duration in minutes.
        public int Minutes { get; set; } = 0;
        // The duration in hours.
        public int Hours { get; set; } = 0;
        // The duration in days.
        public int Days { get; set; } = 0;
        // The duration in weeks.
        public int Weeks { get; set; } = 0;

        // Make it usable with DateTime class.
        public static implicit operator DateTime(TimeDuration t)
        {
            DateTime time = new DateTime();
            time.AddSeconds(t.Seconds);
            time.AddMinutes(t.Minutes);
            time.AddHours(t.Hours);
            time.AddDays(t.Days);
            for (int i = 0; i < t.Weeks; i++)
            {
                time.AddDays(7);
            }
            return time;
        }
    }
}
