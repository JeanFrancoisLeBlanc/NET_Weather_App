using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace react_weather_app.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    // private static readonly string[] Summaries = new[]
    // {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };

    // private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string city = "New York")
    {
        var apiKey = _config["API_KEY"];
        string url = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}&aqi=no";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Weather API request failed");
        }

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        var current = doc.RootElement.GetProperty("current");

        var weather = new
        {
            City = city,
            Date = DateTime.Now.ToShortDateString(),
            TemperatureF = current.GetProperty("temp_f").GetDecimal(),
            Summary = current.GetProperty("condition").GetProperty("text").GetString(),

        };

        return Ok(weather);
    }
}
