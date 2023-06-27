public static class DI
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IRestService, RestService>();
        return services;
    }
}