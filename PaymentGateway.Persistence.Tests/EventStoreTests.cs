using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.EventSourcing.Core.Aggregate;
using PaymentGateway.EventSourcing.Core.Event;
using PaymentGateway.EventSourcing.Core.Exception;
using PaymentGateway.Infrastructure;
using PaymentGateway.Persistence.Tests.Fixtures;
using PaymentGateway.Persistence.Tests.SampleDomain;
using PaymentGateway.SharedKernel.Constants;

namespace PaymentGateway.Persistence.Tests
{
    [Collection(nameof(DependencyInjectionCollection))]
    [Trait("Category", "e2e")]
    public class EventStoreTests : IDisposable
    {
        public EventStoreTests(DependencyInjectionFixture diFixture)
        {
            var tables = diFixture.Container.GetRequiredService<IDictionary<string, CloudTable>>();

            _aggregateId = new CarId(Guid.NewGuid());
            _container        = diFixture.Container;
            _eventStoreTable  = tables[ContainerName.EventStore];
            _azureTableHelper = new AzureTableHelper<DynamicTableEntity>(4);
        }

        public void Dispose()
        {
            _azureTableHelper.DeletePartitionFromTableAsync(_aggregateId.IdAsString(), _eventStoreTable).GetAwaiter().GetResult();
        }

        private readonly CloudTable                           _eventStoreTable;
        private readonly ServiceProvider                      _container;
        private readonly CarId                                _aggregateId;
        private readonly AzureTableHelper<DynamicTableEntity> _azureTableHelper;

        [Theory]
        [InlineData(42)]
        public async Task Should_be_able_to_persist_events(int numberOfEvents)
        {
            // Arrange
            var eventStore = _container.GetRequiredService<IEventStore>();
            var events = Enumerable.Repeat(new CarHasTraveledADistance(_aggregateId, 2), numberOfEvents)
                                   .ToArray()
                                   .As<IDomainEvent<CarId>[]>();

            // Act
            await eventStore.SaveEvents(_aggregateId, events, 0);
            var result = await eventStore.GetEventsForAggregate(_aggregateId);

            // Assert
            result.Should().NotBeNull().And.NotContainNulls();
        }

        [Fact]
        public void Should_throw_an_exception_when_aggregate_is_not_found()
        {
            // Arrange
            var eventStore = _container.GetRequiredService<IEventStore>();
            var id         = new CarId(_aggregateId.Id);

            // Act
            Func<Task> func = async () => await eventStore.GetEventsForAggregate(id);

            // Assert
            func.Should().Throw<AggregateNotFoundException>();
        }

        [Fact]
        public async Task Should_throw_an_exception_when_expected_version_is_out_of_sync()
        {
            // Arrange
            var eventStore = _container.GetRequiredService<IEventStore>();
            var id         = new CarId(_aggregateId.Id);
            var car        = new Car(id);
            car.Travel(10);

            var domainEvents = car.GetUncommittedChanges().ToArray().As<IDomainEvent<IAggregateId>[]>();


            // Act : Save events a first time, and then try again : it should throw an exception the second time
            Func<Task> func = async () => await eventStore.SaveEvents(id, domainEvents, 0);
            await func();

            // Assert
            func.Should().Throw<ConcurrencyException>();
        }
    }
}
