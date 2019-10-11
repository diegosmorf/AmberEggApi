using Api.Common.WebServer.Server;
using Microsoft.AspNetCore.Builder;

namespace Api.Common.WebServer.Extensions
{
    public static class SerilogMiddlewareExtension
    {
        public static IApplicationBuilder UseSerilogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogMiddleware>();
        }
    }
}