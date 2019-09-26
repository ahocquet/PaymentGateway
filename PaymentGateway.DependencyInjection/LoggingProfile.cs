using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.DependencyInjection
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public static class LoggingProfile
    {
        public static void RegisterApiLogger(IServiceCollection container, string apiName)
        {
            container.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger(apiName));
        }
    }
}
