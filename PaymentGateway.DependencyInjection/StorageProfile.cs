using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.SharedKernel.Constants;
using PaymentGateway.SharedKernel.Models;

namespace PaymentGateway.DependencyInjection
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public static class StorageProfile
    {
        public static void Register(IServiceCollection serviceCollection, AppSettings appSettings)
        {
            LoadAzureTable(serviceCollection, appSettings);
        }

        private static void LoadAzureTable(IServiceCollection builder, AppSettings appSettings)
        {
            builder.AddSingleton<CloudStorageAccount>(provider =>
            {
                var connectionString = appSettings.Storage.ConnectionString;
                var storageAccount   = CloudStorageAccount.Parse(connectionString);

                OptimizeTableConnection(storageAccount);
                return storageAccount;
            });

            builder.AddSingleton<CloudTableClient>(provider =>
            {
                var tableClient = provider.GetRequiredService<CloudStorageAccount>().CreateCloudTableClient();
                return tableClient;
            });

            builder.AddSingleton<IDictionary<string, CloudTable>>(provider =>
            {
                var client = provider.GetRequiredService<CloudTableClient>();
                var result = new Dictionary<string, CloudTable>
                {
                    {ContainerName.EventStore, client.GetTableReference(ContainerName.EventStore)},
                    {ContainerName.PaymentProjection, client.GetTableReference(ContainerName.PaymentProjection)},
                };

                return result;
            });
        }

        private static void OptimizeTableConnection(CloudStorageAccount storageAccount)
        {
            var tableServicePoint = ServicePointManager.FindServicePoint(storageAccount.TableEndpoint);
            tableServicePoint.UseNagleAlgorithm = false;
            tableServicePoint.Expect100Continue = false;
            tableServicePoint.ConnectionLimit   = 100;
        }
    }
}
