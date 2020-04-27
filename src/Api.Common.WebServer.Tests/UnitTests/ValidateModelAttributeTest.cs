using Api.Common.WebServer.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Api.Common.WebServer.Tests
{
    [TestFixture]
    public class ValidateModelAttributeTest
    {
        [Test]
        public void WhenInvalidModelState_Then_ReturnBadRequest()
        {
            //arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "error");
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            var sut = new ValidateModelAttribute();

            //act
            sut.OnActionExecuting(context);

            //assert
            context.Result.Should().NotBeNull()
                .And.BeOfType<BadRequestObjectResult>();

        }
    }
}
