using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BehavingAPI;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BehavingAPITests
{
    public class ForecastTests
    {
        [SetUp]
        public void Setup()
        {
            Helper.Init();
        }

        [Test]
        public async Task Forecast_Should_be_valid_next_5_days()
        {
            // act
            var response = await Helper.Get("/weatherforecast");

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
        public async Task VerySlowCall_Should_Log_Performance_Message()
        {
            // act
            var response = await Helper.Get("/WeatherForecast/slow");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var header = "PerformanceWarning";
            response.Headers.Should().Contain(kv => kv.Key == header);

            var value = response.Headers.GetValues(header).FirstOrDefault();
            value.Should().ContainAll("Request to", "WeatherForecastController", "taking too long.");
        }

        [Test]
        public async Task InvalidPayload_Should_Return_Error()
        {
            // arrange
            var invalidPayload = new Comment { Id = -1, Text = "too small" };

            // act
            var response = await Helper.Post("/WeatherForecast", JsonConvert.SerializeObject(invalidPayload));

            //assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(
                response.Content.ReadAsStringAsync().Result);
            errors.Should().NotBeNull();
            errors.Should()
                .HaveCount(2)
                .And.ContainKeys("Id", "Text");
        }

        [Test]
        public async Task UnhandledExceptions_Should_log_and_return_exceptionMessageonly()
        {
            // arrange
            var invalidPayload = new Comment { Id = 2, Text = "just right size" };

            // act
            try
            {
                await Helper.Put("/WeatherForecast/1", JsonConvert.SerializeObject(invalidPayload));
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType<Exception>();
            }
        }

    }
}