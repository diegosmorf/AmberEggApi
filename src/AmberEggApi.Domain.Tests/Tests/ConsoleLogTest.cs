using AmberEggApi.Infrastructure.Loggers;
using Api.Common.Contracts.Loggers;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace AmberEggApi.DomainTests.Tests
{
    [Collection("Domain.Tests.Global.Setup")]
    public class ConsoleLogTest
    {
        [Theory()]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Test")]
        [InlineData("Test Message")]
        [InlineData("Test Message 1231231232145654654 78979789 465465456 21321321231")]
        public async Task WhenDebug_Then_Success(string expectedMessage)
        {
            // arrange            
            var logger = new ConsoleLogger();
            // act
            var info = await logger.Debug(expectedMessage);
            // assert
            info.Message.Should().Be(expectedMessage);
            info.Level.Should().Be(LogLevel.Debug);
        }

        [Theory()]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Test")]
        [InlineData("Test Message")]
        [InlineData("Test Message 1231231232145654654 78979789 465465456 21321321231")]
        public async Task WhenError_Then_Success(string expectedMessage)
        {
            // arrange            
            var logger = new ConsoleLogger();
            // act
            var info = await logger.Error(expectedMessage);
            // assert
            info.Message.Should().Be(expectedMessage);
            info.Level.Should().Be(LogLevel.Error);
        }

        [Theory()]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Test")]
        [InlineData("Test Message")]
        [InlineData("Test Message 1231231232145654654 78979789 465465456 21321321231")]
        public async Task WhenErrorException_Then_Success(string expectedMessage)
        {
            // arrange            
            var logger = new ConsoleLogger();
            // act
            var info = await logger.Error(new Exception(expectedMessage));
            // assert
            info.Message.Should().Be(expectedMessage);
            info.Level.Should().Be(LogLevel.Error);
        }

        [Theory()]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Test")]
        [InlineData("Test Message")]
        [InlineData("Test Message 1231231232145654654 78979789 465465456 21321321231")]
        public async Task WhenInfo_Then_Success(string expectedMessage)
        {
            // arrange            
            var logger = new ConsoleLogger();
            // act
            var info = await logger.Information(expectedMessage);
            // assert
            info.Message.Should().Be(expectedMessage);
            info.Level.Should().Be(LogLevel.Info);
        }
    }
}