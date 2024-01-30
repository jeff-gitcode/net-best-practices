using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // services.AddAutoMapper(Assembly.GetExecutingAssembly());
        // services.AddValidatorsFromAssembly(typeof(Assembly).Assembly);
        // services.AddMediatR(c => c.
        //     RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        return services;
    }
}