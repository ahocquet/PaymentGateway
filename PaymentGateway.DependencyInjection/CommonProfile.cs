using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.ApplicationServices;
using PaymentGateway.BankProvider.ACL;
using PaymentGateway.DomainServices;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.Infrastructure;
using PaymentGateway.Infrastructure.Json;
using PaymentGateway.Persistence;
using PaymentGateway.SharedKernel.Date;
using PaymentGateway.SharedKernel.Models;

namespace PaymentGateway.DependencyInjection
{
    public static class CommonProfile
    {
        public static void Register(IServiceCollection serviceCollection, AppSettings appSettings, IConfiguration configuration)
        {
            serviceCollection.AddSingleton(appSettings);
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<PrivateSetterContractResolver>();

            serviceCollection.AddTransient<IProvideDateTime, DateTimeProvider>();
            serviceCollection.AddTransient<IProcessPayment, PaymentProcessor>();
            serviceCollection.AddTransient<IPaymentService, PaymentService>();
            serviceCollection.AddTransient<IAcquiringBank, AcquiringBank>();
            serviceCollection.AddTransient<IEventStore, EventStore>();
            serviceCollection.AddTransient<IEventPublisher, EventPublisher>();

            serviceCollection.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
        }
    }
}
