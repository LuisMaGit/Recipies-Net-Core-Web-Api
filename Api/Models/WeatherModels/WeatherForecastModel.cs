using System;

namespace Api.Models.WeatherModels
{
    public class WeatherForecastModel
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }

        public string Configuration { get; set; }
    }
}