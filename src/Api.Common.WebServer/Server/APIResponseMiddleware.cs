using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Api.Common.WebServer.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
            if (SkipApiResponseMiddleware(context))
            {
                await next(context);
            }
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await next.Invoke(context);
                        await HandleRequestAsync(context);
                    }
                    catch (Exception ex)
                    {
                        await HandleRequestAsync(context, ex);
                    }
                    finally
                    {
                        await PackageResponse(originalBodyStream, responseBody);
                    }
                }
            }
        }

        private async Task PackageResponse(Stream originalBodyStream, MemoryStream responseBody)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private bool SkipApiResponseMiddleware(HttpContext context)
        {
            return IsSwagger(context) || context.Request.Method == HttpMethods.Options;
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }

        private async Task HandleRequestAsync(HttpContext context)
        {
            var body = await FormatResponse(context.Response);

            switch (context.Response.StatusCode)
            {
                case (int) HttpStatusCode.OK:
                    await HandleRequestAsync(context, body, ResponseMessage.Success);
                    break;

                case (int) HttpStatusCode.Unauthorized:
                    await HandleRequestAsync(context, body, ResponseMessage.UnAuthorized);
                    break;

                default:
                    await HandleRequestAsync(context, body, ResponseMessage.Failure);
                    break;
            }
        }

        private Task HandleRequestAsync(HttpContext context, Exception exception)
        {
            ApiError apiError;

            switch (exception.GetBaseException())
            {
                case ApiException ex:
                    apiError = new ApiError(ex.Message)
                    {
                        ValidationErrors = ex.Errors,
                        ReferenceErrorCode = ex.ReferenceErrorCode,
                        ReferenceDocumentLink = ex.ReferenceDocumentLink
                    };
                    context.Response.StatusCode = ex.StatusCode;
                    break;

                case UnauthorizedAccessException ex:
                    apiError = new ApiError(ex.Message);
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    break;

                default:
                    var msg = exception.GetBaseException().Message;
                    var stack = exception.StackTrace;
                    apiError = new ApiError(msg) {Details = stack};
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            return HandleRequestAsync(context, null, ResponseMessage.Exception, apiError);
        }

        private Task HandleRequestAsync(HttpContext context, object body, ResponseMessage message,
            ApiError apiError = null)
        {
            var code = context.Response.StatusCode;
            context.Response.ContentType = "application/json";

            var bodyText = string.Empty;

            if (body != null)
                bodyText = !body.ToString().IsValidJson() ? JsonConvert.SerializeObject(body) : body.ToString();

            var bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            var apiResponse = new ApiResponse(code, message.GetDescription(), bodyContent, apiError);
            var jsonString = JsonConvert.SerializeObject(apiResponse);

            context.Response.Body.SetLength(0L);
            return context.Response.WriteAsync(jsonString);
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText.Replace("\"", "'");
        }
    }
}