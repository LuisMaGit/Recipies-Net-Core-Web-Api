using System.Collections.Generic;
using Api.Filters.WeatherFilters;
using Api.Models.WeatherModels;
using Api.Services.WeatherService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ExampleResourceFilter(true)]
    [ExampleActionFilter]
    public class WeatherForecastController : ControllerBase
    {
        IWeatherService _weatherService;

        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecastModel> Get()
        {
            return _weatherService.GetWeather();
        }

        [HttpPost]
        public string Post()
        {
            const int x = 2;
            const int y = 3;
            return $"POST {x} , {y}";
        }

        [HttpGet]
        [Route("{id:int}")]
        public string GetById(int id)
        {
            return id.ToString();
        }
    }
}