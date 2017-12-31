using System;
using System.Collections.Generic;
#if !DEBUG
using System.IO;
using System.Text;
#endif

namespace CountlessBot.Classes
{
    /// <summary>
    /// Holds info about log entries.
    /// </summary>
    public class LogEntry
    {
        // The timestamp of when the log was created.
        public string Timestamp { get; set; } = "";
        // The log message.
        public string Message { get; set; } = "";

        public LogEntry(string timestamp, string message)
        {
            this.Timestamp = timestamp;
            this.Message = message;
        }
    }

    /// <summary>
    /// Logs messages to the console in a easy to read state.
    /// </summary>
    public static class Logger
    {
        // All the logs created in this session.
        private static HashSet<LogEntry> Logs { get; set; } = new HashSet<LogEntry>();

        // The default color.
        private const ConsoleColor COLOR_DEFAULT = ConsoleColor.White;
        private const ConsoleColor BACKGROUND_DEFAULT = ConsoleColor.Black;
        // The color for success messages.
        private const ConsoleColor COLOR_SUCCESS = ConsoleColor.Green;
        private const ConsoleColor BACKGROUND_SUCCESS = ConsoleColor.Black;
        // The color for warning messages.
        private const ConsoleColor COLOR_WARNING = ConsoleColor.Yellow;
        private const ConsoleColor BACKGROUND_WARNING = ConsoleColor.Black;
        // The color for error messages.
        private const ConsoleColor COLOR_ERROR = ConsoleColor.Red;
        private const ConsoleColor BACKGROUND_ERROR = ConsoleColor.Black;
        // The color for critical messages.
        private const ConsoleColor COLOR_CRITICAL = ConsoleColor.White;
        private const ConsoleColor BACKGROUND_CRITICAL = ConsoleColor.Red;

        #region Log
        /// <summary>
        /// Writes the current line terminator to the standard output stream.
        /// </summary>
        public static void Log()
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(null);
        }

        /// <summary>
        /// Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(bool value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.
        /// </summary>
        public static void Log(char value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(char[] buffer)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(buffer);
        }

        /// <summary>
        /// Writes the text representation of the specified Decimal value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(decimal value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(double value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(int value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(long value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(object value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(float value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void Log(string value)
        {
            Console.ForegroundColor = COLOR_DEFAULT;
            Console.BackgroundColor = BACKGROUND_DEFAULT;
            WriteLine(value);
        }
        #endregion

        #region LogSuccess
        /// <summary>
        /// Writes the current line terminator to the standard output stream.
        /// </summary>
        public static void LogSuccess()
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(null);
        }

        /// <summary>
        /// Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(bool value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.
        /// </summary>
        public static void LogSuccess(char value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(char[] buffer)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(buffer);
        }

        /// <summary>
        /// Writes the text representation of the specified Decimal value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(decimal value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(double value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(int value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(long value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(object value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(float value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogSuccess(string value)
        {
            Console.ForegroundColor = COLOR_SUCCESS;
            Console.BackgroundColor = BACKGROUND_SUCCESS;
            WriteLine(value);
        }
        #endregion

        #region LogWarning
        /// <summary>
        /// Writes the current line terminator to the standard output stream.
        /// </summary>
        public static void LogWarning()
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(null);
        }

        /// <summary>
        /// Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(bool value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.
        /// </summary>
        public static void LogWarning(char value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(char[] buffer)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(buffer);
        }

        /// <summary>
        /// Writes the text representation of the specified Decimal value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(decimal value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(double value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(int value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(long value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(object value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(float value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogWarning(string value)
        {
            Console.ForegroundColor = COLOR_WARNING;
            Console.BackgroundColor = BACKGROUND_WARNING;
            WriteLine(value);
        }
        #endregion

        #region LogError
        /// <summary>
        /// Writes the current line terminator to the standard output stream.
        /// </summary>
        public static void LogError()
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(null);
        }

        /// <summary>
        /// Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(bool value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.
        /// </summary>
        public static void LogError(char value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(char[] buffer)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(buffer);
        }

        /// <summary>
        /// Writes the text representation of the specified Decimal value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(decimal value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(double value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(int value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(long value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(object value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(float value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogError(string value)
        {
            Console.ForegroundColor = COLOR_ERROR;
            Console.BackgroundColor = BACKGROUND_ERROR;
            WriteLine(value);
        }
        #endregion

        #region LogCritical
        /// <summary>
        /// Writes the current line terminator to the standard output stream.
        /// </summary>
        public static void LogCritical()
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(null);
        }

        /// <summary>
        /// Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(bool value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.
        /// </summary>
        public static void LogCritical(char value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(char[] buffer)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(buffer);
        }

        /// <summary>
        /// Writes the text representation of the specified Decimal value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(decimal value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(double value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(int value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(long value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(object value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(float value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void LogCritical(string value)
        {
            Console.ForegroundColor = COLOR_CRITICAL;
            Console.BackgroundColor = BACKGROUND_CRITICAL;
            WriteLine(value);
        }
        #endregion

        /// <summary>
        /// Creates a pretty timestamp string from a provided time.
        /// </summary>
        /// <returns></returns>
        private static string GetPrettyTimestamp(DateTime time)
        {
            return $"{time.Month.ToString("00")}/{time.Day.ToString("00")}/{time.Year.ToString("00")} - {time.Hour.ToString("00")}:{time.Minute.ToString("00")}.{time.Second.ToString("00")}";
        }

        /// <summary>
        /// Writes a value out to the console and adds it to a log file.
        /// </summary>
        /// <param name="value"></param>
        private static void WriteLine(object value)
        {
            // Create the timestamp.
            string timestamp = GetPrettyTimestamp(DateTime.Now);
            // Write the message to the console.
            Console.WriteLine($"[{timestamp}] {value}");
            // Add the log entry to a list.
            Logs.Add(new LogEntry(timestamp, value.ToString()));
            // Write the log to a file.
            WriteToFile(value, timestamp);
        }

        /// <summary>
        /// Writes to the file where a new bot session started.
        /// </summary>
        public static void StartNewLogSession()
        {
            WriteToFile("\n\n------------ New Bot Session ------------");
        }

        /// <summary>
        /// Write a log entry to a file.
        /// </summary>
        /// <param name="value">The thing you want to write.</param>
        private static void WriteToFile(object value, string timestamp = "")
        {
#if !DEBUG
            // The path to the log file.
            string logsPath = CountlessBot.DataPath + "logs.txt";

            // Make sure the directory exists.
            if (!Directory.Exists(CountlessBot.DataPath))
            {
                // If it didn't exist, create it.
                Directory.CreateDirectory(CountlessBot.DataPath);
            }

            // Text that was already in the logs file.
            string oldText = "";
            // If the old logs file exists, supply the old text with that text.
            if (File.Exists(logsPath))
                oldText = File.ReadAllText(logsPath);

            // Timestamp that will be printed. Create it based on if the provided timestamp is empty.
            string timestampText = string.IsNullOrEmpty(timestamp) ? "" : $"[{timestamp}]";

            // Create a new string builder based on the old text.
            StringBuilder builder = new StringBuilder(oldText);
            // Add a new line to the builder.
            builder.AppendLine($"{timestampText} {value}");

            // Lastly, write the text to a file.
            File.WriteAllText(CountlessBot.DataPath + "logs.txt", builder.ToString());
#endif
        }
    }
}
