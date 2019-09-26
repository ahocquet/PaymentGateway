using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.Infrastructure;
using PaymentGateway.Persistence.Tests.Fixtures;
using PaymentGateway.Persistence.Tests.SampleDomain;
using PaymentGateway.SharedKernel.Constants;

namespace PaymentGateway.Persistence.Tests
{
    [Collection(nameof(DependencyInjectionCollection))]
    [Trait("Category", "e2e")]
    public class RepositoryTests : IDisposable
    {
        public RepositoryTests(DependencyInjectionFixture diFixture)
        {
            var tables = diFixture.Container.GetRequiredService<IDictionary<string, CloudTable>>();

            // These variables are reset per unit test 
            _carId            = new CarId(Guid.NewGuid());
            _azureTableHelper = new AzureTableHelper<DynamicTableEntity>();
            _repository       = diFixture.Container.GetRequiredService<IRepository<Car, CarId>>();
            _eventStoreTable  = tables[ContainerName.EventStore];
        }

        public void Dispose()
        {
            _azureTableHelper.DeletePartitionFromTableAsync(_carId.IdAsString(), _eventStoreTable).Wait();
        }

        private readonly AzureTableHelper<DynamicTableEntity> _azureTableHelper;
        private readonly IRepository<Car, CarId>              _repository;
        private readonly CloudTable                           _eventStoreTable;
        private readonly CarId                                _carId;

        [Fact]
        public async Task Should_persist_an_aggregate_with_100_events()
        {
            // Arrange
            var car = new Car(_carId);
            for (var i = 1; i <= 100; i++)
            {
                car.Travel(i);
            }

            // Act
            await _repository.Save(car);
            var savedModel = await _repository.Get(_carId);

            // Assert
            savedModel.Should().BeEquivalentTo(car);
        }

        [Fact]
        public async Task Should_persist_an_aggregate_with_a_single_event()
        {
            // Arrange
            var domain = new Car(_carId);

            // Act
            await _repository.Save(domain);
            var savedModel = await _repository.Get(_carId);

            // Assert
            savedModel.Should().BeEquivalentTo(domain);
        }
    }
}
