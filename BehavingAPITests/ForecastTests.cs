using BehavingAPI;
using BehavingAPI.Controllers;
using FluentAssertions;
using NUnit.Framework;

namespace BehavingAPITests
{
    public class ForecastTests
    {
        WeatherForecastController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new WeatherForecastController(null);
        }

        [Test]
        public void Forecast_Should_be_valid_next_5_days()
        {
            // act
            var weatherForecasts = _controller.Get();

            // assert
            //Assert.AreEqual(5, weatherForecasts.ToList().Count);
            weatherForecasts.Should()
                .NotBeEmpty()
                .And.HaveCount(5)
                .And.ContainItemsAssignableTo<WeatherForecast>();
        }
    }
}