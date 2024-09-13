using AmberEggApi.Infrastructure.Loggers;
using Api.Common.Contracts.Loggers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AmberEggApi.DomainTests.Tests
{
    public class ConsoleLogTest
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("Test")]
        [TestCase("Test Message")]
        [TestCase("Test Message 1231231232145654654 78979789 465465456 21321321231")]
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

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("Test")]
        [TestCase("Test Message")]
        [TestCase("Test Message 1231231232145654654 78979789 465465456 21321321231")]
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

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("Test")]
        [TestCase("Test Message")]
        [TestCase("Test Message 1231231232145654654 78979789 465465456 21321321231")]
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

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("Test")]
        [TestCase("Test Message")]
        [TestCase("Test Message 1231231232145654654 78979789 465465456 21321321231")]
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