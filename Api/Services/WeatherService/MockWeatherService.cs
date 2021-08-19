using System;
using System.Collections.Generic;
using Api.Models.WeatherModels;

namespace Api.Services.WeatherService
{
    public class MockWeatherService : IWeatherService
    {
        public IEnumerable<WeatherForecastModel> GetWeather()
        {
            WeatherForecastModel[] fake = {
                new WeatherForecastModel{
                    Date =  DateTime.Now,
                    TemperatureC = 0,
                    Summary = "FAKE SUMARY"
                },
            };
            return fake;
        }
    }
}