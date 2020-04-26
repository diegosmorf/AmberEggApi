using Api.Common.WebServer.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Api.Common.WebServer.Tests
{
    
    [TestFixture]
    public class APIResponseMiddlewareTest
    {
        [Test]
        public async Task WhenISendRequest_Then_ReturnResponse()
        {
            //arrange
            var jsonContent = "OK";
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = async (HttpContext httpContext) => await httpContext.Response.WriteAsync(jsonContent);
            var middleware = new ApiResponseMiddleware(next);

            //act
            await middleware.Invoke(context);

            //assert
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(response.Body);
            var plainTextContent = reader.ReadToEnd();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(plainTextContent);

            response.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.OK);

            apiResponse.Result
                        .Should()
                        .Be(jsonContent);
        }
    }
}