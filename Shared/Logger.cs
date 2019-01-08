using System;
using System.Collections.Generic;

namespace Neo.Core.Shared
{
    public sealed class Logger
    {
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

    public enum LogLevel
    {
        Debug,
        Info,
        Ok,
        Warn,
        Error,
        Fatal
    }
}
