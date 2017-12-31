using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CountlessBot.Classes
{
    public class BotHelpers
    {
        private static string fullProcessPath = "";
        /// <summary>
        /// Returns the full path to the current process. File name and everything.
        /// </summary>
        public static string FullProcessPath
        {
            get
            {
                // Only set the value if it's blank. We're caching the value.
                if (string.IsNullOrWhiteSpace(fullProcessPath))
                {
                    // Get the current process.
                    var process = Process.GetCurrentProcess();
                    // Set the process path variable to the process path.
                    fullProcessPath = process.MainModule.FileName;
                }

                // Finally return the full path.
                return fullProcessPath;
            }
        }

        private static string processName = "";
        /// <summary>
        /// Returns the file name of the current process.
        /// </summary>
        public static string ProcessName
        {
            get
            {
                // Only set the value if it's blank. We're caching the value.
                if (string.IsNullOrWhiteSpace(processName))
                {
                    // Set the process name variable by getting the file name of the full path.
                    processName = Path.GetFileName(FullProcessPath);
                }

                // Finally return the process name.
                return processName;
            }
        }

        private static string processPath = "";
        /// <summary>
        /// Returns the folder where the process is being executed from.
        /// </summary>
        public static string ProcessPath
        {
            get
            {
                // Only set the value if it's blank. We're caching the value.
                if (string.IsNullOrWhiteSpace(processName))
                {
                    // Set the process path variable by getting the assembly location.
                    processPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }

                // Finally return the process path.
                return processPath;
            }
        }

        /// <summary>
        /// Checks if a path exists, and if it doesn't, it creates it.
        /// </summary>
        /// <param name="path"></param>
        public static void CheckPath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
