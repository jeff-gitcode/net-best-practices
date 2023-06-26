using Coravel.Invocable;
using coravel_demo;
using Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Services;

public class CheckForWeatherUpdates : IInvocable
{
    public CheckForWeatherUpdates(WeatherServices services, IHubContext<WeatherHub, IWeatherHub> hub)
    {
        Hub = hub;
        Services = services;
    }

    public IHubContext<WeatherHub, IWeatherHub> Hub { get; }
    public WeatherServices Services { get; }
    private static readonly Random rnd = new Random();

    public async Task Invoke()
    {
        Console.WriteLine("Checking for weather updates..." + DateTime.Now);
        WeatherForecast[] forecasts = Services.GetForecasts().ToArray();
        await Hub.Clients.All.WeatherUpdated(forecasts[rnd.Next(0, forecasts.Length)]);
    }
}