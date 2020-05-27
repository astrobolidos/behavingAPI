using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BehavingAPI;
using BehavingAPI.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BehavingAPITests
{
    public class ForecastTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Forecast_Should_be_valid_next_5_days()
        {
            // act
            var response = Helper.GetResult("/weatherforecast").Result;

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var weatherForecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(
                response.Content.ReadAsStringAsync().Result);

            weatherForecasts.Should()
                .NotBeEmpty()
                .And.HaveCount(5)
                .And.ContainItemsAssignableTo<WeatherForecast>();
        }

        [Test]
        public void VerySlowCall_Should_Log_Performance_Message()
        {
            // act
            var response = Helper.GetResult("/WeatherForecast/slow").Result;

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var header = "PerformanceWarning";
            response.Headers.Should().Contain(kv => kv.Key == header);

            var value = response.Headers.GetValues(header).FirstOrDefault();
            value.Should().ContainAll("Request to", "WeatherForecastController", "taking too long.");
        }

    }
}