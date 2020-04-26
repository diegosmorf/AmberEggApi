using System;
using System.Net;
using System.Reflection;

namespace Api.Common.WebServer.Server
{
    public class ApiResponse
    {
        public ApiResponse()
        {
        }

        public ApiResponse(int statusCode, object result)
        {
            StatusCode = statusCode;
            Message = Enum.Parse<HttpStatusCode>(statusCode.ToString()).ToString();
            Result = result;
            IsOk = true;
        }

        public ApiResponse(int statusCode, Exception ex)
        {
            StatusCode = statusCode;
            Message = Enum.Parse<HttpStatusCode>(statusCode.ToString()).ToString();
            Result = new { ex.Message, ex.StackTrace };
            IsOk = false;
        }

        public int StatusCode { get; set; }
        public bool IsOk{ get; set; }
        public string Message { get; set; }
        public object Result { get; set; }

        public string Version => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;
    }
}