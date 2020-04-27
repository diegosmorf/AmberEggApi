using Api.Common.WebServer.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Api.Common.WebServer.Tests
{

    [TestFixture]
    public class APIResponseMiddlewareTest
    {
        [TestCase("OK")]
        [TestCase("")]
        public async Task WhenISendRequest_Then_ReturnResponseOK(string content)
        {
            //arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            async Task next(HttpContext httpContext) => await httpContext.Response.WriteAsync(content);
            var middleware = new ApiResponseMiddleware(next);

            //act
            await middleware.Invoke(context);

            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(response.Body);
            var plainTextContent = reader.ReadToEnd();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(plainTextContent);

            //assert
            response.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.OK);

            apiResponse.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.OK);

            apiResponse.Message
                        .Should()
                        .Be(HttpStatusCode.OK.ToString());

            apiResponse.IsSuccessRequest
                        .Should()
                        .Be(true);

            apiResponse.Result
                        .Should()
                        .Be(content);
        }

        [Test]
        public async Task WhenISendRequestAndException_Then_ReturnResponseException()
        {
            //arrange
            var errorMessage = "Error unit test ";
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var middleware = new ApiResponseMiddleware((innerHttpContext) =>
            {
                throw new Exception(errorMessage);
            });

            //act
            await middleware.Invoke(context);

            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(response.Body);
            var plainTextContent = reader.ReadToEnd();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(plainTextContent);

            //assert
            response.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.InternalServerError);

            apiResponse.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.InternalServerError);

            apiResponse.Message
                        .Should()
                        .Be(HttpStatusCode.InternalServerError.ToString());

            apiResponse.IsSuccessRequest
                        .Should()
                        .Be(false);

            apiResponse.ExceptionMessage
                        .Should()
                        .Be(errorMessage);
        }

        [Test]
        public async Task WhenISendRequestAndException_Then_ReturnResponseUnauthorizedException()
        {
            //arrange
            var errorMessage = "Unauthorized error unit test ";
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var middleware = new ApiResponseMiddleware((innerHttpContext) =>
            {
                throw new UnauthorizedAccessException(errorMessage);
            });

            //act
            await middleware.Invoke(context);

            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(response.Body);
            var plainTextContent = reader.ReadToEnd();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(plainTextContent);

            //assert
            response.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.Unauthorized);

            apiResponse.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.Unauthorized);

            apiResponse.Message
                        .Should()
                        .Be(HttpStatusCode.Unauthorized.ToString());

            apiResponse.IsSuccessRequest
                        .Should()
                        .Be(false);

            apiResponse.ExceptionMessage
                        .Should()
                        .Be(errorMessage);
        }
    }
}
