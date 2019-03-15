using System;
using System.Collections.Generic;

namespace Neo.Core.Shared
{
    // TODO: Add Channels and Files as output channels
    /// <summary>
    ///     Represents a single access point for logging information into different outputs.
    ///     <para>
    ///         This class uses a thread-safe singleton lazy pattern and can only be accessed through the <see cref="Instance"/> property.
    ///     </para>
    /// </summary>
    public sealed class Logger
    {
        /// <summary>
        ///     Returns the only instance of the <see cref="Logger"/>.
        /// </summary>
        public static Logger Instance => instance.Value;

        private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());

        private Logger() { }

        private readonly Dictionary<LogLevel, ConsoleColor> consoleColors = new Dictionary<LogLevel, ConsoleColor> {
            { LogLevel.Debug, ConsoleColor.DarkCyan },
            { LogLevel.Info, ConsoleColor.Cyan },
            { LogLevel.Ok, ConsoleColor.Green },
            { LogLevel.Warn, ConsoleColor.Yellow },
            { LogLevel.Error, ConsoleColor.Magenta },
            { LogLevel.Fatal, ConsoleColor.Red }
        };

        /// <summary>
        ///     Logs the specified message to all outputs.
        /// </summary>
        /// <param name="level">The <see cref="LogLevel"/> of this message.</param>
        /// <param name="message">The content of this message.</param>
        /// <param name="verbose">Whether the message should be displayed more prominently.</param>
        public void Log(LogLevel level, string message, bool verbose = false) {
            if (verbose) {
                Console.WriteLine();

                Console.BackgroundColor = consoleColors[level];
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($" {level.ToString().ToUpper()} ");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = consoleColors[level];
                Console.WriteLine($" {message}");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            } else {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{DateTime.Now.ToLongTimeString()} ");

                Console.ForegroundColor = consoleColors[level];

                Console.Write($"{level.ToString().ToUpper(),-5}");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" {message}");
            }
        }
    }

    /// <summary>
    ///     Specifies the level of a log message.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///     The message contains debug information.
        /// </summary>
        Debug,
        /// <summary>
        ///     The message contains general information.
        /// </summary>
        Info,
        /// <summary>
        ///     The message contains information about a successful action.
        /// </summary>
        Ok,
        /// <summary>
        ///     The message contains warnings.
        /// </summary>
        Warn,
        /// <summary>
        ///     The message contains error information.
        /// </summary>
        Error,
        /// <summary>
        ///     The message contains information about a failed action.
        /// </summary>
        Fatal
    }
}
