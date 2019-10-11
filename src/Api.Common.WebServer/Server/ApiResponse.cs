using System.Reflection;

namespace Api.Common.WebServer.Server
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = "", object result = null, ApiError apiError = null)
        {
            StatusCode = statusCode;
            Message = message;
            Result = result;
            ResponseException = apiError;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiError ResponseException { get; set; }
        public object Result { get; set; }

        public string Version => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;
    }
}