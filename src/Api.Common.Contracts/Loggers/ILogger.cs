using System;

namespace Api.Common.Contracts.Loggers
{
    public interface ILogger
    {
        void Debug(string message);
        void Information(string message);
        void Error(string message);
        void Error(Exception ex);
    }
}
