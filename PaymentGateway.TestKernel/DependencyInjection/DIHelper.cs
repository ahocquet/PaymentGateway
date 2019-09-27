using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PaymentGateway.DependencyInjection;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.SharedKernel.Models;
using PaymentGateway.TestKernel.Extensions;

namespace PaymentGateway.TestKernel.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public class DIHelper
    {
        public static ServiceProvider BuildIntegrationTestContainer(ServiceCollection serviceCollection = null)
        {
            serviceCollection = serviceCollection ?? new ServiceCollection();
            return BuildContainer(serviceCollection, true);
        }

        public static ServiceProvider BuildUnitTestContainer(ServiceCollection serviceCollection = null)
        {
            serviceCollection = serviceCollection ?? new ServiceCollection();
            return BuildContainer(serviceCollection, false);
        }

        private static ServiceProvider BuildContainer(IServiceCollection serviceCollection, bool isIntegrationTests)
        {
            var (config, appSettings) = isIntegrationTests ? BuildAppSettings() : BuildFakeAppSettings();

            RegisterTestDependencies(serviceCollection, appSettings);
            StorageProfile.Register(serviceCollection, appSettings);
            CommonProfile.Register(serviceCollection, appSettings, config);
            MediatRProfile.Register(serviceCollection);
            AutoMapperProfile.Register(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        private static void RegisterTestDependencies(IServiceCollection container, AppSettings appSettings)
        {
            container.AddSingleton(appSettings);

            container.RegisterSubstitute<ILogger>();
            container.RegisterSubstitute<IEventPublisher>();
        }

        public static (IConfigurationRoot moqConfig, AppSettings settings) BuildAppSettings()
        {
            var configurationBuilder = new ConfigurationBuilder()
                                      .SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json");

            var config   = configurationBuilder.Build();
            var settings = new AppSettings();
            config.Bind(settings);

            return (config, settings);
        }

        private static (IConfigurationRoot moqConfig, AppSettings settings) BuildFakeAppSettings()
        {
            var configurationRoot = Substitute.For<IConfigurationRoot>();
            configurationRoot[Arg.Any<string>()].Returns("");

            var settings = new AppSettings()
            {
                Storage = new StorageSettings()
            };

            return (configurationRoot, settings);
        }
    }
}
