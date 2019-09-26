using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Api.Infrastructure.DependencyInjection;
using PaymentGateway.DependencyInjection;
using PaymentGateway.SharedKernel.Models;

namespace PaymentGateway.Api.Infrastructure
{
    public static class IoC
    {
        internal static void SetupIoC(this IServiceCollection serviceCollection, AppSettings appSettings, IConfiguration configuration)
        {
            InternalDependenciesProfile.Bootstrap(serviceCollection);

            CommonProfile.Register(serviceCollection, appSettings, configuration);
            StorageProfile.Register(serviceCollection, appSettings);
            LoggingProfile.RegisterApiLogger(serviceCollection, "Api");
            MediatRProfile.Register(serviceCollection, typeof(Startup).Assembly);
        }
    }
}
