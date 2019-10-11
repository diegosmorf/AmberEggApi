using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Api.Common.WebServer.Server
{
    public interface IServerMiddleware
    {
        Task Invoke(HttpContext httpContext);
        bool LogException(HttpContext httpContext, double elapsedMs, Exception ex);
        ILogger LogForErrorContext(HttpContext httpContext);
    }
}