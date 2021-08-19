using System;
using System.Collections.Generic;
using System.Linq;
using Api.Configuration;
using Api.Models.WeatherModels;
using Microsoft.Extensions.Options;

namespace Api.Services.WeatherService
{
    public class WeatherService : IWeatherService
    {
        private readonly IOptions<WeatherConfiguration> _configurationService;

        public WeatherService(IOptions<WeatherConfiguration> configurationService)
        {
            _configurationService = configurationService;
        }

        public IEnumerable<WeatherForecastModel> GetWeather()
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)],
                Configuration = _configurationService.Value.WeatherTestConfigurationValue,
            }).ToArray();
        }
    }
}