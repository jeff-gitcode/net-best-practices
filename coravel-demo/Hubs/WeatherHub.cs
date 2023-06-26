using coravel_demo;
using Microsoft.AspNetCore.SignalR;

namespace Hubs;

public class WeatherHub : Hub<IWeatherHub>
{
}

public interface IWeatherHub
{
    Task WeatherUpdated(WeatherForecast weather);
}