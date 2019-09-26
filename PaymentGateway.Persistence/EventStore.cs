using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using Streamstone;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.EventSourcing.Core.Aggregate;
using PaymentGateway.EventSourcing.Core.Event;
using PaymentGateway.EventSourcing.Core.Exception;
using PaymentGateway.Infrastructure.Json;
using PaymentGateway.SharedKernel.Constants;

namespace PaymentGateway.Persistence
{
    public class EventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly CloudTable      _table;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings {ContractResolver = new PrivateSetterContractResolver()};

        public EventStore(IDictionary<string, CloudTable> tables, IEventPublisher publisher)
        {
            _publisher = publisher;
            _table     = tables[ContainerName.EventStore];
        }

        public async Task SaveEvents<TAggregateId>(TAggregateId aggregateId, IDomainEvent<TAggregateId>[] events, long expectedVersion) where TAggregateId : IAggregateId
        {
            var partitionKey = aggregateId.IdAsString();
            var partition    = new Partition(_table, partitionKey);

            var existent = await Stream.TryOpenAsync(partition);
            var stream = existent.Found
                ? existent.Stream
                : new Stream(partition);

            if (stream.Version != expectedVersion)
            {
                throw new ConcurrencyException($"Stream Partition version [{stream.Version}] not equals event version [{expectedVersion}] PartitionKey: [{aggregateId}]");
            }

            try
            {
                await Stream.WriteAsync(stream, events.Select(ToEventData).ToArray());
            }
            catch (ConcurrencyConflictException e)
            {
                throw new ConcurrencyException($"PartitionKey: [{partitionKey}] - {e.Message}", e);
            }

            var tasks = new List<Task>(events.Length);
            foreach (var @event in events)
            {
                // publish current event to the bus for further processing by subscribers
                tasks.Add(_publisher.Publish(@event));
            }

            await Task.WhenAll(tasks);
        }

        // collect all processed events for given aggregate and return them as a list
        // used to build up an aggregate from its history (Domain.LoadsFromHistory)
        public async Task<List<IDomainEvent<TAggregateId>>> GetEventsForAggregate<TAggregateId>(TAggregateId aggregateId) where TAggregateId : IAggregateId
        {
            var partitionKey = aggregateId.IdAsString();
            var partition    = new Partition(_table, partitionKey);

            if (!await Stream.ExistsAsync(partition))
            {
                throw new AggregateNotFoundException(partitionKey);
            }

            var stream = await Stream.ReadAsync<EventEntity>(partition);
            var events = stream.Events.Select(ToEvent<TAggregateId>).ToList();

            return events;
        }

        private static IDomainEvent<TAggregateId> ToEvent<TAggregateId>(EventEntity e) where TAggregateId : IAggregateId
        {
            return (IDomainEvent<TAggregateId>) JsonConvert.DeserializeObject(e.Data, Type.GetType(e.Type), JsonSerializerSettings);
        }

        private static EventData ToEventData<TAggregateId>(IDomainEvent<TAggregateId> e) where TAggregateId : IAggregateId
        {
            var properties = new
            {
                Type = e.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(e)
            };

            return new EventData(EventId.None, EventProperties.From(properties));
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class EventEntity : TableEntity
        {
            public string Type { get; set; }
            public string Data { get; set; }
        }
    }
}
