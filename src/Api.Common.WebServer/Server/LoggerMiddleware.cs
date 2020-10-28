using Api.Common.Contracts.Loggers;
using Api.Common.WebServer.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Api.Common.WebServer.Server
{
    public class LoggerMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public LoggerMiddleware(RequestDelegate next, ILogger logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (IsSwagger(httpContext) || httpContext.Request.Method == HttpMethods.Options)
            {
                await next(httpContext);
            }
            else
            {
                var start = Stopwatch.GetTimestamp();

                try
                {
                    await next(httpContext);

                    var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                    var statusCode = httpContext.Response?.StatusCode;

                    var message = $"HTTP {httpContext.Request.Method} {httpContext.Request.GetPath()} responded {statusCode} in {elapsedMs:0.0000} ms";

                    logger.Information(message);
                }
                catch (Exception ex)
                {
                    LogException(httpContext, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex);
                    throw;
                }
            }
        }

        public void LogException(HttpContext httpContext, double elapsedMs, Exception ex)
        {
            var message = $"HTTP {httpContext.Request.Method} {httpContext.Request.GetPath()} responded { (int)HttpStatusCode.InternalServerError} in {elapsedMs:0.0000} ms";

            logger.Error(message);
            logger.Error(ex.ToString());
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }

        public double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}