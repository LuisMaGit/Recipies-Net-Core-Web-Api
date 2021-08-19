
using System.Collections.Generic;
using Api.Models.WeatherModels;

namespace Api.Services.WeatherService
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecastModel> GetWeather();
    }

}