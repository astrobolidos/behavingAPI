using System;
using BehavingAPI;
using BehavingAPI.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace BehavingAPITests
{
    public class ForecastTests
    {
        WeatherForecastController _controller;
        TestLogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();
            _controller = new WeatherForecastController(_logger);
        }

        [Test]
        public void Forecast_Should_be_valid_next_5_days()
        {
            // act
            var weatherForecasts = _controller.Get();

            // assert
            weatherForecasts.Should()
                .NotBeEmpty()
                .And.HaveCount(5)
                .And.ContainItemsAssignableTo<WeatherForecast>();
        }

        [Test]
        public void VerySlowCall_Should_Log_Performance_Message()
        {
            // act
            var response = Helper.GetResult("/WeatherForecast/slow", _logger).Result;

            // assert
            _logger.Message.Should().Contain("This is taking too long");
        }

    }

    public class TestLogger : ILogger<WeatherForecastController>
    {
        public string Message { get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            Message = state.ToString();
        }
    }

}