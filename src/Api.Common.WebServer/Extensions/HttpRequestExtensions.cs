using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Api.Common.WebServer.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetPath(this HttpRequest request)
        {
            return request
                       .HttpContext
                       .Features
                       .Get<IHttpRequestFeature>()?.RawTarget ??
                   request.HttpContext.Request.Path.ToString();
        }
    }
}