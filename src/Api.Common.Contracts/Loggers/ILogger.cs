using System;
using System.Threading.Tasks;

namespace Api.Common.Contracts.Loggers
{
    public interface ILogger
    {
        Task<LogInfo> Debug(string message);
        Task<LogInfo> Information(string message);
        Task<LogInfo> Error(string message);
        Task<LogInfo> Error(Exception ex);
    }
}
