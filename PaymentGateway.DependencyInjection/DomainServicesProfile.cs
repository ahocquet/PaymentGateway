using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.DependencyInjection
{
    public static class DomainServicesProfile
    {
        public static void Register(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddTransient<IDowJonesAnalysisProvider, DowJonesAnalysisProvider>();
        }
    }
}
