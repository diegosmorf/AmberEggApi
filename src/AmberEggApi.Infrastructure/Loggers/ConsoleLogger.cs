using Api.Common.Contracts.Loggers;
using System;
using System.Threading.Tasks;

namespace AmberEggApi.Infrastructure.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public async Task<LogInfo> Debug(string message)
        {
            return await WriteMessage(message, LogLevel.Debug, ConsoleColor.White);
        }

        public async Task<LogInfo> Error(string message)
        {
            return await WriteMessage(message, LogLevel.Error, ConsoleColor.Red);
        }

        public async Task<LogInfo> Error(Exception ex)
        {
            return await Error(ex.Message);
        }

        public async Task<LogInfo> Information(string message)
        {
            return await WriteMessage(message, LogLevel.Info, ConsoleColor.Green);
        }

        private async Task<LogInfo> WriteMessage(string message, LogLevel level, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();

            return await Task.Run(() => new LogInfo { Level = level, Message = message });
        }
    }
}
