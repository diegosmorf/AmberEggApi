using Api.Common.WebServer.Server;
using Microsoft.AspNetCore.Builder;

namespace Api.Common.WebServer.Extensions
{
    public static class ApiResponseMiddlewareExtension
    {
        public static void UseApiResponseWrapperMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ApiResponseMiddleware>();
        }
    }
}