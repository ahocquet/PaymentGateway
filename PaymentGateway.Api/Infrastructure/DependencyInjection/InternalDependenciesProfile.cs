using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Api.Infrastructure.DependencyInjection
{
    public static class InternalDependenciesProfile
    {
        public static void Bootstrap(IServiceCollection container)
        {
            container.AddTransient<ErrorHandlingMiddleware>();
            container.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>());
        }
    }
}
