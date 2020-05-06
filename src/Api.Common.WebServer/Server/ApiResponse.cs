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
            IsSuccessRequest = true;
        }

        public ApiResponse(int statusCode, Exception ex)
        {
            StatusCode = statusCode;
            Message = Enum.Parse<HttpStatusCode>(statusCode.ToString()).ToString();
            ExceptionMessage = ex.Message;
            ExceptionDetail = ex.ToString();
            IsSuccessRequest = false;
        }

        
        public int StatusCode { get; set; }
        public bool IsSuccessRequest{ get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionDetail { get; set; }
        public object Result { get; set; }

        public string Version => Assembly
                                    .GetEntryAssembly()
                                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                    .InformationalVersion;
    }
}