using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PaymentGateway.TestKernel.DependencyInjection;

namespace PaymentGateway.Persistence.Tests.Fixtures
{
    public class DependencyInjectionFixture : IDisposable
    {
        public DependencyInjectionFixture()
        {
            Container = DIHelper.BuildIntegrationTestContainer();
        }

        public ServiceProvider Container { get; }

        public void Dispose()
        {
            Container.Dispose();
        }
    }

    [CollectionDefinition(nameof(DependencyInjectionCollection))]
    public class DependencyInjectionCollection : ICollectionFixture<DependencyInjectionFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
