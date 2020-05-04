using Api.Common.WebServer.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Api.Common.WebServer.Tests.UnitTests
{

    [TestFixture]
    public class SerilogMiddlewareTest
    {
        [TestCase("OK")]
        [TestCase("")]
        public async Task WhenISendRequest_Then_ReturnResponseOK(string content)
        {
            //arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            async Task next(HttpContext httpContext) => await httpContext.Response.WriteAsync(content);
            var middleware = new SerilogMiddleware(next);

            //act
            await middleware.Invoke(context);

            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(response.Body);
            var plainTextContent = reader.ReadToEnd();

            //assert
            response.StatusCode
                        .Should()
                        .Be((int)HttpStatusCode.OK);

            plainTextContent
                        .Should()
                        .NotBeNull()
                        .And
                        .Be(content);
        }

        [Test]
        public void WhenException_Then_ReturnError()
        {
            //arrange
            var message = "Error during test.";
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            Task next(HttpContext httpContext) => throw new Exception(message);
            var middleware = new SerilogMiddleware(next);
                        
            //act
            Func<Task> action = async () => { await middleware.Invoke(context); };

            //assert            
            action.Should()
                .Throw<Exception>()
                .WithMessage(message);

        }
    }
}
