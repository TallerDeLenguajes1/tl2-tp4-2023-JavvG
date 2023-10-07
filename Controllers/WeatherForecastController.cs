// Este es un ejemplo de controlador heredado de ControllerBase. Se deben modificar los campos indicados cuando se desee implementear otro controlador

using Microsoft.AspNetCore.Mvc;

namespace tl2_tp4_2023_JavvG.Controllers;       // Cambiar el nombre a "Web_Api.Controllers"

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase     // Cambiar el nombre del controllador (clase)
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    // (*) Referencia a "WeatherForecastController"

    private readonly ILogger<WeatherForecastController> _logger;        // Cambiar (*) por nombre del controlador

    public WeatherForecastController(ILogger<WeatherForecastController> logger)     // Cambiar (*) por el nombre del controlador
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
