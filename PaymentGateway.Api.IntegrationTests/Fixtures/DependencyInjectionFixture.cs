using System;
using System.Net.Http;
using Xunit;

namespace PaymentGateway.Api.IntegrationTests.Fixtures
{
    public class DependencyInjectionFixture : IDisposable
    {
        public DependencyInjectionFixture()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:55666"),
            };
        }

        public HttpClient HttpClient { get; }

        public void Dispose()
        {
            HttpClient.Dispose();
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
