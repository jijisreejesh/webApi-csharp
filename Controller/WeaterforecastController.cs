using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System;

namespace dotnet_practice.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
      //  private readonly string _connectionString = "Host=192.168.1.10;Port=5432;Database=test;User Id=postgres;Password=PostgresDB@dm1n;"; // Update this with your PostgreSQL connection string
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };
            _logger.LogInformation("Fetching weather forecast data...");

            var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
       // saveData(forecast);
            return forecast;
        }
        //  private void saveData(IEnumerable<WeatherForecast> forecast)
        // {
        //     try
        //     {
        //         using var connection = new NpgsqlConnection(_connectionString);
        //         connection.Open();

        //         using var transaction = connection.BeginTransaction();
        //         foreach (var weather in forecast)
        //         {
        //             using var cmd = new NpgsqlCommand("INSERT INTO weather_forecasts  (date, temperature_c, temperature_f, summary) VALUES (@date, @temperature_c, @temperature_f, @summary)", connection, transaction);
        //             cmd.Parameters.AddWithValue("date", weather.Date);
        //             cmd.Parameters.AddWithValue("temperature_c", weather.TemperatureC);
        //             cmd.Parameters.AddWithValue("temperature_f", weather.TemperatureF);
        //             cmd.Parameters.AddWithValue("summary", weather.Summary);

        //             cmd.ExecuteNonQuery();
        //         }

        //         transaction.Commit();
        //         _logger.LogInformation("Weather forecast data saved to database.");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError($"Error saving weather forecast data: {ex.Message}");
        //     }
        // }
    }
    
    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

}
