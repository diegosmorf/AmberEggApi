using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Api.Common.WebServer.Server
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (SkipUri(context.Request))
            {
                await next(context);
            }
            else
            {
                var originalBodyStream = context.Response.Body;
                context.Response.ContentType = "application/json";
                context.Response.Body = new MemoryStream();

                try
                {
                    await next.Invoke(context);
                    await HandleRequest(context);
                }
                catch (Exception ex)
                {
                    await HandleRequest(context, ex);
                }
                finally
                {
                    await PackageResponse(originalBodyStream, context.Response.Body);
                }

            }
        }

        private async Task PackageResponse(Stream originalBodyStream, Stream responseBody)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private bool SkipUri(HttpRequest request)
        {
            return IsSkipUri(request.Path) || request.Method == HttpMethods.Options;
        }

        private bool IsSkipUri(PathString path)
        {
            var skiplist = new HashSet<string> { "/swagger", "/healthcheck", "/healthchecks-ui" };

            foreach (var uri in skiplist)
            {
                if (path.StartsWithSegments(uri))
                    return true;
            }

            return false;
        }

        private async Task HandleRequest(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (exception.GetBaseException()) switch
            {
                UnauthorizedAccessException _ => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError,
            };
            var code = context.Response.StatusCode;
            var apiResponse = new ApiResponse(code, exception);

            await HandleRequest(context, apiResponse);
        }

        private async Task HandleRequest(HttpContext context)
        {
            var code = context.Response.StatusCode;
            var body = await FormatResponse(context.Response);
            var apiResponse = new ApiResponse(code, body);

            await HandleRequest(context, apiResponse);
        }

        private async Task HandleRequest(HttpContext context, ApiResponse apiResponse)
        {
            var jsonString = JsonConvert.SerializeObject(apiResponse);

            context.Response.Body.SetLength(0L);
            await context.Response.WriteAsync(jsonString);
        }

        private async Task<dynamic> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            body = JsonConvert.SerializeObject(body);
            body = body.Replace("\"", "'");

            return JsonConvert.DeserializeObject<dynamic>(body);
        }
    }
}